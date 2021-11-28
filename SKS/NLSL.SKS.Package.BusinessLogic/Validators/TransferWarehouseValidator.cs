using FluentValidation;

using NLSL.SKS.Package.BusinessLogic.Entities;


namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class TransferWarehouseValidator : BaseValidator<TransferWarehouse>
    {
        public TransferWarehouseValidator()
        {
            RuleFor(p => p.HopType).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.Code).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.Description).NotNull().WithMessage("{PropertyName} was null")
                .Matches("^[A-Za-z0-9öÖäÄüÜ -]*$").WithMessage("{PropertyName} does not Match ^[A-Za-z0-9öÖäÄüÜ -]*$ Regex");
            RuleFor(p => p.ProcessingDelayMins).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.LocationName).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.LocationCoordinates).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.RegionGeometry).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.LogisticsPartner).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.LogisticsPartnerUrl).NotNull().WithMessage("{PropertyName} was null");
        }
    }
}