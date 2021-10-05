using System.Collections.Generic;
using System.Linq;

using FakeItEasy;

using FizzWare.NBuilder;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using NLSL.SKS.Package.BusinessLogic.Entities;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class WarehouseManagementBehaviour
    {
        private IValidator<WarehouseCode> _warehouseCodeValidator;
        private WarehouseManagement _warehouseManagement;
        private IValidator<Warehouse> _warehouseValidator;

        [SetUp]
        public void Setup()
        {
            _warehouseCodeValidator = A.Fake<IValidator<WarehouseCode>>();
            _warehouseValidator = A.Fake<IValidator<Warehouse>>();

            _warehouseManagement = new WarehouseManagement(_warehouseValidator, _warehouseCodeValidator);
        }

        [Test]
        public void Get_ValidWarehousecode_ReturnsWarehouse()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _warehouseCodeValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            Warehouse? result = _warehouseManagement.Get(null);

            result.Should().NotBeNull();
        }

        [Test]
        public void Get_InvalidWarehousecode_ReturnsNull()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);

            A.CallTo(() => _warehouseCodeValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            Warehouse? result = _warehouseManagement.Get(null);

            result.Should().BeNull();
        }

        [Test]
        public void GetAll_ReturnsWarehouseList()
        {
            IReadOnlyCollection<Warehouse> result = _warehouseManagement.GetAll();

            result.Should().NotBeNull();
        }

        [Test]
        public void Add_ValidWarehouse_ReturnsTrue()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _warehouseValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            bool result = _warehouseManagement.Add(null);

            result.Should().BeTrue();
        }

        [Test]
        public void Add_InvalidWarehouse_ReturnsFalse()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);

            A.CallTo(() => _warehouseValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            bool result = _warehouseManagement.Add(null);

            result.Should().BeFalse();
        }
    }
}