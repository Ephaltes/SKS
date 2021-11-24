using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Castle.Core.Logging;

using FakeItEasy;

using FizzWare.NBuilder;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.BusinessLogic.CustomExceptions;
using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;
using NLSL.SKS.Package.ServiceAgents.Exceptions;

using NUnit.Framework;

using ILogger = Castle.Core.Logging.ILogger;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class WarehouseLogicBehaviour
    {
        private IValidator<WarehouseCode> _warehouseCodeValidator;
        private WarehouseLogic _warehouseLogic;
        private IValidator<Warehouse> _warehouseValidator;
        private IWarehouseRepository _warehouseRepository;
        private IMapper _mapper;
        private ILogger<WarehouseLogic> _logger;
        [SetUp]
        public void Setup()
        {
            _warehouseCodeValidator = A.Fake<IValidator<WarehouseCode>>();
            _warehouseValidator = A.Fake<IValidator<Warehouse>>();
            _warehouseRepository = A.Fake<IWarehouseRepository>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<WarehouseLogic>>();
            _warehouseLogic = new WarehouseLogic(_warehouseValidator, _warehouseCodeValidator,_warehouseRepository,_mapper,_logger);
        }

        [Test]
        public void Get_ValidWarehousecode_ReturnsWarehouse()
        {
            ValidationResult validationResult = new ValidationResult();
            A.CallTo(() => _warehouseCodeValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _warehouseRepository.GetWarehouseByCode(null)).WithAnyArguments().Returns(Builder<Package.DataAccess.Entities.Warehouse>.CreateNew().Build());
            A.CallTo(_mapper).Where(x => x.Method.Name == "Map").WithNonVoidReturnType().Returns(Builder<Warehouse>.CreateNew().Build());
           
            Warehouse? result = _warehouseLogic.Get(new WarehouseCode("ABC"));

            result.Should().NotBeNull();
        }

        [Test]
        public void Get_InvalidWarehousecode_ArgumentException()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);
            Action act;
            A.CallTo(() => _warehouseCodeValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            act = () => _warehouseLogic.Get(null);

            act.Should().Throw<BusinessLayerExceptionBase>();
        }

        [Test]
        public void GetAll_ReturnsWarehouseList()
        {
            IReadOnlyCollection<Warehouse> result = _warehouseLogic.GetAll();

            result.Should().NotBeNull();
        }

        [Test]
        public void Add_ValidWarehouse_ReturnsTrue()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _warehouseValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(_mapper).Where(call => call.Method.Name == "Map").WithNonVoidReturnType().Returns(new Package.DataAccess.Entities.Warehouse());
            A.CallTo(() => _warehouseRepository.Create(null)).WithAnyArguments().Returns("A");

            bool result = _warehouseLogic.ReplaceHierarchy(Builder<Warehouse>.CreateNew().Build());
            
            result.Should().BeTrue();
        }

        [Test]
        public void Add_InvalidWarehouse_ArgumentException()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);
            Action act;
            A.CallTo(() => _warehouseValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            act = () => _warehouseLogic.ReplaceHierarchy(null);

            act.Should().Throw<BusinessLayerExceptionBase>();
        }
        
        [Test]
        public void Get_WarehouseIsNull_BusinessLayerDataNotFoundException()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _warehouseCodeValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
           A.CallTo(() => _warehouseRepository.GetWarehouseByCode(null)).WithAnyArguments().Returns(null);
           Action act;

           act = () => _warehouseLogic.Get(new WarehouseCode("ABC"));
           
           act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<BusinessLayerDataNotFoundException>();
        }
        
        [Test]
        public void Get_Throws_DataAccessExceptionBase()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _warehouseCodeValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _warehouseRepository.GetWarehouseByCode(null)).WithAnyArguments().Throws<DataAccessExceptionBase>();
            Action act;

            act = () => _warehouseLogic.Get(new WarehouseCode("ABC"));
           
            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<DataAccessExceptionBase>();
        }
        
        [Test]
        public void Get_Throws_ServiceAgentsExceptionBase()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _warehouseCodeValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _warehouseRepository.GetWarehouseByCode(null)).WithAnyArguments().Throws<ServiceAgentsExceptionBase>();
            Action act;

            act = () => _warehouseLogic.Get(new WarehouseCode("ABC"));
           
            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<ServiceAgentsExceptionBase>();
        }
        
        [Test]
        public void GetAll_Throws_ServiceAgentsExceptionBase()
        {
            A.CallTo(() => _warehouseRepository.GetAllWarehouses()).Throws<ServiceAgentsExceptionBase>();
            Action act;

            act = () => _warehouseLogic.GetAll();
           
            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<ServiceAgentsExceptionBase>();
        }
        
        [Test]
        public void GetAll_Throws_DataAccessException()
        {
            A.CallTo(() => _warehouseRepository.GetAllWarehouses()).Throws<DataAccessExceptionBase>();
            Action act;

            act = () => _warehouseLogic.GetAll();
           
            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<DataAccessExceptionBase>();
        }
        
        [Test]
        public void Add_Throws_ServiceException()
        {
            A.CallTo(() => _warehouseValidator.Validate(null)).WithAnyArguments()
                .Throws<ServiceAgentsExceptionBase>();
            Action act;

            act = () => _warehouseLogic.ReplaceHierarchy(null);
           
            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<ServiceAgentsExceptionBase>();
        }
        
        [Test]
        public void Add_Throws_DataAccessException()
        {
            A.CallTo(() => _warehouseValidator.Validate(null)).WithAnyArguments()
                .Throws<DataAccessExceptionBase>();
            Action act;

            act = () => _warehouseLogic.ReplaceHierarchy(null);
           
            act.Should().Throw<BusinessLayerExceptionBase>().WithInnerException<DataAccessExceptionBase>();
        }
    }
}