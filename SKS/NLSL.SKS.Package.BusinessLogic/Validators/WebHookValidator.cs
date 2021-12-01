using NLSL.SKS.Package.BusinessLogic.Entities;

using FluentValidation;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class WebHookValidator : BaseValidator<WebHook>
    {
        public WebHookValidator()
        {
            RuleFor(p => p.trackingId).NotNull().WithMessage("{PropertyName} was null")
                .Matches("^[A-Z0-9]{9}$").WithMessage("{PropertyName} does not Match ^[A-Z0-9]{9}$ Regex");
        }
    }
}