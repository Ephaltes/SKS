using FluentValidation;

using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class TrackingIdValidator: BaseValidator<TrackingId>
    {
        public TrackingIdValidator()
        {
            RuleFor(x=> x.Id).NotNull().WithMessage("{PropertyName} was null")
                .Matches("^[A-Z0-9]{9}$").WithMessage("{PropertyName} does not Match ^[A-Z0-9]{9}$ Regex");
        }
    }
}