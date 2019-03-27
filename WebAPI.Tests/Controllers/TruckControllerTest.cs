using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using NUnit.Framework;
using System.IO;
using TruckSelling.WebAPI;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TruckSelling.WebAPI.Tests;
using TruckSelling.WebAPI.Controllers;
using TruckSelling.WebAPI.Models;

namespace TruckSelling.WebAPI.Tests
{
    public class TestsTruckController
    {
        private TruckController _truckController;
        private ModelController _modelController;

        [SetUp]
        public void Setup()
        {
            var builder = new TestsContextBuilder();
            builder.Build();
            _truckController = new TruckController(builder.AppDbContext);
            _modelController = new ModelController(builder.AppDbContext);
        }

        [Test]
        public async Task CreateTruck_ChassisAlreadyExists_ReturnsConflict()
        {
            // create model
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult1 = await _modelController.PostModel(model);
            var modelCreatedResult = actionResult1 as CreatedAtActionResult;
            Assert.IsNotNull(modelCreatedResult);
            Assert.AreEqual(201, modelCreatedResult.StatusCode);
            model.ModelId = Convert.ToInt32(modelCreatedResult.RouteValues["id"]);

            // create truck
            var truck = new Truck();
            truck.Chassis = Guid.NewGuid().ToString();
            truck.Year = Convert.ToInt16(new Random().Next(1900, 2019));
            truck.Model = model;

            IActionResult actionResult2 = await _truckController.PostTruck(truck);
            var truckCreatedResult = actionResult2 as CreatedAtActionResult;
            Assert.IsNotNull(truckCreatedResult);
            Assert.AreEqual(201, truckCreatedResult.StatusCode);

            // try create another with same chassis
            IActionResult actionResult3 = await _truckController.PostTruck(truck);
            var conflictResult = actionResult3 as ConflictResult;
            Assert.IsNotNull(conflictResult);
            Assert.AreEqual(409, conflictResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task CreateTruck_ReturnsCreated()
        {
            // create model
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult1 = await _modelController.PostModel(model);
            var modelCreatedResult = actionResult1 as CreatedAtActionResult;
            Assert.IsNotNull(modelCreatedResult);
            Assert.AreEqual(201, modelCreatedResult.StatusCode);
            model.ModelId = Convert.ToInt32(modelCreatedResult.RouteValues["id"]);

            // create truck
            var truck = new Truck();
            truck.Chassis = Guid.NewGuid().ToString();
            truck.Year = Convert.ToInt16(new Random().Next(1900, 2019));
            truck.Model = model;

            IActionResult actionResult2 = await _truckController.PostTruck(truck);
            var truckCreatedResult = actionResult2 as CreatedAtActionResult;
            Assert.IsNotNull(truckCreatedResult);
            Assert.AreEqual(201, truckCreatedResult.StatusCode);
        }

        [Test]
        public async Task UpdateTruck_ChassisAlreadyExists_ReturnsConflict()
        {
            // create model
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult = await _modelController.PostModel(model);
            var modelCreatedResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(modelCreatedResult);
            Assert.AreEqual(201, modelCreatedResult.StatusCode);
            model.ModelId = Convert.ToInt32(modelCreatedResult.RouteValues["id"]);

            // create truck 1 
            var truck1 = new Truck();
            truck1.Chassis = Guid.NewGuid().ToString();
            truck1.Year = Convert.ToInt16(new Random().Next(1900, 2019));
            truck1.Model = model;

            actionResult = await _truckController.PostTruck(truck1);
            var createdResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // create truck 2
            var truck2 = new Truck();
            truck2.Chassis = Guid.NewGuid().ToString();
            truck2.Year = Convert.ToInt16(new Random().Next(1900, 2019));
            truck2.Model = model;

            actionResult = await _truckController.PostTruck(truck2);
            createdResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // try to update truck 2 with truck1.chassis
            truck2.Chassis = truck1.Chassis;
            actionResult = await _truckController.PutTruck(truck2.TruckId, truck2);
            var conflictResult = actionResult as ConflictResult;
            Assert.IsNotNull(conflictResult);
            Assert.AreEqual(409, conflictResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task UpdateTruck_NoConflict_ReturnsNoContent()
        {
            // create model
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult = await _modelController.PostModel(model);
            var modelCreatedResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(modelCreatedResult);
            Assert.AreEqual(201, modelCreatedResult.StatusCode);
            model.ModelId = Convert.ToInt32(modelCreatedResult.RouteValues["id"]);

            // create truck 
            var truck = new Truck();
            truck.Chassis = Guid.NewGuid().ToString();
            truck.Year = Convert.ToInt16(new Random().Next(1900, 2019));
            truck.Model = model;

            actionResult = await _truckController.PostTruck(truck);
            var createdResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // update truck 
            actionResult = await _truckController.PutTruck(truck.TruckId, truck);
            var noContentResult = actionResult as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task UpdateTruck_NotExists_ReturnsNotFound()
        {
            // create model
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult = await _modelController.PostModel(model);
            var modelCreatedResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(modelCreatedResult);
            Assert.AreEqual(201, modelCreatedResult.StatusCode);
            model.ModelId = Convert.ToInt32(modelCreatedResult.RouteValues["id"]);

            // instantiate truck
            var truck = new Truck();
            truck.TruckId = 999999;
            truck.Chassis = Guid.NewGuid().ToString();
            truck.Year = Convert.ToInt16(new Random().Next(1900, 2019));
            truck.Model = model;

            // try to update
            actionResult = await _truckController.PutTruck(999999, truck);
            var notFoundResult = actionResult as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task GetTrucks()
        {
            // create model
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult = await _modelController.PostModel(model);
            var modelCreatedResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(modelCreatedResult);
            Assert.AreEqual(201, modelCreatedResult.StatusCode);
            model.ModelId = Convert.ToInt32(modelCreatedResult.RouteValues["id"]);

            // create truck
            var truck = new Truck();
            truck.Chassis = Guid.NewGuid().ToString();
            truck.Year = Convert.ToInt16(new Random().Next(1900, 2019));
            truck.Model = model;

            actionResult = await _truckController.PostTruck(truck);
            var createdResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // get all
            var models = _truckController.GetTrucks();
            Assert.IsNotNull(models);
            Assert.IsTrue(models.Count() > 0);

            Assert.Pass();
        }

        [Test]
        public async Task GetTruck_Exists_ReturnsOK()
        {
            // create model
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult = await _modelController.PostModel(model);
            var modelCreatedResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(modelCreatedResult);
            Assert.AreEqual(201, modelCreatedResult.StatusCode);
            model.ModelId = Convert.ToInt32(modelCreatedResult.RouteValues["id"]);

            // create truck
            var truck = new Truck();
            truck.Chassis = Guid.NewGuid().ToString();
            truck.Year = Convert.ToInt16(new Random().Next(1900, 2019));
            truck.Model = model;

            actionResult = await _truckController.PostTruck(truck);
            var createdResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // get first
            var list = _truckController.GetTrucks();
            truck = list.FirstOrDefault<Truck>();

            // get by id
            actionResult = await _truckController.GetTruck(truck.TruckId);
            var okResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task GetTruck_NotExists_ReturnsNotFound()
        {
            // get
            IActionResult actionResult = await _truckController.GetTruck(999999);
            var notFoundResult = actionResult as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task DeleteTruck_Exists_ReturnsOk()
        {
            // create model
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult = await _modelController.PostModel(model);
            var modelCreatedResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(modelCreatedResult);
            Assert.AreEqual(201, modelCreatedResult.StatusCode);
            model.ModelId = Convert.ToInt32(modelCreatedResult.RouteValues["id"]);

            // create truck
            var truck = new Truck();
            truck.Chassis = Guid.NewGuid().ToString();
            truck.Year = Convert.ToInt16(new Random().Next(1900, 2019));
            truck.Model = model;

            actionResult = await _truckController.PostTruck(truck);
            var createdResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // delete truck
            actionResult = await _truckController.DeleteTruck(truck.TruckId);
            var okResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task DeleteTruck_NotExists_ReturnsNotFound()
        {
            // delete
            IActionResult actionResult = await _truckController.DeleteTruck(999999);
            var notFoundResult = actionResult as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            Assert.Pass();
        }
    }
}
