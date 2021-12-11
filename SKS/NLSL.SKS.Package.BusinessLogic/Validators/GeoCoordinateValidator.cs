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
            RuleFor(p => p.Location).NotNull().WithMessage("{PropertyName} was null");
        }
    }
}

