/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.BusinessLogic.CustomExceptions;
using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;
using NLSL.SKS.Package.Services.Attributes;

using Swashbuckle.AspNetCore.Annotations;

using Error = NLSL.SKS.Package.Services.DTOs.Error;

namespace NLSL.SKS.Package.Services.Controllers
{
    /// <summary>
    /// </summary>
    [ApiController]
    public class WarehouseManagementApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWarehouseLogic _warehouseLogic;
        private readonly ILogger<WarehouseManagementApiController> _logger;

        public WarehouseManagementApiController(IWarehouseLogic warehouseLogic, IMapper mapper, ILogger<WarehouseManagementApiController> logger)
        {
            _warehouseLogic = warehouseLogic;
            _mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        /// Exports the hierarchy of Warehouse and Truck objects.
        /// </summary>
        /// <response code="200">Successful response</response>
        /// <response code="400">An error occurred loading.</response>
        /// <response code="404">No hierarchy loaded yet.</response>
        [HttpGet]
        [Route("/warehouse")]
        [ValidateModelState]
        [SwaggerOperation("ExportWarehouses")]
        [SwaggerResponse(200, type: typeof(Warehouse), description: "Successful response")]
        [SwaggerResponse(400, type: typeof(Error), description: "An error occurred loading.")]
        public virtual IActionResult ExportWarehouses()
        {
            try
            {
                _logger.LogDebug("ExportWarehouses Request received");
                
                IReadOnlyCollection<BusinessLogic.Entities.Warehouse> warehouseList = _warehouseLogic.GetAll();
            
                if(warehouseList.Count <= 0)
                {
                    _logger.LogWarning($"ExportWarehouses failed no Warehouses found");
                    return new NotFoundObjectResult(new Error
                                                    { ErrorMessage = "No hierarchy loaded yet." });
                }
            
                List<Warehouse> rList = warehouseList.Select(warehouse => _mapper.Map<BusinessLogic.Entities.Warehouse, Warehouse>(warehouse)).ToList();

                return new ObjectResult(rList) { StatusCode = 200 };
            }
            catch (BusinessLayerExceptionBase e) when (e.InnerException is BusinessLayerDataNotFoundException)
            {
                _logger.LogError(e,$"TrackParcel failed with {e.Message}");
                return new NotFoundObjectResult(new Error() {ErrorMessage = $"No hierarchy loaded yet"});
            }
            catch (BusinessLayerExceptionBase e) when (e.InnerException is BusinessLayerValidationException)
            {
                _logger.LogError(e,$"TrackParcel failed with {e.Message}");
                return new BadRequestObjectResult(new Error() {ErrorMessage = $"An error occurred loading."});
            }
            catch (BusinessLayerExceptionBase e) when (e.InnerException is DataAccessExceptionBase)
            {
                _logger.LogError(e,$"TrackParcel failed with {e.Message}");
                return new BadRequestObjectResult(new Error
                                                  { ErrorMessage = $"The operation failed due to an error." });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,$"ExportWarehouses failed with {exception.Message}");
                
                return new BadRequestObjectResult(new Error
                                                  { ErrorMessage = "An error occurred loading." });

            }
        }

