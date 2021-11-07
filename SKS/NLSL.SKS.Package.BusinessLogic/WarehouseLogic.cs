using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Extensions.Logging;

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
        private readonly ILogger<WarehouseLogic> _logger;
        public WarehouseLogic(IValidator<Warehouse> warehouseValidator, IValidator<WarehouseCode> warehouseCodeValidator, IWarehouseRepository warehouseRepository, IMapper mapper, ILogger<WarehouseLogic> logger)
        {
            _warehouseValidator = warehouseValidator;
            _warehouseCodeValidator = warehouseCodeValidator;
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public Warehouse? Get(WarehouseCode warehouseCode)
        {
            _logger.LogDebug("starting, get a warehouse");
            
            _logger.LogDebug("validating warehouseCode");
            ValidationResult result = _warehouseCodeValidator.Validate(warehouseCode);
            if (!result.IsValid)
            {
                _logger.LogWarning("validation error for warehouseCode");
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }

            DataAccess.Entities.Warehouse? warehouseFromDb = _warehouseRepository.GetWarehouseByCode(warehouseCode.Code);
            if (warehouseFromDb is null)
            {
                _logger.LogInformation("Could not find Warehouse in db");
                return null;
            }
            _logger.LogDebug("get warehouse complete");
            return _mapper.Map<DataAccess.Entities.Warehouse, Warehouse>(warehouseFromDb);
        }
        public IReadOnlyCollection<Warehouse> GetAll()
        {
            _logger.LogDebug("starting, get all warehouses");
            IReadOnlyCollection<DataAccess.Entities.Warehouse> warehouseFromDb = _warehouseRepository.GetAllWarehouses();
           
            _logger.LogDebug("get all warehouses complete");
            return warehouseFromDb.Select(warehouse => _mapper.Map<DataAccess.Entities.Warehouse, Warehouse>(warehouse)).ToList();
        }
        public bool Add(Warehouse warehouse)
        {
            _logger.LogDebug("starting, add warehouse");
            
            _logger.LogDebug("validating warehouse");
            ValidationResult result = _warehouseValidator.Validate(warehouse);
            if (!result.IsValid)
            {
                _logger.LogWarning("validation error for warehouse");
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }

            DataAccess.Entities.Warehouse mappedWarehouse = _mapper.Map<Warehouse, DataAccess.Entities.Warehouse>(warehouse);
            string warehouseCode = _warehouseRepository.Create(mappedWarehouse);

            _logger.LogDebug("add warehouse complete");
            return !string.IsNullOrWhiteSpace(warehouseCode);
        }
    }
}