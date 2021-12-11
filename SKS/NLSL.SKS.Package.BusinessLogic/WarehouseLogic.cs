using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.BusinessLogic.CustomExceptions;
using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;
using NLSL.SKS.Package.ServiceAgents.Exceptions;

namespace NLSL.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly ILogger<WarehouseLogic> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<WarehouseCode> _warehouseCodeValidator;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IValidator<Warehouse> _warehouseValidator;

        public WarehouseLogic(IValidator<Warehouse> warehouseValidator, IValidator<WarehouseCode> warehouseCodeValidator, IWarehouseRepository warehouseRepository, IMapper mapper, ILogger<WarehouseLogic> logger)
        {
            _warehouseValidator = warehouseValidator;
            _warehouseCodeValidator = warehouseCodeValidator;
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Hop? Get(WarehouseCode warehouseCode)
        {
            try
            {
                _logger.LogDebug("starting, get a warehouse");

                _logger.LogDebug("validating warehouseCode");
                ValidationResult result = _warehouseCodeValidator.Validate(warehouseCode);
                if (!result.IsValid)
                {
                    _logger.LogWarning("validation error for warehouseCode");
                    throw new BusinessLayerValidationException(result.Errors.First().ErrorMessage);
                }

                DataAccess.Entities.Hop? warehouseFromDb = _warehouseRepository.GetWarehouseByCode(warehouseCode.Code);
                if (warehouseFromDb is null)
                {
                    _logger.LogInformation("Could not find Warehouse in db");
                    throw new BusinessLayerDataNotFoundException("no warehouse found that matches code");
                }
                switch(warehouseFromDb)
                {
                    case DataAccess.Entities.Truck c: 
                        _logger.LogDebug("get warehouse complete");
                        return _mapper.Map<DataAccess.Entities.Truck,BusinessLogic.Entities.Truck>(c);
                    case DataAccess.Entities.Warehouse c: 
                        _logger.LogDebug("get warehouse complete");
                        return _mapper.Map<DataAccess.Entities.Warehouse,BusinessLogic.Entities.Warehouse>(c);
                    case DataAccess.Entities.Transferwarehouse c: 
                        _logger.LogDebug("get warehouse complete");
                        return _mapper.Map<DataAccess.Entities.Transferwarehouse,BusinessLogic.Entities.TransferWarehouse>(c);
                    default: throw new BusinessLayerDataNotFoundException("no hop found");
                }
                
            }
            catch (BusinessLayerValidationException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (BusinessLayerDataNotFoundException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new BusinessLayerExceptionBase("No Data found", e);
            }
            catch (DataAccessExceptionBase e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new BusinessLayerExceptionBase("Error in DataAccessLayer", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new BusinessLayerExceptionBase("Error in ServiceAgents", e);
            }
        }

        public IReadOnlyCollection<Warehouse> GetAll()
        {
            try
            {
                _logger.LogDebug("starting, get all warehouses");
                IReadOnlyCollection<DataAccess.Entities.Warehouse> warehouseFromDb = _warehouseRepository.GetAllWarehouses();

                _logger.LogDebug("get all warehouses complete");
                return warehouseFromDb.Select(warehouse => _mapper.Map<DataAccess.Entities.Warehouse, Warehouse>(warehouse)).ToList();
            }
            catch (DataAccessExceptionBase e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new BusinessLayerExceptionBase("Error in DataAccessLayer", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new BusinessLayerExceptionBase("Error in ServiceAgents", e);
            }
        }

        public bool ReplaceHierarchy(Warehouse warehouse)
        {
            try
            {
                _logger.LogDebug("starting, add warehouse");

                _logger.LogDebug("validating warehouse");
                ValidationResult result = _warehouseValidator.Validate(warehouse);
                if (!result.IsValid)
                {
                    _logger.LogWarning("validation error for warehouse");
                    throw new BusinessLayerValidationException(result.Errors.First().ErrorMessage);
                }

                DataAccess.Entities.Warehouse mappedWarehouse = _mapper.Map<Warehouse, DataAccess.Entities.Warehouse>(warehouse);
                _warehouseRepository.DeleteHierarchy();
                string warehouseCode = _warehouseRepository.Create(mappedWarehouse);

                _logger.LogDebug("add warehouse complete");
                return !string.IsNullOrWhiteSpace(warehouseCode);
            }
            catch (BusinessLayerValidationException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (DataAccessExceptionBase e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new BusinessLayerExceptionBase("Error in DataAccessLayer", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new BusinessLayerExceptionBase("Error in ServiceAgents", e);
            }
        }
    }
}