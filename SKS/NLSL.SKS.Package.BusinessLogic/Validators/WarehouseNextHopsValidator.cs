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

            RuleFor(p => p.Hop)
                .SetInheritanceValidator(v =>
                                         {
                                             v.Add(new TruckValidator());
                                             v.Add(new TransferWarehouseValidator());
                                             //v.Add(new WarehouseValidator());
                                         });
        }
    }
}