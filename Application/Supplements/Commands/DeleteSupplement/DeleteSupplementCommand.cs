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

namespace Application.Supplements.Commands.DeleteSupplement
{
    public record DeleteSupplementCommand : IRequest
    {
        public DeleteSupplementCommand(Guid supplementId)
        {
            SupplementId = supplementId;
        }

        public Guid SupplementId { get; }

        public class DeleteSupplementCommandHandler : BaseRequestHandler<DeleteSupplementCommand>
        {
            private readonly IDeratControlDbContext db;
            private readonly IFileStorage fileStorage;
            private readonly IMemoryCache cache;

            public DeleteSupplementCommandHandler(
              IMemoryCache cache,
              ICurrentDateService currentDateService,
              ICurrentUserProvider currentUserProvider,
              IDeratControlDbContext db,
              IFileStorage fileStorage) : base(currentDateService, currentUserProvider)
            {
                this.cache = cache;
                this.db = db;
                this.fileStorage = fileStorage;
            }

            protected override async Task Handle(RequestContext context, DeleteSupplementCommand request, CancellationToken cancellationToken)
            {
                var supplement = await db.Supplements.FindAsync(new object[] { request.SupplementId }, cancellationToken);

                if (supplement is null)
                    return;

                var inUse = await db.Points.AnyAsync(p => p.SupplementId == request.SupplementId, cancellationToken: cancellationToken);

                if (inUse)
                    throw new BadRequestException("Засіб використовується.");

                db.Supplements.Remove(supplement);
                await db.SaveChangesAsync(cancellationToken);

                await fileStorage.Remove(supplement.CertificateFilePath);

                cache.Remove($"{nameof(Supplement)}-{context.CurrentUser.UserId}");
            }
        }
    }
}
