using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Facilities.Commands.DeleteFacility
{
    public record DeleteFacilityCommand : IRequest
    {
        public DeleteFacilityCommand(Guid facilityId)
        {
            FacilityId = facilityId;
        }

        public Guid FacilityId { get; }

        public class DeleteFacilityCommandHandler : IRequestHandler<DeleteFacilityCommand>
        {
            private readonly IDeratControlDbContext db;

            public DeleteFacilityCommandHandler(IDeratControlDbContext db)
            {
                this.db = db;
            }

            public async Task Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
            {
                var facility = await db.Facilities.FindAsync(new object[] { request.FacilityId }, cancellationToken);

                if (facility is null)
                    return;

                var isUsed = await db
                  .Errands
                  .AnyAsync(e => e.FacilityId == request.FacilityId, cancellationToken: cancellationToken);

                if (isUsed)
                    throw new BadRequestException("Об'єкт використовується.");

                db.Facilities.Remove(facility);

                await db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
