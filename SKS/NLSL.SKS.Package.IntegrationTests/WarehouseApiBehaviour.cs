using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Newtonsoft.Json;

using NLSL.SKS.Package.Services.DTOs;

using NUnit.Framework;

namespace NLSL.SKS.Package.IntegrationTests
{
    public class WarehouseApiBehaviour
    {
         private string baseUrl;
         private HttpClient _httpClient;
         private Warehouse _warehouse;
         
        [SetUp]
        public void Setup()
        {
            baseUrl = baseUrl = TestContext.Parameters.Get("baseUrl", "https://localhost:5001");
            _httpClient = new HttpClient(){
                BaseAddress = new Uri(baseUrl)
            };
            _warehouse = JsonConvert.DeserializeObject<Warehouse>(File.ReadAllText("warehouse_test_data"));
        }

        [Test]
        public async Task ImportWarehouse_success()
        {
            var content = new StringContent(JsonConvert.SerializeObject(_warehouse), Encoding.UTF8, "application/json");
            var request = await _httpClient.PostAsync($"/warehouse", content);
             
            request.IsSuccessStatusCode.Should().BeTrue();
        }
        
        [Test]
        public async Task GetTestWarehouse_success()
        {
            var content = new StringContent(JsonConvert.SerializeObject(_warehouse), Encoding.UTF8, "application/json");
            var request = await _httpClient.PostAsync("/warehouse", content);
            HttpResponseMessage httpResult = await _httpClient.GetAsync($"/warehouse/{"TEST01"}");
            string jsonString = await httpResult.Content.ReadAsStringAsync();
            Warehouse warehouse = JsonConvert.DeserializeObject<Warehouse>(jsonString);

            warehouse.Should().NotBeNull();
            warehouse.Code.Should().Be("TEST01");

        }
        
        [Test]
        public async Task GetAllWarehouse_success()
        {
            var content = new StringContent(JsonConvert.SerializeObject(_warehouse), Encoding.UTF8, "application/json");
            var request = await _httpClient.PostAsync("/warehouse", content);
            HttpResponseMessage httpResult = await _httpClient.GetAsync($"/warehouse");
            string jsonString = await httpResult.Content.ReadAsStringAsync();
            List<Warehouse> warehouse = JsonConvert.DeserializeObject<List<Warehouse>>(jsonString);

            warehouse.Should().NotBeNull();
            warehouse.Count.Should().BeGreaterThan(1);

        }
    }
}