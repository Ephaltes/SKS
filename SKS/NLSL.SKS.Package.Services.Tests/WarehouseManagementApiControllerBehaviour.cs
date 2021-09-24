using System;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using NLSL.SKS.Package.Services.Controllers;
using NLSL.SKS.Package.Services.DTOs;

using NUnit.Framework;

namespace NLSL.SKS.Package.Services.Tests
{
    public class WarehouseManagementApiControllerBehaviour
    {
        private WarehouseManagementApiController testController;
        
        [SetUp]
        public void Setup()
        {
            testController = new WarehouseManagementApiController();
        }
        [Test]
        public void ExportWarehouses_ReturnsListOfWareHouses_Success()
        {
            ObjectResult result;
            result =(ObjectResult) testController.ExportWarehouses();

            result.StatusCode.Should().Be(200);
        }
        [Test]
        public void GetWarehouse_TestCode_Success()
        {
            ObjectResult result;
            
            result =(ObjectResult) testController.GetWarehouse("test");

            result.StatusCode.Should().Be(200);
        }
        [Test]
        public void GetWarehouse_CodeNotFound_StatusCode404()
        {
            ObjectResult result;
            
            result =(ObjectResult) testController.GetWarehouse("test222");

            result.StatusCode.Should().Be(404);
        }
        
        [Test]
        public void ImportWarehouses_Warehouse_Success()
        {
            Warehouse warehouse = new Warehouse();
            StatusCodeResult result;
            
            result =(StatusCodeResult) testController.ImportWarehouses(warehouse);

            result.StatusCode.Should().Be(200);
        }
        
        [Test]
        public void ImportWarehouses_NullWareHouse_StatusCode400()
        {
            StatusCodeResult result;
            
            result =(StatusCodeResult) testController.ImportWarehouses(null);

            result.StatusCode.Should().Be(400);
        }
    }
}
