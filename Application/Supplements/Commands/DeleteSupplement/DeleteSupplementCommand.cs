using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Supplements.Commands.DeleteSupplement {
  public record DeleteSupplementCommand : IRequest {
    public DeleteSupplementCommand(Guid supplementId) {
      SupplementId = supplementId;
    }

    public Guid SupplementId { get; }

    public class DeleteSupplementCommandHandler : BaseRequestHandler<DeleteSupplementCommand, Unit> {
      private readonly IDeratControlDbContext db;
      private readonly IMemoryCache cache;

      public DeleteSupplementCommandHandler(
        IMemoryCache cache,
        ICurrentDateService currentDateService,
        ICurrentUserProvider currentUserProvider, 
        IDeratControlDbContext db) : base(currentDateService, currentUserProvider) {
        this.cache = cache;
        this.db = db;
      }

      protected override async Task<Unit> Handle(RequestContext context, DeleteSupplementCommand request, CancellationToken cancellationToken) {
        var supplement = await db.Supplements.FindAsync(new object[] { request.SupplementId }, cancellationToken);

        if (supplement is null)
          return Unit.Value;

        db.Supplements.Remove(supplement);
        await db.SaveChangesAsync(cancellationToken);
        
        cache.Remove(nameof(Supplement));

        return Unit.Value;
      }
    }
  }
}
