using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth {
  public class AuthCommand : IRequest<AuthResponse> {
    public string UserName { get; set; }
    public string Password { get; set; }

    public class AuthCommandHandler : IRequestHandler<AuthCommand, AuthResponse> {
      private readonly IAuthService authService;

      public AuthCommandHandler(IAuthService authService) {
        this.authService = authService;
      }

      public async Task<AuthResponse> Handle(AuthCommand request, CancellationToken cancellationToken) {
        var token = await authService.SignIn(request.UserName, request.Password);
        return new AuthResponse { Token = token  };
      }
    }
  }
}
