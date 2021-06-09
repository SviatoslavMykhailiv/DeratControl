using FluentValidation;

namespace Application.Facilities.Commands.UpsertFacility
{
    public class UpsertFacilityCommandValidator : AbstractValidator<UpsertFacilityCommand>
    {
        public UpsertFacilityCommandValidator()
        {
            RuleFor(c => c.Address).NotEmpty();
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.CompanyName).NotEmpty();
            RuleFor(c => c.SecurityCode).NotEmpty().MaximumLength(6);
        }
    }
}
