using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Facilities.Commands.UpsertFacility {
  public class UpsertFacilityCommand : IRequest<Guid> {
    public Guid? FacilityId { get; init; }
    public string CompanyName { get; init; }
    public string Name { get; init; }
    public string City { get; init; }
    public string Address { get; init; }
    public string SecurityCode { get; init; }

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
        facility.City = request.City;
        facility.SecurityCode = request.SecurityCode;
        facility.Address = request.Address;

        await context.SaveChangesAsync(cancellationToken);

        return facility.Id;
      }
    }
  }
}
