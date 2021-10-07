using System.Collections.Generic;

using FluentValidation;
using FluentValidation.Results;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.BusinessLogic.Validators;

namespace NLSL.SKS.Package.BusinessLogic
{
    public class WarehouseManagement : IWarehouseManagement
    {
        private IValidator<WarehouseCode> _warehouseCodeValidator;
        private IValidator<Warehouse> _warehouseValidator;
        public WarehouseManagement(IValidator<Warehouse> warehouseValidator, IValidator<WarehouseCode> warehouseCodeValidator)
        {
            _warehouseValidator = warehouseValidator;
            _warehouseCodeValidator = warehouseCodeValidator;
        }
        public Warehouse? Get(WarehouseCode warehouseCode)
        {
            ValidationResult result = _warehouseCodeValidator.Validate(warehouseCode);

            return result.IsValid ? new Warehouse() : null;
        }
        public IReadOnlyCollection<Warehouse> GetAll()
        {
            return new List<Warehouse>();
        }
        public bool Add(Warehouse warehouse)
        {
            ValidationResult result = _warehouseValidator.Validate(warehouse);

            return result.IsValid;
        }
    }
}