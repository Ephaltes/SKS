using System;

using FluentAssertions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NLSL.SKS.Package.Services.DTOs;
using NLSL.SKS.Package.Services.DTOs.JsonConverter;

using NUnit.Framework;

namespace NLSL.SKS.Package.DTOs.Tests
{
    public class HopJsonConverterBehaviour
    {
        private HopJsonConverter converter;
        [SetUp]
        public void Setup()
        {
            converter = new HopJsonConverter();
        }

        [Test]
        public void Create_TruckHop_Successful()
        {
            Type type = null;
            Hop hop = new Truck
                      { HopType = "Truck" };

            JObject? jObject = JObject.Parse(JsonConvert.SerializeObject(hop));

            Hop? result = converter.Create(type, jObject);

            result.GetType().Should().Be(typeof(Truck));
        }

        [Test]
        public void Create_TransferWarehouse_Successful()
        {
            Type type = null;
            Hop hop = new Transferwarehouse
                      { HopType = "Transferwarehouse" };

            JObject? jObject = JObject.Parse(JsonConvert.SerializeObject(hop));

            Hop? result = converter.Create(type, jObject);

            result.GetType().Should().Be(typeof(Transferwarehouse));
        }

        [Test]
        public void Create_Warehouse_Successful()
        {
            Type type = null;
            Hop hop = new Warehouse
                      { HopType = "Warehouse" };

            JObject? jObject = JObject.Parse(JsonConvert.SerializeObject(hop));

            Hop? result = converter.Create(type, jObject);

            result.GetType().Should().Be(typeof(Warehouse));
        }

        [Test]
        public void Create_WrongHopType_ThrowsNotImplementedException()
        {
            Type type = null;
            Hop hop = new Warehouse
                      { HopType = "" };
            Action action;

            JObject? jObject = JObject.Parse(JsonConvert.SerializeObject(hop));

            action = () => converter.Create(type, jObject);

            action.Should().Throw<NotImplementedException>();
        }

        [Test]
        public void Create_NoJObject_ThrowsArgumentException()
        {
            Type type = null;
            Action action;

            action = () => converter.Create(type, null);

            action.Should().Throw<ArgumentNullException>().WithParameterName("jObject");
        }

        [Test]
        public void Create_NoHopType_ThrowsArgumentException()
        {
            Type type = null;
            Action action;

            string jsonString = "{\"level\":null,\"nextHops\":null,\"code\":null,\"description\":null,\"processingDelayMins\":null,\"locationName\":null,\"locationCoordinates\":null}";

            JObject jObject = JObject.Parse(jsonString);
            
            action = () => converter.Create(type, jObject);

            action.Should().Throw<ArgumentNullException>().WithParameterName("hopType");
        }
    }
}