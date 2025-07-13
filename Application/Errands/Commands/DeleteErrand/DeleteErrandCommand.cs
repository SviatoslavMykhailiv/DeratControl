using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Errands.Commands.DeleteErrand
{
    public record DeleteErrandCommand : IRequest
    {
        public DeleteErrandCommand(Guid errandId)
        {
            ErrandId = errandId;
        }

        public Guid ErrandId { get; }

        public class DeleteErrandCommandHandler : BaseRequestHandler<DeleteErrandCommand>
        {
            private readonly IDeratControlDbContext db;

            public DeleteErrandCommandHandler(
              IDeratControlDbContext db,
              ICurrentDateService currentDateService,
              ICurrentUserProvider currentUserProvider) : base(currentDateService, currentUserProvider)
            {
                this.db = db;
            }

            protected override async Task Handle(RequestContext context, DeleteErrandCommand request, CancellationToken cancellationToken)
            {
                var errand = await db.Errands.FirstOrDefaultAsync(e => e.Id == request.ErrandId, cancellationToken);

                if (errand is null)
                    return;

                db.Errands.Remove(errand);
                await db.SaveChangesAsync(cancellationToken);                
            }
        }
    }
}
