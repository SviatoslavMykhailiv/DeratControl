using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
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
    public string Certificate { get; init; }

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

        var image = (Image)request.Certificate;

        supplement.ExpirationDate = request.ExpirationDate;
        supplement.SupplementName = request.SupplementName;
        
        if(image is not null) {
          supplement.GeneratePath(image.Format);
          await fileStorage.SaveFile(supplement.CertificateFilePath, image);
        }
       
        await context.SaveChangesAsync(cancellationToken);
        
        cache.Remove(nameof(Supplement));

        return supplement.Id;
      }
    }
  }
}
