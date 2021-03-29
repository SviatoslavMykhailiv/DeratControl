using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Facilities.Commands.UpsertFacility {
  public class UpsertFacilityCommand : IRequest<Guid> {
    public Guid? FacilityId { get; set; }
    public string CompanyName { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string SecurityCode { get; set; }

    public class UpsertFacilityCommandHandler : IRequestHandler<UpsertFacilityCommand, Guid> {
      private readonly IDeratControlDbContext context;

      public UpsertFacilityCommandHandler(IDeratControlDbContext context) {
        this.context = context;
      }

      public async Task<Guid> Handle(UpsertFacilityCommand request, CancellationToken cancellationToken) {

        Facility facility;

        if (request.FacilityId.HasValue) {
          facility = await context.Facilities.FindAsync(new object[] { request.FacilityId.Value }, cancellationToken);
        }
        else {
          facility = new Facility();
          context.Facilities.Add(facility);
        }

        facility.CompanyName = request.CompanyName;
        facility.Name = request.Name;
        facility.SecurityCode = request.SecurityCode;
        facility.Address = request.Address;

        await context.SaveChangesAsync(cancellationToken);

        return facility.FacilityId;
      }
    }
  }
}
