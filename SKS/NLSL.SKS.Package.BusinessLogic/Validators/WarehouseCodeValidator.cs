using FluentValidation;

using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class WarehouseCodeValidator : AbstractValidator<WarehouseCode>
    {
        public WarehouseCodeValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}