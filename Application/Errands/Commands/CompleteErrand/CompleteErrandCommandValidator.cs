using FluentValidation;

namespace Application.Errands.Commands.CompleteErrand {
  public class CompleteErrandCommandValidator : AbstractValidator<CompleteErrandCommand> {
    public CompleteErrandCommandValidator() {
      RuleFor(c => c.Report).NotEmpty();
      RuleFor(c => c.SecurityCode).NotEmpty();
    }
  }
}
