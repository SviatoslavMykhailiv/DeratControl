using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.SetUserAvailability
{
    public record SetUserAvailabilityCommand : IRequest
    {
        public SetUserAvailabilityCommand(Guid userId, bool available)
        {
            UserId = userId;
            Available = available;
        }

        public Guid UserId { get; }
        public bool Available { get; }

        public class SetUserAvailabilityCommandHandler : IRequestHandler<SetUserAvailabilityCommand>
        {
            private readonly IUserManagerService userManagerService;
            private readonly IDeratControlDbContext db;

            public SetUserAvailabilityCommandHandler(IUserManagerService userManagerService, IDeratControlDbContext db)
            {
                this.userManagerService = userManagerService;
                this.db = db;
            }

            public async Task<Unit> Handle(SetUserAvailabilityCommand request, CancellationToken cancellationToken)
            {
                var errandsExist = await db.Errands.AnyAsync(e => e.EmployeeId == request.UserId, cancellationToken: cancellationToken);

                if (errandsExist)
                    throw new BadRequestException("У даного працівника існують невиконані завдання.");

                await userManagerService.SetUserAvailability(request.UserId, request.Available);
                return Unit.Value;
            }
        }
    }
}
