using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using AutoMapper;

using FakeItEasy;

using FizzWare.NBuilder;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.BusinessLogic.CustomExceptions;
using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;
using NLSL.SKS.Package.ServiceAgents.Entities;
using NLSL.SKS.Package.ServiceAgents.Exceptions;
using NLSL.SKS.Package.ServiceAgents.Interface;
using NLSL.SKS.Package.WebhookManager.Interfaces;

using NUnit.Framework;

using GeoCoordinate = NLSL.SKS.Package.DataAccess.Entities.GeoCoordinate;
using HopArrival = NLSL.SKS.Package.DataAccess.Entities.HopArrival;
using Parcel = NLSL.SKS.Package.BusinessLogic.Entities.Parcel;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class ParcelLogicBehaviour
    {
        private IGeoCodingAgent _geoCodingAgent;
        private ILogger<ParcelLogic> _logger;
        private IMapper _mapper;
        private ParcelLogic _parcelLogic;
        private IParcelRepository _parcelRepository;
        private IValidator<Parcel> _parcelValidator;
        private IValidator<ReportHop> _reportHopValidator;
        private IValidator<TrackingId> _trackingIdValidator;
        private IWarehouseRepository _warehouseRepository;
        private IHttpAgent _httpClient;
        private IWebHookManager _webHookManager;
        [SetUp]
        public void Setup()
        {
            _parcelValidator = A.Fake<IValidator<Parcel>>();
            _trackingIdValidator = A.Fake<IValidator<TrackingId>>();
            _reportHopValidator = A.Fake<IValidator<ReportHop>>();
            _warehouseRepository = A.Fake<IWarehouseRepository>();
            _geoCodingAgent = A.Fake<IGeoCodingAgent>();
            _httpClient = A.Fake<IHttpAgent>();
            _webHookManager = A.Fake<WebhookManager.WebhookManager>();
            _parcelRepository = A.Fake<IParcelRepository>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<ParcelLogic>>();
            _parcelLogic = new ParcelLogic(_parcelValidator,
                _trackingIdValidator,
                _reportHopValidator,
                _parcelRepository,
                _mapper,
                _logger,
                _geoCodingAgent,
                _warehouseRepository,
                _httpClient,
                _webHookManager
                );
        }

        [Test]
        public void Transition_ValidParcel_ReturnsParcel()
        {
            IReadOnlyCollection<GeoCoordinates> geoCoordinateList = Builder<GeoCoordinates>.CreateListOfSize(2).Build().ToList();

            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _geoCodingAgent.GetGeoCoordinates(null)).WithAnyArguments().Returns(geoCoordinateList);
            A.CallTo(() => _warehouseRepository.GetHopForPoint(null)).WithAnyArguments()
                .Returns(new DataAccess.Entities.Truck());
            A.CallTo(() => _warehouseRepository.GetParentOfHopByCode(null)).WithAnyArguments()
                .Returns(new DataAccess.Entities.Warehouse(){Code = "123123123"});

            Parcel? result = _parcelLogic.Submit(new Parcel());

            result.Should().NotBeNull();
        }

        [Test]
        public void Transition_InvalidParcel_ArgumentException()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);
            Action act;

            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            act = () => _parcelLogic.Submit(null);

            act.Should().Throw<BusinessLayerExceptionBase>();
        }

        [Test]
        public void Track_ValidTrackingId_ReturnsParcel()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _parcelRepository.GetParcelByTrackingId(null)).WithAnyArguments().Returns(new DataAccess.Entities.Parcel());
            A.CallTo(_mapper).Where(call => call.Method.Name == "Map").WithNonVoidReturnType().Returns(new Parcel());


            Parcel? result = _parcelLogic.Track(new TrackingId("ABCABC123"));

            result.Should().NotBeNull();
        }

        [Test]
        public void Track_InvalidTrackingId_ArgumentException()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);
            Action act;
            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            act = () => _parcelLogic.Track(null);

            act.Should().Throw<BusinessLayerExceptionBase>();
        }

        [Test]
        public void Submit_ValidParcel_ReturnsParcel()
        {
            IReadOnlyCollection<GeoCoordinates> geoCoordinateList = Builder<GeoCoordinates>.CreateListOfSize(2).Build().ToList();

            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _geoCodingAgent.GetGeoCoordinates(null)).WithAnyArguments().Returns(geoCoordinateList);
            A.CallTo(() => _warehouseRepository.GetHopForPoint(null)).WithAnyArguments()
                .Returns(new DataAccess.Entities.Truck());
            A.CallTo(() => _warehouseRepository.GetParentOfHopByCode(null)).WithAnyArguments()
                .Returns(new DataAccess.Entities.Warehouse(){Code = "123123123"});
            
            Parcel? result = _parcelLogic.Submit(new Parcel());

            result.Should().NotBeNull();
        }

        [Test]
        public void Submit_InvalidParcel_Arguemntexception()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);
            Action act;
            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            act = () => _parcelLogic.Submit(null);

            act.Should().Throw<BusinessLayerExceptionBase>();
        }

        [Test]
        public void Delivered_ValidTrackingId_ReturnsTrue()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _parcelRepository.GetParcelByTrackingId(null)).WithAnyArguments().Returns(new DataAccess.Entities.Parcel
                                                                                                     { FutureHops = new() });


            bool? result = _parcelLogic.Delivered(new TrackingId("ABCABC123"));

            result.Should().BeTrue();
        }

        [Test]
        public void Delivered_InvalidTrackingId_ArgumentException()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);

            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            Action act = () => _parcelLogic.Delivered(null);

            act.Should().Throw<BusinessLayerExceptionBase>();
        }

        [Test]
        public void ReportHop_ValidHop_ReturnsTrue()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _reportHopValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _parcelRepository.GetParcelByTrackingId(null)).WithAnyArguments().Returns(new DataAccess.Entities.Parcel
                                                                                                     {
                                                                                                         FutureHops = new()
                                                                                                                      {
                                                                                                                          new HopArrival
                                                                                                                          {
                                                                                                                              Code = "Warehouse123",
                                                                                                                              Hop = new DataAccess.Entities.Warehouse()
                                                                                                                          }
                                                                                                                      },
                                                                                                         VisitedHops = new(),
                                                                                                         TrackingId = "ABCABC123"
                                                                                                     });

            bool result = _parcelLogic.ReportHop(new ReportHop
                                                 { TrackingId = new TrackingId("ABCABC123"), HopCode = "Warehouse123", });

            result.Should().BeTrue();
        }

        [Test]
        public void ReportHop_InvalidHop_ArgumentException()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);
            Action act;
            A.CallTo(() => _reportHopValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            act = () => _parcelLogic.ReportHop(null);

            act.Should().Throw<BusinessLayerExceptionBase>();
        }

        [Test]
        public void Track_Throws_BusinessLayerDataNotfoundException()
        {
            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments()
                .Throws<BusinessLayerDataNotFoundException>();

            Action act = () => _parcelLogic.Track(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<BusinessLayerDataNotFoundException>();
        }

        [Test]
        public void Track_Throws_DataAccessException()
        {
            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments()
                .Throws<DataAccessExceptionBase>();

            Action act = () => _parcelLogic.Track(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<DataAccessExceptionBase>();
        }

        [Test]
        public void Track_Throws_ServiceAgentException()
        {
            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments()
                .Throws<ServiceAgentsExceptionBase>();

            Action act = () => _parcelLogic.Track(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<ServiceAgentsExceptionBase>();
        }

        [Test]
        public void Submit_Throws_BusinessLayerDataNotfoundException()
        {
            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments()
                .Throws<BusinessLayerDataNotFoundException>();

            Action act = () => _parcelLogic.Submit(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<BusinessLayerDataNotFoundException>();
        }

        [Test]
        public void Submit_Throws_DataAccessException()
        {
            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments()
                .Throws<DataAccessExceptionBase>();

            Action act = () => _parcelLogic.Submit(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<DataAccessExceptionBase>();
        }

        [Test]
        public void Submit_Throws_ServiceAgentException()
        {
            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments()
                .Throws<ServiceAgentsExceptionBase>();

            Action act = () => _parcelLogic.Submit(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<ServiceAgentsExceptionBase>();
        }


        [Test]
        public void Delivered_Throws_BusinessLayerDataNotfoundException()
        {
            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments()
                .Throws<BusinessLayerDataNotFoundException>();

            Action act = () => _parcelLogic.Delivered(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<BusinessLayerDataNotFoundException>();
        }

        [Test]
        public void Delivered_Throws_DataAccessException()
        {
            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments()
                .Throws<DataAccessExceptionBase>();

            Action act = () => _parcelLogic.Delivered(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<DataAccessExceptionBase>();
        }

        [Test]
        public void Delivered_Throws_ServiceAgentException()
        {
            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments()
                .Throws<ServiceAgentsExceptionBase>();

            Action act = () => _parcelLogic.Delivered(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<ServiceAgentsExceptionBase>();
        }

        [Test]
        public void ReportHop_Throws_BusinessLayerDataNotfoundException()
        {
            A.CallTo(() => _reportHopValidator.Validate(null)).WithAnyArguments()
                .Throws<BusinessLayerDataNotFoundException>();

            Action act = () => _parcelLogic.ReportHop(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<BusinessLayerDataNotFoundException>();
        }

        [Test]
        public void ReportHop_Throws_DataAccessException()
        {
            A.CallTo(() => _reportHopValidator.Validate(null)).WithAnyArguments()
                .Throws<DataAccessExceptionBase>();

            Action act = () => _parcelLogic.ReportHop(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<DataAccessExceptionBase>();
        }

        [Test]
        public void ReportHop_Throws_ServiceAgentException()
        {
            A.CallTo(() => _reportHopValidator.Validate(null)).WithAnyArguments()
                .Throws<ServiceAgentsExceptionBase>();

            Action act = () => _parcelLogic.ReportHop(null);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<ServiceAgentsExceptionBase>();
        }
    }
}