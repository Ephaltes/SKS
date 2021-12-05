using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NLSL.SKS.Package.Services.DTOs;

using NUnit.Framework;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NLSL.SKS.Package.Services.Tests
{
    public class ParcelWebHookApiBehaviour
    {
        private string baseUrl;
        private HttpClient _httpClient;
        private HttpListener _listener;
        private Parcel _testParceL;
        [SetUp]
        public void Setup()
        {
            baseUrl = "https://localhost:5001";
            _httpClient = new HttpClient(){
                                              BaseAddress = new Uri(baseUrl)
                                          };
            _testParceL = new Parcel
                          {
                              Weight = 1,
                              Recipient = new Recipient
                                          {
                                              City = "Berlin",
                                              Country = "Germany",
                                              Name = "Max Mustermann",
                                              Street = "Berliner Str. 44",
                                              PostalCode = "10713"
                                          },
                              Sender = new Recipient
                                       {
                                           City = "Wien",
                                           Country = "Österreich",
                                           Name = "Maxi Musti",
                                           Street = "Wienerbergstraße 20",
                                           PostalCode = "1120"
                                       }
                          };
        }

        [Test]
        public async Task AddWebHook_Success()
        {
            var content = new StringContent(File.ReadAllText("warehouse_test_data"), Encoding.UTF8, "application/json");
            var warehouserequest = await _httpClient.PostAsync("/warehouse",content);
            if (!warehouserequest.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            
            var resultSubmit = await _httpClient.PostAsJsonAsync("/parcel", _testParceL);
            if (!resultSubmit.IsSuccessStatusCode)
            {
                
                Assert.Fail();
            }
            JObject obj = JObject.Parse(await resultSubmit.Content.ReadAsStringAsync());
            string trackingID = (string)obj["trackingId"];

            var resultaAddWebhook = await _httpClient.PostAsync("/parcel/" + trackingID + "/webhooks?url=test.com", null);

            if (!resultaAddWebhook.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            var parsedResponse = JsonSerializer.Deserialize<WebhookMessage>(await resultaAddWebhook.Content.ReadAsStringAsync());
            
            var webhooks = await _httpClient.GetAsync("/parcel/" + trackingID + "/webhooks");

            var result = await webhooks.Content.ReadAsStringAsync();
            //throw new Exception(result);
            IList<WebhookResponse> listOfWebhooks = JsonConvert.DeserializeObject<IList<WebhookResponse>>(result);

            listOfWebhooks.Count.Should().Be(1);
            //throw new Exception(JsonSerializer.Serialize(listOfWebhooks[0]));
            listOfWebhooks[0].Url.Should().Be("test.com");
            listOfWebhooks[0].TrackingId.Should().Be(trackingID);
        }
        
        [Test]
        public async Task RemoveWebHook_Success()
        {
            var content = new StringContent(File.ReadAllText("warehouse_test_data"), Encoding.UTF8, "application/json");
            var warehouserequest = await _httpClient.PostAsync("/warehouse",content);
            if (!warehouserequest.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            
            var resultSubmit = await _httpClient.PostAsJsonAsync("/parcel", _testParceL);
            if (!resultSubmit.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            JObject obj = JObject.Parse(await resultSubmit.Content.ReadAsStringAsync());
            string trackingID = (string)obj["trackingId"];

            var resultaAddWebhook = await _httpClient.PostAsync("/parcel/" + trackingID + "/webhooks?url=test.com", null);

            if (!resultaAddWebhook.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            var parsedResponse = JsonConvert.DeserializeObject<WebhookMessage>(await resultaAddWebhook.Content.ReadAsStringAsync());
            
            var webhooks = await _httpClient.GetAsync("/parcel/" + trackingID + "/webhooks");

            var listOfWebhooks = JsonConvert.DeserializeObject<WebhookResponses>(await webhooks.Content.ReadAsStringAsync());

            listOfWebhooks.Count.Should().Be(1);
            listOfWebhooks[0].Url.Should().Be("test.com");
            listOfWebhooks[0].TrackingId.Should().Be(trackingID);
            
            var resultDelete = await _httpClient.DeleteAsync("/parcel/webhooks/" + listOfWebhooks[0].Id);
            
            if (!resultDelete.IsSuccessStatusCode)
            {
                //var result = await resultDelete.Content.ReadAsStringAsync();
                //throw new Exception(result + resultDelete.StatusCode);
                Assert.Fail();
            }
            
            var webhooksDeleted = await _httpClient.GetAsync("/parcel/" + trackingID + "/webhooks");

            var listOfWebhooksDelete = JsonConvert.DeserializeObject<WebhookResponses>(await webhooksDeleted.Content.ReadAsStringAsync());

            listOfWebhooksDelete.Count.Should().Be(0); ;
        }
    }
}