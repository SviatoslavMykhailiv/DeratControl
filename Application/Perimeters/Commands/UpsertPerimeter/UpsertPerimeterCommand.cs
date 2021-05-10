using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
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
    public string SchemeImage { get; set; }
    public int LeftLoc { get; set; }
    public int TopLoc { get; set; }
    public decimal Scale { get; init; }
    public ICollection<PointDto> Points { get; set; } = new List<PointDto>();

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
            .FirstOrDefaultAsync(p => p.Id == request.PerimeterId, cancellationToken: cancellationToken);
        }
        else {
          perimeter = new Perimeter { Facility = await GetFacility(request.FacilityId, cancellationToken) ?? throw new NotFoundException() };
          context.Perimeters.Add(perimeter);
        }

        var supplements = await GetSupplements();
        var traps = await GetTraps();
        
        

        perimeter.LeftLoc = request.LeftLoc;
        perimeter.TopLoc = request.TopLoc;
        perimeter.PerimeterName = request.PerimeterName;
        perimeter.Scale = request.Scale;
        var image = (Image)request.SchemeImage;
        
        if (image is not null) {
          perimeter.GenerateSchemePath(image.Format);
          await fileStorage.SaveFile(perimeter.SchemeImagePath, image);
        }

        var inputPointList = request.Points.Where(p => p.PointId.HasValue).ToDictionary(p => p.PointId);

        foreach(var point in perimeter.Points.ToList()) {
          if (inputPointList.ContainsKey(point.Id)) 
            continue;

          perimeter.RemovePoint(point.Id);
        }

        foreach (var point in request.Points)
          perimeter.SetPoint(
            point.PointId, 
            point.Order, 
            point.LeftLoc, 
            point.TopLoc, 
            traps[point.TrapId], 
            supplements[point.SupplementId]);

        await context.SaveChangesAsync(cancellationToken);
        

        return perimeter.Id;
      }

      private Task<Dictionary<Guid, Trap>> GetTraps() => context.Traps.ToDictionaryAsync(s => s.Id);
      private Task<Dictionary<Guid, Supplement>> GetSupplements() => context.Supplements.ToDictionaryAsync(s => s.Id);
      private ValueTask<Facility> GetFacility(Guid facilityId, CancellationToken cancellationToken) =>
        context.Facilities.FindAsync(new object[] { facilityId }, cancellationToken);
    }
  }
}
