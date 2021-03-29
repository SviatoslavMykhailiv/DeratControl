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

namespace Application.Perimeters.Commands.UpsertPerimeter {
  public class UpsertPerimeterCommand : IRequest<Guid> {
    public UpsertPerimeterCommand() {
      Points = new List<PointDto>();
    }

    public Guid? PerimeterId { get; set; }
    public Guid FacilityId { get; set; }
    public string PerimeterName { get; set; }
    public byte[] SchemeImage { get; set; }
    public int LeftLoc { get; set; }
    public int TopLoc { get; set; }
    public ICollection<PointDto> Points { get; set; }

    public class UpsertPerimeterCommandHandler : IRequestHandler<UpsertPerimeterCommand, Guid> {
      private readonly IDeratControlDbContext context;
      private readonly IFileStorage fileStorage;

      public UpsertPerimeterCommandHandler(IDeratControlDbContext context, IFileStorage fileStorage) {
        this.context = context;
        this.fileStorage = fileStorage;
      }

      public async Task<Guid> Handle(UpsertPerimeterCommand request, CancellationToken cancellationToken) {
        Perimeter perimeter;

        if (request.PerimeterId.HasValue) {
          perimeter = await context
            .Perimeters
            .Include(p => p.Points)
            .FirstOrDefaultAsync(p => p.PerimeterId == request.PerimeterId);
        }
        else {
          perimeter = new Perimeter();
          context.Perimeters.Add(perimeter);
        }

        var supplements = await GetSupplements();
        var traps = await GetTraps();

        perimeter.Facility = await GetFacility(request.FacilityId, cancellationToken) ?? throw new NotFoundException();
        perimeter.LeftLoc = request.LeftLoc;
        perimeter.TopLoc = request.TopLoc;
        perimeter.PerimeterName = request.PerimeterName;
        perimeter.SchemeImagePath = Guid.NewGuid().ToString();
        perimeter.UpdatePoints(ToPoints(request.Points, supplements, traps));

        await context.SaveChangesAsync(cancellationToken);
        await fileStorage.SavePerimeterScheme(perimeter.SchemeImagePath, request.SchemeImage);

        return perimeter.PerimeterId;
      }

      private Task<Dictionary<Guid, Trap>> GetTraps() => context.Traps.ToDictionaryAsync(s => s.TrapId);
      private Task<Dictionary<Guid, Supplement>> GetSupplements() => context.Supplements.ToDictionaryAsync(s => s.SupplementId);
      private ValueTask<Facility> GetFacility(Guid facilityId, CancellationToken cancellationToken) =>
        context.Facilities.FindAsync(new object[] { facilityId }, cancellationToken);

      private List<Point> ToPoints(
        IEnumerable<PointDto> points,
        Dictionary<Guid, Supplement> supplements,
        Dictionary<Guid, Trap> traps) => points.Select(p => new Point {
          Order = p.Order,
          LeftLoc = p.LeftLoc,
          TopLoc = p.TopLoc,
          Trap = traps[p.TrapId],
          Supplement = supplements[p.SupplementId]
        }).ToList();
    }
  }
}