        /// <summary>
        /// Get a certain warehouse or truck by code
        /// </summary>
        /// <param name="code"></param>
        /// <response code="200">Successful response</response>
        /// <response code="400">An error occurred loading.</response>
        /// <response code="404">Warehouse id not found</response>
        [HttpGet]
        [Route("/warehouse/{code}")]
        [ValidateModelState]
        [SwaggerOperation("GetWarehouse")]
        [SwaggerResponse(200, type: typeof(Warehouse), description: "Successful response")]
        [SwaggerResponse(400, type: typeof(Error), description: "An error occurred loading.")]
        [SwaggerResponse(404, type: typeof(Error), description: "Warehouse not found")]
        public virtual IActionResult GetWarehouse([FromRoute] [Required] string code)
        {
            try
            {
                _logger.LogDebug("GetWarehouse Request received");
                WarehouseCode warehouseCode = new WarehouseCode(code);

                BusinessLogic.Entities.Hop? warehouse = _warehouseLogic.Get(warehouseCode);
            
                if(warehouse is null)
                {
                    _logger.LogWarning("GetWarehouse failed warehouse is null");
                    return new NotFoundObjectResult(new Error
                                                    { ErrorMessage = "Warehouse not found" });
                }
            
                switch(warehouse)
                {
                    case BusinessLogic.Entities.Truck c: 
                        _logger.LogDebug("GetWarehouse Request complete");
                        return new ObjectResult(_mapper.Map<BusinessLogic.Entities.Truck,DTOs.Truck>(c)) { StatusCode = 200 };
                    case BusinessLogic.Entities.Warehouse c: 
                        _logger.LogDebug("GetWarehouse Request complete");
                        return new ObjectResult(_mapper.Map<BusinessLogic.Entities.Warehouse,DTOs.Warehouse>(c)) { StatusCode = 200 };
                    case BusinessLogic.Entities.TransferWarehouse c: 
                        _logger.LogDebug("GetWarehouse Request complete");
                        return new ObjectResult(_mapper.Map<BusinessLogic.Entities.TransferWarehouse,DTOs.Transferwarehouse>(c)) { StatusCode = 200 };
                    default: return new NotFoundObjectResult(new Error
                                                                                 { ErrorMessage = "Warehouse not found" });;
                }
            }
            catch (BusinessLayerExceptionBase e) when (e.InnerException is BusinessLayerDataNotFoundException)
            {
                _logger.LogError(e,$"TrackParcel failed with {e.Message}");
                return new NotFoundObjectResult(new Error() {ErrorMessage = $"Warehouse id not found"});
            }
            catch (BusinessLayerExceptionBase e) when (e.InnerException is BusinessLayerValidationException)
            {
                _logger.LogError(e,$"TrackParcel failed with {e.Message}");
                return new BadRequestObjectResult(new Error() {ErrorMessage = $"An error occurred loading."});
            }
            catch (BusinessLayerExceptionBase e) when (e.InnerException is DataAccessExceptionBase)
            {
                _logger.LogError(e,$"TrackParcel failed with {e.Message}");
                return new BadRequestObjectResult(new Error
                                                  { ErrorMessage = $"An error occurred loading." });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,$"ExportWarehouses failed with {exception.Message}");
                
                return new BadRequestObjectResult(new Error
                                                  { ErrorMessage = "An error occurred loading." });

            }
        }

        /// <summary>
        /// Imports a hierarchy of Warehouse and Truck objects.
        /// </summary>
        /// <param name="warehouse"></param>
        /// <response code="200">Successfully loaded.</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/warehouse")]
        [ValidateModelState]
        [SwaggerOperation("ImportWarehouses")]
        [SwaggerResponse(400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult ImportWarehouses([FromBody] DTOs.Warehouse warehouse)
        {
            try
            {
                _logger.LogDebug("ImportWarehouses Request received");
                
                BusinessLogic.Entities.Warehouse eWarehouse = _mapper.Map<DTOs.Warehouse, BusinessLogic.Entities.Warehouse>(warehouse);

                bool wasAdded = _warehouseLogic.ReplaceHierarchy(eWarehouse);

                if (wasAdded)
                    return new OkResult();

                _logger.LogWarning($"ImportWarehouses failed couldnt add Warehouse");
                return new BadRequestObjectResult(new Error
                                                  { ErrorMessage = "The operation failed due to an error." });

            }
            catch (BusinessLayerExceptionBase e) when (e.InnerException is BusinessLayerValidationException)
            {
                _logger.LogError(e,$"TrackParcel failed with {e.Message}");
                return new BadRequestObjectResult(new Error() {ErrorMessage = $"The operation failed due to an error."});
            }
            catch (BusinessLayerExceptionBase e) when (e.InnerException is DataAccessExceptionBase)
            {
                _logger.LogError(e,$"TrackParcel failed with {e.Message}");
                return new BadRequestObjectResult(new Error
                                                  { ErrorMessage = $"The operation failed due to an error." });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,$"ExportWarehouses failed with {exception.Message}");
                
                return new BadRequestObjectResult(new Error
                                                  { ErrorMessage = "The operation failed due to an error." });

            }
        }
    }
}