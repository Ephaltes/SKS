using System;
using System.ComponentModel.DataAnnotations;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.Services.Attributes;
using NLSL.SKS.Package.WebhookManager.Interfaces;

using Swashbuckle.AspNetCore.Annotations;

using WebhookResponse = NLSL.SKS.Package.Services.DTOs.WebhookResponse;
using WebhookResponses = NLSL.SKS.Package.Services.DTOs.WebhookResponses;

namespace NLSL.SKS.Package.Services.Controllers
{
    [ApiController]
    public class ParcelWebhookApiController
    {
        private readonly IMapper _mapper;

        private readonly IWebHookLogic _webHookLogic;

        private readonly ILogger<ParcelWebhookApiController> _logger;

        public ParcelWebhookApiController(IMapper mapper, ILogger<ParcelWebhookApiController> logger, IWebHookLogic webHookLogic)
        {
            _mapper = mapper;
            _logger = logger;
            _webHookLogic = webHookLogic;
        }

        /// <summary>
        /// Get all registered subscriptions for the parcel webhook.
        /// </summary>
        /// <param name="trackingId"></param>
        /// <response code="200">List of webooks for the &#x60;trackingId&#x60;</response>
        /// <response code="404">No parcel found with that tracking ID.</response>
        [HttpGet]
        [Route("/parcel/{trackingId}/webhooks")]
        [ValidateModelState]
        [SwaggerOperation("ListParcelWebhooks")]
        [SwaggerResponse(statusCode: 200, type: typeof(WebhookResponses), description: "List of webooks for the &#x60;trackingId&#x60;")]
        public virtual IActionResult ListParcelWebhooks([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")]string trackingId)
        {
            try
            {
                var result = _webHookLogic.GetByTrackingId(new TrackingId(trackingId));
                if (result is null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(_mapper.Map<BusinessLogic.Entities.WebhookResponses, DTOs.WebhookResponses>(result));
            }
            catch
            {
                throw;
            }
            /*catch
            {
                return new NotFoundResult();
            }*/
        }

        /// <summary>
        /// Subscribe to a webhook notification for the specific parcel.
        /// </summary>
        /// <param name="trackingId"></param>
        /// <param name="url"></param>
        /// <response code="200">Successful response</response>
        /// <response code="404">No parcel found with that tracking ID.</response>
        [HttpPost]
        [Route("/parcel/{trackingId}/webhooks")]
        [ValidateModelState]
        [SwaggerOperation("SubscribeParcelWebhook")]
        [SwaggerResponse(statusCode: 200, type: typeof(WebhookResponse), description: "Successful response")]
        public virtual IActionResult SubscribeParcelWebhook([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")]string trackingId, [FromQuery][Required()]string url)
        { 
            try
            {
                var businessLogicWebhook = new BusinessLogic.Entities.WebHook();
                businessLogicWebhook.trackingId = trackingId;
                businessLogicWebhook.URL = url;
                businessLogicWebhook.CreatedAt = DateTime.Now;
                
                var result = _webHookLogic.Add(businessLogicWebhook);
                if (result is null)
                {
                    return new NotFoundResult();
                }
                return new OkObjectResult(_mapper.Map<BusinessLogic.Entities.WebhookResponse, DTOs.WebhookResponse>(result));
            }
            catch
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Remove an existing webhook subscription.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Subscription does not exist.</response>
        [HttpDelete]
        [Route("/parcel/webhooks/{id}")]
        [ValidateModelState]
        [SwaggerOperation("UnsubscribeParcelWebhook")]
        public virtual IActionResult UnsubscribeParcelWebhook([FromRoute][Required]long? id)
        { 
            try
            {
                _webHookLogic.Remove(id);
                return new OkResult();
            }
            catch
            {
                return new NotFoundResult();
            }
        }
    }
}