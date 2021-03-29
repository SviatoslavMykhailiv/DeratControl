using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Supplements.Commands.UpsertSupplement {
  public class UpsertSupplementCommand : IRequest<Guid> {
    public Guid? SupplementId { get; set; }
    public string SupplementName { get; set; }
    public DateTime ExpirationDate { get; set; }
    public byte[] Certificate { get; set; }
    public IReadOnlyCollection<SupplementFieldDto> Fields { get; set; }

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
          supplement = await context.Supplements.FindAsync(new object[] { request.SupplementId.Value }, cancellationToken);
        }
        else {
          supplement = new Supplement();
          context.Supplements.Add(supplement);
        }

        supplement.ExpirationDate = request.ExpirationDate;
        supplement.SupplementName = supplement.SupplementName;
        supplement.CertificatePath = await fileStorage.SaveCertificate(supplement.SupplementId, request.Certificate);

        await context.SaveChangesAsync(cancellationToken);

        cache.Remove(nameof(Supplement));

        return supplement.SupplementId;
      }
    }
  }
}
