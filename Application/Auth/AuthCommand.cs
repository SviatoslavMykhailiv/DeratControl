using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth
{
    public class AuthCommand : IRequest<AuthResponse>
    {
        public string UserName { get; init; }
        public string Password { get; init; }

        public class AuthCommandHandler : IRequestHandler<AuthCommand, AuthResponse>
        {
            private readonly IAuthService authService;

            public AuthCommandHandler(IAuthService authService)
            {
                this.authService = authService;
            }

            public async Task<AuthResponse> Handle(AuthCommand request, CancellationToken cancellationToken)
            {
                var token = await authService.SignIn(request.UserName, request.Password);
                return new AuthResponse(token);
            }
        }
    }
}
