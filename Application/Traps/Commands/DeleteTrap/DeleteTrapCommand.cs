using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Traps.Commands.DeleteTrap
{
    public record DeleteTrapCommand : IRequest
    {
        public DeleteTrapCommand(Guid trapId)
        {
            TrapId = trapId;
        }

        public Guid TrapId { get; }

        public class DeleteTrapCommandHandler : BaseRequestHandler<DeleteTrapCommand, Unit>
        {
            private readonly IDeratControlDbContext db;
            private readonly IMemoryCache cache;

            public DeleteTrapCommandHandler(
              IMemoryCache cache,
              IDeratControlDbContext db,
              ICurrentDateService currentDateService,
              ICurrentUserProvider currentUserProvider) : base(currentDateService, currentUserProvider)
            {
                this.cache = cache;
                this.db = db;
            }

            protected override async Task<Unit> Handle(RequestContext context, DeleteTrapCommand request, CancellationToken cancellationToken)
            {
                var trap = await db.Traps.FindAsync(new object[] { request.TrapId }, cancellationToken);

                if (trap is null)
                    return Unit.Value;

                var inUse = await db.Points.AnyAsync(p => p.TrapId == request.TrapId, cancellationToken: cancellationToken);

                if (inUse)
                    throw new BadRequestException("Пастка використовується.");

                db.Traps.Remove(trap);

                await db.SaveChangesAsync(cancellationToken);

                cache.Remove($"{nameof(Trap)}-{context.CurrentUser.UserId}");

                return Unit.Value;
            }
        }
    }
}
