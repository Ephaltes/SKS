using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.BusinessLogic.Validators;

namespace NLSL.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private IValidator<WarehouseCode> _warehouseCodeValidator;
        private IValidator<Warehouse> _warehouseValidator;
        private IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;
        public WarehouseLogic(IValidator<Warehouse> warehouseValidator, IValidator<WarehouseCode> warehouseCodeValidator, IWarehouseRepository warehouseRepository, IMapper mapper)
        {
            _warehouseValidator = warehouseValidator;
            _warehouseCodeValidator = warehouseCodeValidator;
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
        }
        public Warehouse? Get(WarehouseCode warehouseCode)
        {
            ValidationResult result = _warehouseCodeValidator.Validate(warehouseCode);
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }
            Package.DataAccess.Entities.Warehouse? warehouseFromDb = _warehouseRepository.GetWarehouseByCode(warehouseCode.Code);
            if (warehouseFromDb == null)
            {
                return null;
            }
            return _mapper.Map<Package.DataAccess.Entities.Warehouse,Warehouse>(warehouseFromDb);
        }
        public IReadOnlyCollection<Warehouse> GetAll()
        {
            var warehouseFromDb = _warehouseRepository.GetAllWarehouses();
            
            return _mapper.Map<IReadOnlyCollection<Package.DataAccess.Entities.Warehouse>,IReadOnlyCollection<Warehouse>>(warehouseFromDb.ToList());
        }
        public bool Add(Warehouse warehouse)
        {
            ValidationResult result = _warehouseValidator.Validate(warehouse);
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }

            var mappedWarehouse = _mapper.Map<Warehouse, Package.DataAccess.Entities.Warehouse>(warehouse);
            var warehouseCode = _warehouseRepository.Create(mappedWarehouse);
            
            
            return !string.IsNullOrWhiteSpace(warehouseCode);
        }
    }
}