using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using FluentAssertions;

using NLSL.SKS.Package.ServiceAgents.Entities;

using NUnit.Framework;

namespace NLSL.SKS.Package.ServiceAgents.Test
{
    public class GeoCodingAgentBehaviour
    {
        private GeoCodingAgent _agent;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
                                                                 {
                                                                     cfg.AddProfile<MapperProfile>();
                                                                 });

            _mapper = new Mapper(config);

            _agent = new GeoCodingAgent(_mapper);
        }

        [Test]
        public void GetGeoCoordinates_Success()
        {
            Address request = new Address
                              {
                                  Street = "Höchstädtplatz 6",
                                  ZipCode = "1200",
                                  Country = "Austria"
                              };

            List<GeoCoordinates>? results = _agent.GetGeoCoordinates(request);
            GeoCoordinates result = results.FirstOrDefault();

            result.Should().NotBeNull();
            result.Address.Should().Be("FH Technikum Wien, 6, Höchstädtplatz, KG Brigittenau, Brigittenau, Wien, 1200, Österreich");
            result.Latitude.Should().Be(48.239166400000002);
            result.Longitude.Should().Be(16.3774409);
        }
        
        [Test]
        public void GetGeoCoordinates_Failed_NotFound()
        {
            Address request = new Address
                              {
                                  Street = "asdadawdawd 21d 12d1 2d1212dadwadwa",
                              };

            List<GeoCoordinates>? results = _agent.GetGeoCoordinates(request);
            GeoCoordinates result = results.FirstOrDefault();

            result.Should().BeNull();
        }
    }
}