using System;
using System.IO;

using FakeItEasy;

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

        [Test]
        public void CanConvert_Truck_True()
        {
            bool result = converter.CanConvert(typeof(Truck));

            result.Should().BeTrue();
        }

        [Test]
        public void CanConvert_Parcel_False()
        {
            bool result = converter.CanConvert(typeof(Parcel));

            result.Should().BeFalse();
        }

        [Test]
        public void WriteJson_Called_ThrowsNotImplemented()
        {
            Action action;

            action = () => converter.WriteJson(null, null, null);

            action.Should().Throw<NotImplementedException>();
        }

        [Test]
        public void ReadJson_ReadsJsonAsTruck_Successful()
        {
            Type truckType = typeof(Truck);
            string jsonString = "{\"level\":null,\"nextHops\":null,\"hopType\":\"Truck\",\"code\":null,\"description\":null,\"processingDelayMins\":null,\"locationName\":null,\"locationCoordinates\":null}";

            object? result = converter.ReadJson(new JsonTextReader(new StringReader(jsonString)),
                truckType,
                null,
                new JsonSerializer());

            result.GetType().Should().Be(truckType);
        }

        [Test]
        public void ReadJson_ReaderIsNull_ThrowsArugmentNullException()
        {
            Type truckType = typeof(Truck);
            Action action;
            string jsonString = "{\"level\":null,\"nextHops\":null,\"hopType\":\"Truck\",\"code\":null,\"description\":null,\"processingDelayMins\":null,\"locationName\":null,\"locationCoordinates\":null}";

            action = () => converter.ReadJson(null,
                         truckType,
                         null,
                         new JsonSerializer());

            action.Should().Throw<ArgumentNullException>().WithParameterName("reader");
        }

        [Test]
        public void ReadJson_SerializerIsNull_ThrowsArugmentNullException()
        {
            Type truckType = typeof(Truck);
            Action action;
            string jsonString = "{\"level\":null,\"nextHops\":null,\"hopType\":\"Truck\",\"code\":null,\"description\":null,\"processingDelayMins\":null,\"locationName\":null,\"locationCoordinates\":null}";

            action = () => converter.ReadJson(new JsonTextReader(new StringReader(jsonString)),
                         truckType,
                         null,
                         null);

            action.Should().Throw<ArgumentNullException>().WithParameterName("serializer");
        }

        [Test]
        public void ReadJson_TokenTypeIsNull_ReturnsNull()
        {
            Type truckType = typeof(Truck);
            JsonReader reader = A.Fake<JsonReader>();
            A.CallTo(() => reader.TokenType).Returns(JsonToken.Null);


            object? result = converter.ReadJson(reader,
                truckType,
                null,
                new JsonSerializer());

            result.Should().BeNull();
        }
    }
}