using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using FakeItEasy;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.ServiceAgents.Entities;
using NLSL.SKS.Package.ServiceAgents.Exceptions;

using NUnit.Framework;

namespace NLSL.SKS.Package.ServiceAgents.Tests
{
    public class GeoCodingAgentBehaviour
    {
        private GeoCodingAgent _agent;
        private IMapper _mapper;
        private ILogger<GeoCodingAgent> _logger;

        [SetUp]
        public void Setup()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
                                                                 {
                                                                     cfg.AddProfile<MapperProfile>();
                                                                 });

            _mapper = new Mapper(config);
            _logger = A.Fake<ILogger<GeoCodingAgent>>();
            
            _agent = new GeoCodingAgent(_mapper,_logger);
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
            Action action;
            Address request = new Address
                              {
                                  Street = "asdadawdawd 21d 12d1 2d1212dadwadwa",
                              };

            action = () => _agent.GetGeoCoordinates(request);

            action.Should().Throw<ServiceAgentsExceptionBase>();
        }
    }
}