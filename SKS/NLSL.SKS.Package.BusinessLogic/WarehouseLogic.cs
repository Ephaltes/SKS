using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.DataAccess.Interfaces;

namespace NLSL.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly IMapper _mapper;
        private readonly IValidator<WarehouseCode> _warehouseCodeValidator;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IValidator<Warehouse> _warehouseValidator;
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

            DataAccess.Entities.Warehouse? warehouseFromDb = _warehouseRepository.GetWarehouseByCode(warehouseCode.Code);
            if (warehouseFromDb is null)
            {
                return null;
            }

            return _mapper.Map<DataAccess.Entities.Warehouse, Warehouse>(warehouseFromDb);
        }
        public IReadOnlyCollection<Warehouse> GetAll()
        {
            IReadOnlyCollection<DataAccess.Entities.Warehouse> warehouseFromDb = _warehouseRepository.GetAllWarehouses();

            return _mapper.Map<IReadOnlyCollection<DataAccess.Entities.Warehouse>, IReadOnlyCollection<Warehouse>>(warehouseFromDb.ToList());
        }
        public bool Add(Warehouse warehouse)
        {
            ValidationResult result = _warehouseValidator.Validate(warehouse);
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }

            DataAccess.Entities.Warehouse mappedWarehouse = _mapper.Map<Warehouse, DataAccess.Entities.Warehouse>(warehouse);
            string warehouseCode = _warehouseRepository.Create(mappedWarehouse);


            return !string.IsNullOrWhiteSpace(warehouseCode);
        }
    }
}