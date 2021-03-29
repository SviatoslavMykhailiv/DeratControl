using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.UpsertUser {
  public class UpsertUserCommand : IRequest<Guid> {
    public Guid? UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public Guid? FacilityId { get; set; }
    public IReadOnlyCollection<Guid> Facilities { get; set; }

    public class UpsertUserCommandHandler : IRequestHandler<UpsertUserCommand, Guid> {
      private readonly IUserManagerService userManagerService;

      public UpsertUserCommandHandler(IUserManagerService userManagerService) {
        this.userManagerService = userManagerService;
      }

      public async Task<Guid> Handle(UpsertUserCommand request, CancellationToken cancellationToken) {
        return await userManagerService.SaveUser(
          request.UserName, 
          request.Password, 
          request.FirstName, 
          request.LastName, 
          request.PhoneNumber, 
          request.FacilityId);
      }
    }
  }
}
