using System;
using System.Net.Http;
using System.Net.Http.Json;
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
                string? url =  logisticPartnerUri + "/parcel/" + parcel.TrackingId;


                BusinessLogic.Entities.Parcel? mappedToBusinessLogikParcel = _mapper.Map<Parcel, BusinessLogic.Entities.Parcel>(parcel);
                Services.DTOs.Parcel? mappedToControllerParcel = _mapper.Map<BusinessLogic.Entities.Parcel, Services.DTOs.Parcel>(mappedToBusinessLogikParcel);

                string? json = JsonConvert.SerializeObject(mappedToControllerParcel);
                
                StringContent? data = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine(url);
                HttpResponseMessage? httpResult = _httpClient.PostAsJsonAsync(url, mappedToControllerParcel).Result;

                if (!httpResult.IsSuccessStatusCode)
                {
                    _logger.LogDebug("could not call partner api");
                    throw new ServiceAgentHttpRequestFailed(" could not call partner api: " + httpResult.ReasonPhrase +"");
                }
                _logger.LogDebug("ending SendParcelToLogisticPartnerPost");
            }
            catch (ServiceAgentHttpRequestFailed e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new ServiceAgentsExceptionBase("No data found", e);
            }
        }

        public bool PostAsJson(string url,object content)
        {
            try
            {
                HttpResponseMessage? httpResult = _httpClient.PostAsJsonAsync(url, content).Result;
                
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