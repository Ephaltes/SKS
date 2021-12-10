using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using AutoMapper;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.ServiceAgents.Exceptions;
using NLSL.SKS.Package.ServiceAgents.Interface;

namespace NLSL.SKS.Package.ServiceAgents 
{
    public class HttpAgent : IHttpAgent
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpAgent> _logger;
        private readonly IMapper _mapper;

        public HttpAgent(HttpClient httpClient, IMapper mapper, ILogger<HttpAgent> logger)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _logger = logger;
        }

        public void SendParcelToLogisticPartnerPost(string logisticPartnerUri,Package.DataAccess.Entities.Parcel parcel)
        {
            try
            {
                _logger.LogDebug("starting SendParcelToLogisticPartnerPost");
                string? url = logisticPartnerUri + "/parcel/" + parcel.TrackingId;


                BusinessLogic.Entities.Parcel? mappedToBusinessLogikParcel = _mapper.Map<Parcel, BusinessLogic.Entities.Parcel>(parcel);
                Services.DTOs.Parcel? mappedToControllerParcel = _mapper.Map<BusinessLogic.Entities.Parcel, Services.DTOs.Parcel>(mappedToBusinessLogikParcel);

                string? json = JsonConvert.SerializeObject(mappedToControllerParcel);

                StringContent? data = new StringContent(json, Encoding.UTF8, "application/json");
                _logger.LogDebug(url);
                HttpResponseMessage? httpResult = _httpClient.PostAsJsonAsync(url, mappedToControllerParcel).Result;

                if (!httpResult.IsSuccessStatusCode)
                {
                    _logger.LogDebug("could not call partner api");

                    throw new ServiceAgentHttpRequestFailed(" could not call partner api: " + httpResult.ReasonPhrase + "");
                }

                _logger.LogDebug("ending SendParcelToLogisticPartnerPost");
            }
            catch (ServiceAgentHttpRequestFailed e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new ServiceAgentsExceptionBase("No data found", e);
            }
            catch (AggregateException aggregateException) when (aggregateException.InnerException is HttpRequestException)
            {
                _logger.LogWarning(aggregateException, "Invalid Partner URL");
            }
        }

        public bool PostAsJson(string url,object content)
        {
            try
            {
                HttpResponseMessage? httpResult;
                try
                {
                    httpResult = _httpClient.PostAsJsonAsync(url, content).Result;
                    if (!httpResult.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("webhook bad statuscode "+ httpResult.StatusCode+" with url "+url );
                    }
                    
                }
                catch
                {
                    _logger.LogWarning("could not send webhook exception");
                    return false;
                }
                
                return httpResult.IsSuccessStatusCode;
            }
            catch (ServiceAgentHttpRequestFailed e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new ServiceAgentsExceptionBase("No data found", e);
            }
        }
    }
}