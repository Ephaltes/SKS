using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class WarehouseNextHopsValidator : BaseValidator<WarehouseNextHops>
    {
        public WarehouseNextHopsValidator()
        {
            RuleFor(p => p.TraveltimeMins).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.Hop).NotNull().WithMessage("{PropertyName} was null");
        }
    }
}
