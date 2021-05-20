using Application.Common;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.UpsertUser {
  public class UpsertUserCommand : UserDto, IRequest<Guid> {
    public class UpsertUserCommandHandler : BaseRequestHandler<UpsertUserCommand, Guid> {
      private readonly IUserManagerService userManagerService;

      public UpsertUserCommandHandler(ICurrentDateService currentDateService, ICurrentUserProvider currentUserProvider, IUserManagerService userManagerService) : base(currentDateService, currentUserProvider) {
        this.userManagerService = userManagerService;
      }

      protected override async Task<Guid> Handle(RequestContext context, UpsertUserCommand request, CancellationToken cancellationToken) {
        return await userManagerService.SaveUser(context.CurrentUser.UserId, request, cancellationToken);
      }
    }
  }
}
