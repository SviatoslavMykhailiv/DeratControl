using FluentValidation;

namespace Application.Traps.Commands.UpsertTrap
{
    public class UpsertTrapCommandValidator : AbstractValidator<UpsertTrapCommand>
    {
        public UpsertTrapCommandValidator()
        {
            RuleFor(c => c.Color).NotEmpty();
            RuleFor(c => c.TrapName).NotEmpty();
        }
    }
}
