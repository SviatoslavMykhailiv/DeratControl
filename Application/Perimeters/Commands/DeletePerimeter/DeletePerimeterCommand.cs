using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Perimeters.Commands.DeletePerimeter
{
    public record DeletePerimeterCommand : IRequest
    {
        public DeletePerimeterCommand(Guid perimeterId)
        {
            PerimeterId = perimeterId;
        }

        public Guid PerimeterId { get; }

        public class DeletePerimeterCommandHandler : IRequestHandler<DeletePerimeterCommand>
        {
            private readonly IDeratControlDbContext db;

            public DeletePerimeterCommandHandler(IDeratControlDbContext db)
            {
                this.db = db;
            }

            public async Task Handle(DeletePerimeterCommand request, CancellationToken cancellationToken)
            {
                var perimeter = await db.Perimeters.FindAsync(new object[] { request.PerimeterId }, cancellationToken);

                if (perimeter is null)
                    return;

                var isUsed = await db
                  .Errands
                  .AnyAsync(e => e.FacilityId == perimeter.FacilityId, cancellationToken: cancellationToken);

                if (isUsed)
                    throw new BadRequestException("Периметр використовується.");

                db.Perimeters.Remove(perimeter);

                await db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
