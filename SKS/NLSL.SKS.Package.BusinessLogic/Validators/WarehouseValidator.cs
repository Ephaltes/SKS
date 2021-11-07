using NLSL.SKS.Package.BusinessLogic.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class WarehouseValidator : BaseValidator<Warehouse>
    {
        public WarehouseValidator()
        {
            RuleFor(p => p.HopType).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.Code).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.Description).NotNull().WithMessage("{PropertyName} was null")
                .Matches("^[A-Za-z0-9öÖäÄüÜ -]*$").WithMessage("{PropertyName} does not Match ^[A-Za-z0-9öÖäÄüÜ -]*$ Regex");
            RuleFor(p => p.ProcessingDelayMins).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.LocationName).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.LocationCoordinates).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p=> p.NextHops).NotEmpty().WithMessage("{PropertyName} was null");
            RuleFor(p => p.Level).NotEmpty().WithMessage("{PropertyName} was null");
            RuleForEach(p => p.NextHops).SetValidator(new WarehouseNextHopsValidator());
        }
    }
}
