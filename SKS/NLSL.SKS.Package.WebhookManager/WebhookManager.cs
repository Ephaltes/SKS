using System;
using System.Collections.Generic;

using AutoMapper;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;
using NLSL.SKS.Package.ServiceAgents.Exceptions;
using NLSL.SKS.Package.ServiceAgents.Interface;
using NLSL.SKS.Package.WebhookManager.Entities;
using NLSL.SKS.Package.WebhookManager.Entities.Enums;
using NLSL.SKS.Package.WebhookManager.Exceptions;
using NLSL.SKS.Package.WebhookManager.Interfaces;

namespace NLSL.SKS.Package.WebhookManager
{
    public class WebhookManager : IWebHookManager
    {
        private readonly IHttpAgent _httpAgent;
        private readonly ILogger<WebhookManager> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHookRepository _webHookRepository;

        public WebhookManager(IWebHookRepository webHookRepository, IHttpAgent httpAgent, ILogger<WebhookManager> logger, IMapper mapper)
        {
            _webHookRepository = webHookRepository;
            _httpAgent = httpAgent;
            _logger = logger;
            _mapper = mapper;
        }

        public WebhookResponse SubscribeNewWebHook(WebHook webHook)
        {
            try
            {
                _logger.LogDebug("started adding a new subscription");
                DataAccess.Entities.WebHook mapped = _mapper.Map<WebHook, DataAccess.Entities.WebHook>(webHook);
                var id =_webHookRepository.Create(mapped);
                webHook.Id = id;
                WebhookResponse response = _mapper.Map<WebHook, WebhookResponse>(webHook);

                _logger.LogDebug("finished adding a new subscription");
                return response;
            }
            catch (DataAccessExceptionBase e)
            {
                _logger.LogDebug("error in adding a new subscription");
                throw new WebHookManagerExceptionBase("failure in database", e);
            }
        }

        public void UnSubscribeWebHook(long? id)
        {
            try
            {
                _logger.LogDebug("started deleting a  subscription");
                _webHookRepository.Delete(id);
                _logger.LogDebug("finished deleting a new subscription");
            }
            catch (DataAccessExceptionBase e)
            {
                _logger.LogDebug("error in deleting a subscription");
                throw new WebHookManagerExceptionBase("error in deleting a parcel", e);
            }
        }

        public WebhookResponses? GetAllByTrackingId(string trackingId)
        {
            try
            {
                _logger.LogDebug("started getting all parcels");
                IList<DataAccess.Entities.WebHook>? result = _webHookRepository.GetAllWebHooksByTrackingId(trackingId);
                var mapped = _mapper.Map<IList<DataAccess.Entities.WebHook>, WebhookResponses>(result);
                _logger.LogDebug("finished getting all parcels");
                return mapped;
            }
            catch (DataAccessExceptionBase e)
            {
                _logger.LogDebug("started getting all parcels");
                throw new WebHookManagerExceptionBase("failure in database", e);
            }
        }


        public void ParcelStateChanged(Parcel parcel)
        {
            try
            {
                _logger.LogDebug("started parcel changed");
                IList<DataAccess.Entities.WebHook> x = _webHookRepository.GetAllWebHooksByTrackingId(parcel.TrackingId);

                foreach (DataAccess.Entities.WebHook hook in x)
                {
                    WebhookMessage webHookMessage = _mapper.Map<Parcel, WebhookMessage>(parcel);
                    _httpAgent.PostAsJson(hook.URL, webHookMessage);
                    if (parcel.State == StateEnum.Delivered)
                    {
                        UnSubscribeWebHook(hook.Id);
                    }
                }
                _logger.LogDebug("finished parcel changed");
            }
            catch (DataAccessExceptionBase e)
            {
                _logger.LogDebug("failure in parcel changed");
                throw new WebHookManagerExceptionBase("failure in database", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
                _logger.LogDebug("failure in parcel changed");
                throw new WebHookManagerExceptionBase("failure in http agent", e);
            }
        }
    }
}