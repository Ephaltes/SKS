namespace NLSL.SKS.Package.IntegrationTests
{
    public class StaffApiBehaviour
    {
        /* private HttpClient _httpClient;
         private HttpListener _listener;
         private int _port;
         private Parcel _testParceL;
         private string baseUrl;
         [SetUp]
         public void Setup()
         {
             baseUrl = TestContext.Parameters.Get("baseUrl", "https://localhost:5001");
             _httpClient = new HttpClient
                           {
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
 
             HttpResponseMessage resultaAddWebhook = await _httpClient.PostAsync("/parcel/" + trackingID + "/webhooks?url=http://localhost:" + _port, null);
 
             if (!resultaAddWebhook.IsSuccessStatusCode)
             {
                 Assert.Fail();
             }
 
             WebhookMessage parsedResponse = JsonConvert.DeserializeObject<WebhookMessage>(await resultaAddWebhook.Content.ReadAsStringAsync());
 
             HttpResponseMessage webhooks = await _httpClient.GetAsync("/parcel/" + trackingID + "/webhooks");
 
             string result = await webhooks.Content.ReadAsStringAsync();
             IList<WebhookResponse> listOfWebhooks = JsonConvert.DeserializeObject<IList<WebhookResponse>>(result);
 
             listOfWebhooks.Count.Should().Be(1);
             listOfWebhooks[0].Url.Should().Be("http://localhost:" + _port);
             listOfWebhooks[0].TrackingId.Should().Be(trackingID);
 
             Thread listeningThread = new Thread(() =>
                                                 {
                                                     _listener.Start();
 
                                                     HttpListenerContext connection = _listener.GetContext();
 
                                                     string text;
                                                     using (StreamReader reader = new StreamReader(connection.Request.InputStream,
                                                                connection.Request.ContentEncoding))
                                                     {
                                                         text = reader.ReadToEnd();
                                                     }
 
                                                     WebhookMessage result = JsonConvert.DeserializeObject<WebhookMessage>(text);
                                                     connection.Response.StatusCode = 200;
                                                     connection.Response.Close();
                                                     result.TrackingId.Should().Be(trackingID);
                                                 });
 
             listeningThread.Start();
 
 
             HttpResponseMessage hops = await _httpClient.GetAsync("/parcel/" + trackingID);
             if (!hops.IsSuccessStatusCode)
             {
                 Assert.Fail();
             }
 
             TrackingInformation parsedHops = JsonConvert.DeserializeObject<TrackingInformation>(await hops.Content.ReadAsStringAsync());
 
             await _httpClient.PostAsync("/parcel/" + trackingID + "/reportHop/" + parsedHops.FutureHops[0].Code, null);
 
             bool success = listeningThread.Join(20000);
             success.Should().BeTrue();
         }*/
    }
}