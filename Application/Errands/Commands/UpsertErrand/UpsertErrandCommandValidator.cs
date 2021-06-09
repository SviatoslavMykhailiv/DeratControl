using FluentValidation;

namespace Application.Errands.Commands.UpsertErrand
{
    public class UpsertErrandCommandValidator : AbstractValidator<UpsertErrandCommand>
    {
        public UpsertErrandCommandValidator()
        {
            RuleFor(c => c.Description).NotEmpty();
        }
    }
}
