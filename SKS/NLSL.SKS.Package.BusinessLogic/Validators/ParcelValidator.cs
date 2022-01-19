using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class ParcelValidator : BaseValidator<Parcel>
    {
        public ParcelValidator()
        {
            RuleFor(p => p.Recipient).NotNull().WithMessage("{PropertyName} was null").SetValidator(new RecipientValidator());
            RuleFor(p => p.Sender).NotNull().WithMessage("{PropertyName} was null").SetValidator(new RecipientValidator());
            RuleFor(p => p.VisitedHops).NotNull().WithMessage("{PropertyName} was null");
            RuleForEach(p=> p.VisitedHops).SetValidator(new HopArrivalValidator());
            RuleFor(p => p.FutureHops).NotNull().WithMessage("{PropertyName} was null");
            RuleForEach(p=> p.FutureHops).SetValidator(new HopArrivalValidator());
            RuleFor(p => p.State).NotNull().WithMessage("{PropertyName} was null");

            RuleFor(p => p.TrackingId).Matches("^[A-Z0-9]{9}$")
                .When(x=> !string.IsNullOrEmpty(x.TrackingId))
                .WithMessage("{PropertyName} does not Match ^[A-Z0-9]{9}$ Regex");
            RuleFor(p => p.Weight).NotNull().WithMessage("{PropertyName} was null")
                .GreaterThan(0.0f).WithMessage("{PropertyName} is to light");
        }
    }
}
