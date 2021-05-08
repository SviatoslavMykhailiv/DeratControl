using Application.Common.Dtos;
using Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.UpsertUser {
  public class UpsertUserCommand : UserDto, IRequest<Guid> {
    public class UpsertUserCommandHandler : IRequestHandler<UpsertUserCommand, Guid> {
      private readonly IUserManagerService userManagerService;

      public UpsertUserCommandHandler(IUserManagerService userManagerService) {
        this.userManagerService = userManagerService;
      }

      public async Task<Guid> Handle(UpsertUserCommand request, CancellationToken cancellationToken) {
        return await userManagerService.SaveUser(request, cancellationToken);
      }
    }
  }
}
