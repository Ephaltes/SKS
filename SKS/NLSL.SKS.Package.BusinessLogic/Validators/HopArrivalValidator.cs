using NLSL.SKS.Package.BusinessLogic.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    class HopArrivalValidator : BaseValidator<HopArrival>
    {
        public HopArrivalValidator()
        {
            RuleFor(p => p.Code).Matches("^[A-Z]{4}\\d{1,4}$").WithMessage("{PropertyName} does not Match ^[A-Z]{4}\\d{1,4}$ Regex");
            RuleFor(p => p.Description).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.DateTime).NotNull().WithMessage("{PropertyName} was null");
        }
    }
}

