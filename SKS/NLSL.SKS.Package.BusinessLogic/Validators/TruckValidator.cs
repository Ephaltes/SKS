using FluentValidation;

using NLSL.SKS.Package.BusinessLogic.Entities;


namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class TruckValidator : BaseValidator<Truck>
    {
        public TruckValidator()
        {
            RuleFor(p => p.HopType).NotNull().WithMessage("{PropertyName} was null");


            RuleFor(p => p.Code).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.Description).NotNull().WithMessage("{PropertyName} was null")
                .Matches("^[A-Za-z0-9öÖäÄüÜ -]*$").WithMessage("{PropertyName} does not Match ^[A-Za-z0-9öÖäÄüÜ -]*$ Regex");
            RuleFor(p => p.ProcessingDelayMins).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.LocationName).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.LocationCoordinates).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.RegionGeoJson).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.NumberPlate).NotNull().WithMessage("{PropertyName} was null");
        }
    }
}