using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Newtonsoft.Json;

using NLSL.SKS.Package.Services.DTOs;

using NUnit.Framework;

namespace NLSL.SKS.Package.IntegrationTests
{
    public class ParcelApiBehaviour
    {
        private Recipient _austriaRecipient;
        private Recipient _germanyRecipient;
        private HttpClient _httpClient;

        private string baseUrl;

        [SetUp]
        public async Task Setup()
        {
            baseUrl = "https://localhost:5001/parcel";
            _httpClient = new HttpClient
                          {
                              BaseAddress = new Uri(baseUrl),
                          };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            Warehouse _warehouse = JsonConvert.DeserializeObject<Warehouse>(File.ReadAllText("warehouse_test_data"));
            var content = new StringContent(JsonConvert.SerializeObject(_warehouse), Encoding.UTF8, "application/json");
            var request = await _httpClient.PostAsync($"https://localhost:5001/warehouse", content);


            _germanyRecipient = new Recipient
                                {
                                    City = "Berlin",
                                    Country = "Germany",
                                    Name = "Max Mustermann",
                                    Street = "Berliner Str. 44",
                                    PostalCode = "10713"
                                };

            _austriaRecipient = new Recipient
                                {
                                    City = "Wien",
                                    Country = "Österreich",
                                    Name = "Maxi Musti",
                                    Street = "Wienerbergstraße 20",
                                    PostalCode = "1120"
                                };
        }

        [Test]
        public async Task SubmitParcelToGermany_success()
        {
            Parcel parcel = new Parcel
                            {
                                Weight = 1,
                                Recipient = _germanyRecipient,
                                Sender = _austriaRecipient
                            };

            HttpResponseMessage httpResult = await _httpClient.PostAsJsonAsync("", parcel);

            NewParcelInfo newParcelInfo = await httpResult.Content.ReadFromJsonAsync<NewParcelInfo>();

            newParcelInfo.Should().NotBeNull();
            newParcelInfo.TrackingId.Should().NotBeEmpty();
        }

        [Test]
        public async Task SubmitParcelToGermany_AsPartner_success()
        {
            Parcel parcel = new Parcel
                            {
                                Weight = 1,
                                Recipient = _germanyRecipient,
                                Sender = _austriaRecipient
                            };

            Random _random = new Random();
            string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int Length = 9;

            var trackingId = new string(Enumerable.Repeat(Chars, Length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

            HttpResponseMessage httpResult = await _httpClient.PostAsJsonAsync( $"{_httpClient.BaseAddress}/{trackingId}", parcel);

            NewParcelInfo newParcelInfo = await httpResult.Content.ReadFromJsonAsync<NewParcelInfo>();

            newParcelInfo.Should().NotBeNull();
            newParcelInfo.TrackingId.Should().Be(trackingId);
        }
        
        [Test]
        public async Task SubmitParcel_TrackParcel_success()
        {
            Parcel parcel = new Parcel
                            {
                                Weight = 1,
                                Recipient = _germanyRecipient,
                                Sender = _austriaRecipient
                            };

            
            HttpResponseMessage httpResult = await _httpClient.PostAsJsonAsync("", parcel);

            NewParcelInfo newParcelInfo = await httpResult.Content.ReadFromJsonAsync<NewParcelInfo>();

            httpResult = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{newParcelInfo.TrackingId}");

            string jsonString = await httpResult.Content.ReadAsStringAsync();
            TrackingInformation trackingInformation = JsonConvert.DeserializeObject<TrackingInformation>(jsonString);
            
            trackingInformation.Should().NotBeNull();
            trackingInformation.FutureHops.Count.Should().Be(3);
        }
        
        [Test]
        public async Task ReportHop_success()
        {
            Parcel parcel = new Parcel
                            {
                                Weight = 1,
                                Recipient = _germanyRecipient,
                                Sender = _austriaRecipient
                            };
            

            HttpResponseMessage httpResult = await _httpClient.PostAsJsonAsync("", parcel);

            NewParcelInfo newParcelInfo = await httpResult.Content.ReadFromJsonAsync<NewParcelInfo>();
            
            httpResult = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{newParcelInfo.TrackingId}");

            string jsonString = await httpResult.Content.ReadAsStringAsync();
            TrackingInformation trackingInformation = JsonConvert.DeserializeObject<TrackingInformation>(jsonString);
            
            await _httpClient.PostAsync($"{_httpClient.BaseAddress}/{newParcelInfo.TrackingId}/reportHop/{trackingInformation.FutureHops.First().Code}"
                , null);
            
            httpResult = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{newParcelInfo.TrackingId}");

            jsonString = await httpResult.Content.ReadAsStringAsync();
            trackingInformation = JsonConvert.DeserializeObject<TrackingInformation>(jsonString);

            
            trackingInformation.Should().NotBeNull();
            trackingInformation.FutureHops.Count.Should().Be(2);
        }
        
        [Test]
        public async Task Parcel_delivered_success()
        {
            Parcel parcel = new Parcel
                            {
                                Weight = 1,
                                Recipient = _germanyRecipient,
                                Sender = _austriaRecipient
                            };
            

            HttpResponseMessage httpResult = await _httpClient.PostAsJsonAsync("", parcel);

            NewParcelInfo newParcelInfo = await httpResult.Content.ReadFromJsonAsync<NewParcelInfo>();
            
            httpResult = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{newParcelInfo.TrackingId}");

            string jsonString = await httpResult.Content.ReadAsStringAsync();
            TrackingInformation trackingInformation = JsonConvert.DeserializeObject<TrackingInformation>(jsonString);

            foreach (HopArrival nextHop in trackingInformation.FutureHops)
            {
                await _httpClient.PostAsync($"{_httpClient.BaseAddress}/{newParcelInfo.TrackingId}/reportHop/{nextHop.Code}"
                    , null);
            }
            
            httpResult = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/{newParcelInfo.TrackingId}/reportDelivery",null);

            httpResult.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}