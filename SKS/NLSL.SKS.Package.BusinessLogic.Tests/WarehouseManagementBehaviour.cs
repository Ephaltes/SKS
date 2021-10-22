using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using FakeItEasy;

using FizzWare.NBuilder;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class WarehouseManagementBehaviour
    {
        private IValidator<WarehouseCode> _warehouseCodeValidator;
        private WarehouseLogic _warehouseLogic;
        private IValidator<Warehouse> _warehouseValidator;
        private IWarehouseRepository _warehouseRepository;
        private IMapper _mapper;
        [SetUp]
        public void Setup()
        {
            _warehouseCodeValidator = A.Fake<IValidator<WarehouseCode>>();
            _warehouseValidator = A.Fake<IValidator<Warehouse>>();
            _warehouseRepository = A.Fake<IWarehouseRepository>();
            _mapper = A.Fake<IMapper>();
            _warehouseLogic = new WarehouseLogic(_warehouseValidator, _warehouseCodeValidator,_warehouseRepository,_mapper);
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

            act.Should().Throw<ArgumentException>();
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

            bool result = _warehouseLogic.Add(Builder<Warehouse>.CreateNew().Build());
            
            result.Should().BeTrue();
        }

        [Test]
        public void Add_InvalidWarehouse_ArgumentException()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);
            Action act;
            A.CallTo(() => _warehouseValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            act = () => _warehouseLogic.Add(null);

            act.Should().Throw<ArgumentException>();
        }
    }
}