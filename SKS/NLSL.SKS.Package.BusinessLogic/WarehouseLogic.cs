using System.Collections.Generic;

using FluentValidation;
using FluentValidation.Results;

using NLSL.SKS.Pacakge.DataAccess.Interfaces;
using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.BusinessLogic.Validators;

namespace NLSL.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private IValidator<WarehouseCode> _warehouseCodeValidator;
        private IValidator<Warehouse> _warehouseValidator;
        private IWarehouseRepository _parcelRepository;
        public WarehouseLogic(IValidator<Warehouse> warehouseValidator, IValidator<WarehouseCode> warehouseCodeValidator, IWarehouseRepository parcelRepository)
        {
            _warehouseValidator = warehouseValidator;
            _warehouseCodeValidator = warehouseCodeValidator;
            _parcelRepository = parcelRepository;
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