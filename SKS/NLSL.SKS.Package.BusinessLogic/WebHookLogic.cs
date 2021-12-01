using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.BusinessLogic.CustomExceptions;
using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;
using NLSL.SKS.Package.ServiceAgents.Exceptions;
using NLSL.SKS.Package.WebhookManager.Exceptions;
using NLSL.SKS.Package.WebhookManager.Interfaces;

using WebhookResponses = NLSL.SKS.Package.WebhookManager.Entities.WebhookResponses;


namespace NLSL.SKS.Package.BusinessLogic
{
    public class WebHookLogic : IWebHookLogic
    {
      
        private readonly IValidator<WebHook> _webHookValidator;
        private readonly IValidator<TrackingId> _trackingIdValidator;
        private readonly ILogger<WebHookLogic> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHookManager _webHookManger;
        public WebHookLogic(IValidator<WebHook> webHookValidator, ILogger<WebHookLogic> logger, IMapper mapper, IWebHookManager webHookManger, IValidator<TrackingId> trackingIdValidator)
        {
            _webHookValidator = webHookValidator;
            _logger = logger;
            _mapper = mapper;
            _webHookManger = webHookManger;
            _trackingIdValidator = trackingIdValidator;
        }

        public WebhookResponse? Add(WebHook webHook)
        {
            try
            {
                _logger.LogDebug("starting add a webhook");

                _logger.LogDebug("validating webhook");
                ValidationResult result = _webHookValidator.Validate(webHook);
                if (!result.IsValid)
                {
                    _logger.LogWarning("validation error for webhook");

                    throw new BusinessLayerValidationException(result.Errors.First().ErrorMessage);
                }
                
                var mappedWebHook = _mapper.Map<WebHook, WebhookManager.Entities.WebHook>(webHook);
                mappedWebHook.Id = null;
                var response = _webHookManger.SubscribeNewWebHook(mappedWebHook);
                
                WebhookResponse? mappedResponse = _mapper.Map<WebhookManager.Entities.WebhookResponse, WebhookResponse>(response);

                _logger.LogDebug("started add a webhook complete");

                return mappedResponse;
            }
            catch (BusinessLayerValidationException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (WebHookManagerExceptionBase e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in WebhookManager", e);
            }
        }
        
        
        public void Remove(long? id)
        {
            try
            {
                _logger.LogDebug("starting remove a webhook");
                _webHookManger.UnSubscribeWebHook(id);
                _logger.LogDebug("finished remove a webhook");
            }
            catch (WebHookManagerExceptionBase e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in WebhookManager", e);
            }
        }

        public Entities.WebhookResponses GetByTrackingId(TrackingId id)
        {
            try
            {
                _logger.LogDebug("stated getting by tracking id");
                _logger.LogDebug("validating trackingID");
                ValidationResult result = _trackingIdValidator.Validate(id);
                if (!result.IsValid)
                {
                    _logger.LogWarning("validation error for parcel");

                    throw new BusinessLayerValidationException(result.Errors.First().ErrorMessage);
                }

                var returnVal = _webHookManger.GetAllByTrackingId(id.Id);
                if (returnVal is null)
                {
                    _logger.LogWarning("no data");
                    throw new BusinessLayerDataNotFoundException("no data found for trackingID");
                }
                _logger.LogDebug("finished getting by tracking id");
                return _mapper.Map<WebhookManager.Entities.WebhookResponses, Entities.WebhookResponses>(returnVal);
            }
            catch (WebHookManagerExceptionBase e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in WebhookManager", e);
            }
            catch (BusinessLayerValidationException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (BusinessLayerDataNotFoundException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error no data found", e); 
            }
        }
    }
}