using NLSL.SKS.Package.BusinessLogic.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class GeoCoordinateValidator : BaseValidator<GeoCoordinate>
    {
        public GeoCoordinateValidator()
        {
            RuleFor(p => p.Lon).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.Lat).NotNull().WithMessage("{PropertyName} was null");
        }
    }
}

