using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NLSL.SKS.Package.Services.DTOs;

using NUnit.Framework;


namespace NLSL.SKS.Package.IntegrationTests
{
    public class StaffApiBehaviour
    {
        private string baseUrl;
        private HttpClient _httpClient;
        private HttpListener _listener;
        private Parcel _testParceL;
        private int _port;
        [SetUp]
        public void Setup()
        {
            baseUrl = TestContext.Parameters.Get("baseUrl", "https://localhost:5001");
            _httpClient = new HttpClient(){
                                              BaseAddress = new Uri(baseUrl)
                                          };
            _listener = new HttpListener();
            _port = 40129;
            _listener.Prefixes.Add($"http://localhost:{_port}/");
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

        [TearDown]
        public void teardown()
        {
            _listener.Stop();
        }
        [Test]
        public async Task WebhookShouldBeCallled_Success()
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

            var resultaAddWebhook = await _httpClient.PostAsync("/parcel/" + trackingID + "/webhooks?url=http://localhost:"+_port, null);

            if (!resultaAddWebhook.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            var parsedResponse = JsonConvert.DeserializeObject<WebhookMessage>(await resultaAddWebhook.Content.ReadAsStringAsync());
            
            var webhooks = await _httpClient.GetAsync("/parcel/" + trackingID + "/webhooks");

            var result = await webhooks.Content.ReadAsStringAsync();
            IList<WebhookResponse> listOfWebhooks = JsonConvert.DeserializeObject<IList<WebhookResponse>>(result);

            listOfWebhooks.Count.Should().Be(1);
            listOfWebhooks[0].Url.Should().Be("http://localhost:"+_port);
            listOfWebhooks[0].TrackingId.Should().Be(trackingID);
            
            Thread listeningThread = new Thread(() =>
                                                {
                                                    _listener.Start();

                                                    var connection = _listener.GetContext();
                                                    
                                                    string text;
                                                    using (var reader = new StreamReader(connection.Request.InputStream,
                                                        connection.Request.ContentEncoding))
                                                    {
                                                        text = reader.ReadToEnd();
                                                    }

                                                    var result = JsonConvert.DeserializeObject<WebhookMessage>(text);
                                                    connection.Response.StatusCode = 200;
                                                    connection.Response.Close();
                                                    result.TrackingId.Should().Be(trackingID);
                                                });
            
            listeningThread.Start();
            
            
            var hops = await _httpClient.GetAsync("/parcel/" + trackingID);
            if (!hops.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            var parsedHops = JsonConvert.DeserializeObject<TrackingInformation>(await hops.Content.ReadAsStringAsync());
            
            await _httpClient.PostAsync("/parcel/" + trackingID + "/reportHop/" + parsedHops.FutureHops[0].Code, null);

            bool success = listeningThread.Join(20000);
            success.Should().BeTrue();
        }
    }
}