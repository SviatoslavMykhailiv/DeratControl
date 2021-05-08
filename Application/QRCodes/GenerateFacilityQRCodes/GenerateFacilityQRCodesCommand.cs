using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.QRCodes.GenerateFacilityQRCodes {
  public class GenerateFacilityQRCodesCommand : IRequest<byte[]> {
    public Guid FacilityId { get; init; }
    public IReadOnlyCollection<Guid> Perimeters { get; init; } = Array.Empty<Guid>();
    public IReadOnlyCollection<Guid> Traps { get; init; } = Array.Empty<Guid>();
    public int Count { get; init; }

    public class GenerateFacilityQRCodesCommandHandler : IRequestHandler<GenerateFacilityQRCodesCommand, byte[]> {
      private readonly IDeratControlDbContext db;
      private readonly IQRListGenerator qrListGenerator;

      public GenerateFacilityQRCodesCommandHandler(
        IDeratControlDbContext db,
        IQRListGenerator qrListGenerator) {
        this.db = db;
        this.qrListGenerator = qrListGenerator;
      }

      public async Task<byte[]> Handle(GenerateFacilityQRCodesCommand request, CancellationToken cancellationToken) {
        var facility = await GetFacility(request.FacilityId, cancellationToken) ?? throw new NotFoundException();
        var traps = (await GetTrapList(request.Traps, cancellationToken)).ToDictionary(t => t.Id);
        var qrs = facility.GenerateQRList(request.Count, request.Perimeters, traps.Values);

        return qrListGenerator.Generate(qrs, facility, traps);
      }

      private async Task<Facility> GetFacility(Guid facilityId, CancellationToken cancellationToken) {
        return await db
          .Facilities
          .Include(f => f.Perimeters)
          .AsNoTracking()
          .FirstOrDefaultAsync(f => f.Id == facilityId, cancellationToken);
      }

      private async Task<IReadOnlyCollection<Trap>> GetTrapList(IReadOnlyCollection<Guid> trapIdList, CancellationToken cancellationToken) {
        return await db
          .Traps
          .Where(t => trapIdList.Contains(t.Id))
          .AsNoTracking()
          .ToListAsync(cancellationToken);
      }
    }
  }
}
