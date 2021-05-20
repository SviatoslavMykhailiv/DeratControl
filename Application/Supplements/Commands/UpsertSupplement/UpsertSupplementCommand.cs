using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
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

    public class UpsertSupplementCommandHandler : BaseRequestHandler<UpsertSupplementCommand, Guid> {
      private readonly IDeratControlDbContext db;
      private readonly IFileStorage fileStorage;
      private readonly IMemoryCache cache;

      public UpsertSupplementCommandHandler(
        ICurrentDateService currentDateService,
        ICurrentUserProvider currentUserProvider,
        IDeratControlDbContext db,
        IFileStorage fileStorage,
        IMemoryCache cache) : base(currentDateService, currentUserProvider) {
        this.db = db;
        this.fileStorage = fileStorage;
        this.cache = cache;
      }

      protected override async Task<Guid> Handle(RequestContext context, UpsertSupplementCommand request, CancellationToken cancellationToken) {
        Supplement supplement;

        if (request.SupplementId.HasValue) {
          supplement = await db
            .Supplements
            .FirstOrDefaultAsync(s => s.Id == request.SupplementId.Value, cancellationToken);
        }
        else {
          supplement = new Supplement { ProviderId = context.CurrentUser.UserId };
          db.Supplements.Add(supplement);
        }

        var image = (Image)request.Certificate;

        supplement.ExpirationDate = request.ExpirationDate;
        supplement.SupplementName = request.SupplementName;

        if (image is not null) {
          supplement.GeneratePath(image.Format);
          await fileStorage.SaveFile(supplement.CertificateFilePath, image);
        }

        await db.SaveChangesAsync(cancellationToken);

        cache.Remove($"{nameof(Supplement)}-{context.CurrentUser.UserId}");

        return supplement.Id;
      }
    }
  }
}
