using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Supplements.Commands.UpsertSupplement {
  public class UpsertSupplementCommand : IRequest<Guid> {
    public Guid? SupplementId { get; init; }
    public string SupplementName { get; init; }
    public DateTime ExpirationDate { get; init; }
    public byte[] Certificate { get; init; }

    public class UpsertSupplementCommandHandler : IRequestHandler<UpsertSupplementCommand, Guid> {
      private readonly IDeratControlDbContext context;
      private readonly IFileStorage fileStorage;
      private readonly IMemoryCache cache;

      public UpsertSupplementCommandHandler(IDeratControlDbContext context, IFileStorage fileStorage, IMemoryCache cache) {
        this.context = context;
        this.fileStorage = fileStorage;
        this.cache = cache;
      }

      public async Task<Guid> Handle(UpsertSupplementCommand request, CancellationToken cancellationToken) {
        Supplement supplement;

        if (request.SupplementId.HasValue) {
          supplement = await context
            .Supplements
            .FirstOrDefaultAsync(s => s.Id == request.SupplementId.Value, cancellationToken);
        }
        else {
          supplement = new Supplement();
          context.Supplements.Add(supplement);
        }

        supplement.ExpirationDate = request.ExpirationDate;
        supplement.SupplementName = request.SupplementName;

        
        
        await context.SaveChangesAsync(cancellationToken);
        //await fileStorage.SaveFile(supplement.CertificatePath, request.Certificate);

        cache.Remove(nameof(Supplement));

        return supplement.Id;
      }
    }
  }
}
