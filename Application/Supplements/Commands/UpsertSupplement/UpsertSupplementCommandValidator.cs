using FluentValidation;

namespace Application.Supplements.Commands.UpsertSupplement {
  public class UpsertSupplementCommandValidator : AbstractValidator<UpsertSupplementCommand> {
    public UpsertSupplementCommandValidator() {
      RuleFor(c => c.SupplementName).NotEmpty();
    }
  }
}
