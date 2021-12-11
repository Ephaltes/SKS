using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NLSL.SKS.Package.Services.DTOs;

using NUnit.Framework;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NLSL.SKS.Package.IntegrationTests
{
    public class StaffApiBehaviour
    {
         private HttpClient _httpClient;
         private HttpListener _listener;
         private int _port;
         private Parcel _testParceL;
         private string baseUrl;
         private string _postbinContainerId ;
         private static string _postbinAdress = "https://postb.in";
         private static string _postbinApiPath = "/api/bin/";
         
         [SetUp]
         public async Task Setup()
         {
             baseUrl = TestContext.Parameters.Get("baseUrl", "https://localhost:5001");
             _httpClient = new HttpClient
                           {
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
             await PostbinSetup();
         }
         private async Task PostbinSetup()
         {
             var x = await _httpClient.PostAsync(_postbinAdress + _postbinApiPath.TrimEnd('/'),null);
             
             JObject obj = JObject.Parse(await x.Content.ReadAsStringAsync());
             _postbinContainerId = (string)obj["binId"];
         }
         [TearDown]
         public async Task teardown()
         {
             await _httpClient.DeleteAsync(_postbinAdress + _postbinApiPath + _postbinContainerId);
         }
         [Test]
         public async Task WebhookShouldBeCallled_Success()
         {
             StringContent content = new StringContent(File.ReadAllText("warehouse_test_data"), Encoding.UTF8, "application/json");
             HttpResponseMessage warehouserequest = await _httpClient.PostAsync("/warehouse", content);
             if (!warehouserequest.IsSuccessStatusCode)
             {
                 Assert.Fail();
             }
 
             content = new StringContent(JsonConvert.SerializeObject(_testParceL), Encoding.UTF8, "application/json");
             HttpResponseMessage resultSubmit = await _httpClient.PostAsync("/parcel", content);
             if (!resultSubmit.IsSuccessStatusCode)
             {
                 Assert.Fail();
             }
 
             JObject obj = JObject.Parse(await resultSubmit.Content.ReadAsStringAsync());
             string trackingID = (string)obj["trackingId"];
 
             HttpResponseMessage resultaAddWebhook = await _httpClient.PostAsync("/parcel/" + trackingID + "/webhooks?url="+ _postbinAdress +"/"+ _postbinContainerId, null);
 
             if (!resultaAddWebhook.IsSuccessStatusCode)
             {
                 Assert.Fail();
             }
 
             WebhookMessage parsedResponse = JsonConvert.DeserializeObject<WebhookMessage>(await resultaAddWebhook.Content.ReadAsStringAsync());
 
             HttpResponseMessage webhooks = await _httpClient.GetAsync("/parcel/" + trackingID + "/webhooks");
 
             string result = await webhooks.Content.ReadAsStringAsync();
             IList<WebhookResponse> listOfWebhooks = JsonConvert.DeserializeObject<IList<WebhookResponse>>(result);
 
             listOfWebhooks.Count.Should().Be(1);
             listOfWebhooks[0].Url.Should().Be(_postbinAdress +"/"+ _postbinContainerId);
             listOfWebhooks[0].TrackingId.Should().Be(trackingID);
             
             
             HttpResponseMessage hops = await _httpClient.GetAsync("/parcel/" + trackingID);
             if (!hops.IsSuccessStatusCode)
             {
                 Assert.Fail();
             }
 
             TrackingInformation parsedHops = JsonConvert.DeserializeObject<TrackingInformation>(await hops.Content.ReadAsStringAsync());
 
             await _httpClient.PostAsync("/parcel/" + trackingID + "/reportHop/" + parsedHops.FutureHops[0].Code, null);

             HttpResponseMessage mirrorRequest = await _httpClient.GetAsync(_postbinAdress+_postbinApiPath+_postbinContainerId+"/req/shift");
             if(!mirrorRequest.IsSuccessStatusCode)
             { 
                 Assert.Fail();
             }

             JObject objMirrorRequest = JObject.Parse(await mirrorRequest.Content.ReadAsStringAsync());
             var postbinResponse = objMirrorRequest["body"].ToString();

             JObject objMirrorRequestBody = JObject.Parse(postbinResponse);
             var trackingIdFromWebhook = objMirrorRequestBody["trackingId"].ToString();

             trackingIdFromWebhook.Should().Be(trackingID);
         }
    }
}