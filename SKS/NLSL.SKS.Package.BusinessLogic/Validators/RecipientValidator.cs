using FluentValidation;

using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class RecipientValidator : BaseValidator<Recipient>
    {
        public RecipientValidator()
        {
            RuleFor(p => p.Country).NotNull().WithMessage("{PropertyName} was null");

            When(p => p.Country.ToLower().Trim() == "austria" ||
                      p.Country.ToLower().Trim() == "österreich" ||
                      p.Country.ToLower().Trim() == "at",
                () =>
                {
                    RuleFor(p => p.PostalCode)
                        .NotNull().WithMessage("{PropertyName} is Empty")
                        .Matches("^(A-)?[0-9]{4}$").WithMessage("{PropertyName} does not Match ^(A-)?[0-9]{4}$ Regex");

                    RuleFor(p => p.Street)
                        .NotNull().WithMessage("{PropertyName} is Empty")
                        .Matches("^[A-Za-z0-9öÖäÄüÜ/ -]+$").WithMessage("{PropertyName} does not Match ^[A-Za-z0-9öÖäÄüÜ /]+$ Regex");

                    RuleFor(p => p.City)
                        .NotNull().WithMessage("{PropertyName} is Empty")
                        .Matches("^[A-Za-zöÖäÄüÜ -]+$").WithMessage("{PropertyName} does not Match ^[A-Za-zöÖäÄüÜ -]+$ Regex");

                    RuleFor(p => p.Name)
                        .NotNull().WithMessage("{PropertyName} is Empty")
                        .Matches("^[A-Za-zöÖäÄüÜ -]+$").WithMessage("{PropertyName} does not Match ^[A-Za-zöÖäÄüÜ -]+$ Regex");
                });
        }
    }
}