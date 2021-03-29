using FluentValidation;

namespace Application.Users.Commands.UpsertUser {
  public class UpsertUserCommandValidator : AbstractValidator<UpsertUserCommand> {
    public UpsertUserCommandValidator() {
      RuleFor(c => c.FirstName).NotEmpty();
      RuleFor(c => c.LastName).NotEmpty();
      RuleFor(c => c.PhoneNumber).NotEmpty();
    }
  }
}
