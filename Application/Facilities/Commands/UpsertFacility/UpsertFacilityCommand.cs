using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Facilities.Commands.UpsertFacility
{
    public class UpsertFacilityCommand : IRequest<Guid>
    {
        public Guid? FacilityId { get; init; }
        public string CompanyName { get; init; }
        public string Name { get; init; }
        public string City { get; init; }
        public string Address { get; init; }
        public string SecurityCode { get; init; }

        public class UpsertFacilityCommandHandler : BaseRequestHandler<UpsertFacilityCommand, Guid>
        {
            private readonly IDeratControlDbContext db;

            public UpsertFacilityCommandHandler(
              ICurrentDateService currentDateService,
              ICurrentUserProvider currentUserProvider,
              IDeratControlDbContext db) : base(currentDateService, currentUserProvider)
            {
                this.db = db;
            }

            protected override async Task<Guid> Handle(RequestContext context, UpsertFacilityCommand request, CancellationToken cancellationToken)
            {
                Facility facility;

                if (request.FacilityId.HasValue)
                {
                    facility = await db.Facilities.FindAsync(new object[] { request.FacilityId.Value }, cancellationToken);
                }
                else
                {
                    facility = new Facility { ProviderId = context.CurrentUser.UserId };
                    db.Facilities.Add(facility);
                }

                facility.CompanyName = request.CompanyName;
                facility.Name = request.Name;
                facility.City = request.City;
                facility.SecurityCode = request.SecurityCode;
                facility.Address = request.Address;

                await db.SaveChangesAsync(cancellationToken);

                return facility.Id;
            }
        }
    }
}
