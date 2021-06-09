using FluentValidation;

namespace Application.Auth
{
    public class AuthCommandValidator : AbstractValidator<AuthCommand>
    {
        public AuthCommandValidator()
        {
            RuleFor(c => c.UserName).NotEmpty();
            RuleFor(c => c.Password).NotEmpty();
        }
    }
}
