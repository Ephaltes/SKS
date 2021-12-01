using System;

using AutoMapper;

using FakeItEasy;

using FizzWare.NBuilder.Extensions;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.BusinessLogic.CustomExceptions;
using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;
using NLSL.SKS.Package.WebhookManager.Interfaces;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class WebHookLogicBehaviour
    {
        private ILogger<WebHookLogic> _logger;
        private IMapper _mapper;
        private IValidator<TrackingId> _trackingIdValidator;
        private IWebHookLogic _webHookLogic;
        private IWebHookManager _webHookManger;
        private IValidator<WebHook> _webHookValidator;

        [SetUp]
        public void Setup()
        {
            _webHookValidator = A.Fake<IValidator<WebHook>>();
            _trackingIdValidator = A.Fake<IValidator<TrackingId>>();
            _logger = A.Fake<ILogger<WebHookLogic>>();
            _mapper = A.Fake<IMapper>();
            _webHookManger = A.Fake<IWebHookManager>();
            _webHookLogic = new WebHookLogic(_webHookValidator, _logger, _mapper, _webHookManger, _trackingIdValidator);
        }
        
        [Test]
        public void Add_ValidWebHook_ReturnsWebHookResponse()
        {
            DateTime example = DateTime.Now;
            WebHook test = new WebHook();
            test.trackingId = "ABCDEFGHI";
            test.CreatedAt = example;
            test.URL = "test.com";
            var test2 = new WebhookManager.Entities.WebHook();
            test2.trackingId = "ABCDEFGHI";
            test2.CreatedAt = example;
            test2.URL = "test.com";
            var exampleResponse = new WebhookManager.Entities.WebhookResponse();
            exampleResponse.Id = 1;
            exampleResponse.TrackingId = "ABCDEFGHI";
            exampleResponse.Url = "test.com";
            ValidationResult validationResult = new ValidationResult();
            var exampleResponse2 = new BusinessLogic.Entities.WebhookResponse();
            exampleResponse2.Id = 1;
            exampleResponse2.TrackingId = "ABCDEFGHI";
            exampleResponse2.Url = "test.com";


            A.CallTo(() => _webHookValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _webHookManger.SubscribeNewWebHook(null)).WithAnyArguments().Returns(exampleResponse);
            A.CallTo(_mapper).Where(call => call.Method.Name == "Map").WithReturnType<WebhookManager.Entities.WebHook>().Returns(test2);
            A.CallTo(_mapper).Where(call => call.Method.Name == "Map").WithReturnType<Entities.WebhookResponse>().Returns(exampleResponse2);
            
            var result = _webHookLogic.Add(test);

            result.Id.Should().Be(1);
            result.TrackingId.Should().Be("ABCDEFGHI");
            result.Url.Should().Be("test.com");
        }
        [Test]
        public void Add_InvalidWebHook_ReturnsWebHookResponse()
        {
            DateTime example = DateTime.Now;
            WebHook test = new WebHook();
            test.trackingId = "ABCDEFGH";
            test.CreatedAt = example;
            test.URL = "test.com";
            
            A.CallTo(() => _webHookValidator.Validate(null)).WithAnyArguments()
                .Throws<BusinessLayerValidationException>();

            Action act = () => _webHookLogic.Add(test);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<BusinessLayerValidationException>();
        }
        [Test]
        public void Remove_valid_musthavehappenedonce()
        {
            _webHookLogic.Remove(1L);
            
            A.CallTo(() => _webHookManger.UnSubscribeWebHook(null)).WithAnyArguments().MustHaveHappened();
        }
        [Test]
        public void GetByTrackingId_InvalidID_BusinessLayerValidationException()
        {
            var test = new TrackingId("ABCDEFGH");
            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments()
                .Throws<BusinessLayerValidationException>();

            Action act = () => _webHookLogic.GetByTrackingId(test);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<BusinessLayerValidationException>();
        }
        [Test]
        public void GetByTrackingId_NullResultSet_BusinessLayerDataNotFoundException()
        {
            var test = new TrackingId("ABCDEFGHI");
            ValidationResult validationResult = new ValidationResult();
            A.CallTo(() => _webHookManger.GetAllByTrackingId(null)).WithAnyArguments().Returns(null);
            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            
            
            Action act = () => _webHookLogic.GetByTrackingId(test);

            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<BusinessLayerDataNotFoundException>();
        }
    }
}