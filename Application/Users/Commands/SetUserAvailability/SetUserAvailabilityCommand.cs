using Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.SetUserAvailability {
  public record SetUserAvailabilityCommand : IRequest {
    public SetUserAvailabilityCommand(Guid userId, bool available) {
      UserId = userId;
      Available = available;
    }

    public Guid UserId { get; }
    public bool Available { get; }

    public class SetUserAvailabilityCommandHandler : IRequestHandler<SetUserAvailabilityCommand> {
      private readonly IUserManagerService userManagerService;

      public SetUserAvailabilityCommandHandler(IUserManagerService userManagerService) {
        this.userManagerService = userManagerService;
      }

      public async Task<Unit> Handle(SetUserAvailabilityCommand request, CancellationToken cancellationToken) {
        await userManagerService.SetUserAvailability(request.UserId, request.Available);
        return Unit.Value;
      }
    }
  }
}
