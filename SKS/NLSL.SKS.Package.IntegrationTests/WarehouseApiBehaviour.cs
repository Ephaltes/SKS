using System;
using System.Net.Http;

using NUnit.Framework;

namespace NLSL.SKS.Package.IntegrationTests
{
    public class WarehouseApiBehaviour
    {
         private string baseUrl;
         private HttpClient _httpClient;
         
        [SetUp]
        public void Setup()
        {
            baseUrl = "http://localhost:5000/api/warehouse";
            _httpClient = new HttpClient(){
                BaseAddress = new Uri(baseUrl)
            };
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}