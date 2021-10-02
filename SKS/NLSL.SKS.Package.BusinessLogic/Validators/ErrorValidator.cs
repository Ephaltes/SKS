using NLSL.SKS.Package.BusinessLogic.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class ErrorValidator : BaseValidator<Error>
    {
        public ErrorValidator()
        {
            RuleFor(p => p.ErrorMessage).NotNull().WithMessage("{PropertyName} was null");
        }
    }
}