using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using FakeItEasy;

using FizzWare.NBuilder;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.Services.Controllers;

using NUnit.Framework;

namespace NLSL.SKS.Package.Services.Tests
{
    public class WarehouseManagementApiControllerBehaviour
    {
        private IMapper _mapper;
        private WarehouseManagementApiController _testController;
        private IWarehouseLogic _warehouseLogic;
        private ILogger<WarehouseManagementApiController> _logger;

        [SetUp]
        public void Setup()
        {
            _mapper = A.Fake<IMapper>();
            _warehouseLogic = A.Fake<IWarehouseLogic>();
            _logger = A.Fake<ILogger<WarehouseManagementApiController>>();

            _testController = new WarehouseManagementApiController(_warehouseLogic, _mapper,_logger);
        }
        [Test]
        public void ExportWarehouses_ReturnsListOfWareHouses_Success()
        {
            ObjectResult result;
            IReadOnlyCollection<Warehouse> warehouseList = Builder<Warehouse>.CreateListOfSize(2).Build().ToList();
            A.CallTo(() => _warehouseLogic.GetAll()).Returns(warehouseList);

            result = (ObjectResult)_testController.ExportWarehouses();

            result.StatusCode.Should().Be(200);
        }
        [Test]
        public void ExportWarehouses_NoWarehouses_EmptyList()
        {
            ObjectResult result;
            A.CallTo(() => _warehouseLogic.GetAll()).Returns(new List<Warehouse>());

            result = (ObjectResult)_testController.ExportWarehouses();

            result.StatusCode.Should().Be(404);
        }
        [Test]
        public void GetWarehouse_Code_Success()
        {
            ObjectResult result;
            A.CallTo(() => _warehouseLogic.Get(A<WarehouseCode>.Ignored)).Returns(new Warehouse());

            result = (ObjectResult)_testController.GetWarehouse("test");

            result.StatusCode.Should().Be(200);
        }
        [Test]
        public void GetWarehouse_CodeNotFound_StatusCode404()
        {
            ObjectResult result;
            A.CallTo(() => _warehouseLogic.Get(A<WarehouseCode>.Ignored)).Returns(null);

            result = (ObjectResult)_testController.GetWarehouse("test222");

            result.StatusCode.Should().Be(404);
        }

        [Test]
        public void ImportWarehouses_Warehouse_Success()
        {
            DTOs.Warehouse warehouse = new DTOs.Warehouse();
            StatusCodeResult result;
            A.CallTo(() => _warehouseLogic.Add(null)).WithAnyArguments().Returns(true);
            
            result = (StatusCodeResult)_testController.ImportWarehouses(warehouse);

            result.StatusCode.Should().Be(200);
        }

        [Test]
        public void ImportWarehouses_NullWareHouse_StatusCode400()
        {
            DTOs.Warehouse warehouse = new DTOs.Warehouse();
            ObjectResult result;
            A.CallTo(() => _warehouseLogic.Add(null)).WithAnyArguments().Returns(false);
            
            result = (ObjectResult)_testController.ImportWarehouses(warehouse);

            result.StatusCode.Should().Be(400);
        }
    }
}