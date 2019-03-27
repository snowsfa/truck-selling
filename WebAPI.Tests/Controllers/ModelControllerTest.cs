using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using NUnit.Framework;
using System.IO;
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
    public class TestsModelController
    {
        private ModelController _modelController;

        [SetUp]
        public void Setup()
        {
            var builder = new TestsContextBuilder();
            builder.Build();
            _modelController = new ModelController(builder.AppDbContext);
        }

        [Test]
        public async Task CreateModel_ModelAlreadyExists_ReturnsConflict()
        {
            // create
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult1 = await _modelController.PostModel(model);
            var createdResult = actionResult1 as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // try create another with same description
            IActionResult actionResult2 = await _modelController.PostModel(model);
            var conflictResult = actionResult2 as ConflictResult;
            Assert.IsNotNull(conflictResult);
            Assert.AreEqual(409, conflictResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task CreateModel_ReturnsCreated()
        {
            // create
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult = await _modelController.PostModel(model);
            var createdResult = actionResult as CreatedAtActionResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            Assert.Pass();            
        }

        [Test]
        public async Task UpdateModel_ModelAlreadyExists_ReturnsConflict()
        {
            // create model 1 
            var model1 = new Model();
            model1.Description = Guid.NewGuid().ToString();

            IActionResult actionResult1 = await _modelController.PostModel(model1);
            var createdResult = actionResult1 as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // create model 2
            var model2 = new Model();
            model2.Description = Guid.NewGuid().ToString();

            actionResult1 = await _modelController.PostModel(model2);
            createdResult = actionResult1 as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // try to update model 2 with model1.description
            model2.Description = model1.Description;
            actionResult1 = await _modelController.PutModel(model2.ModelId, model2);
            var conflictResult = actionResult1 as ConflictResult;
            Assert.IsNotNull(conflictResult);
            Assert.AreEqual(409, conflictResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task UpdateModel_NoConflict_ReturnsNoContent()
        {
            // create 
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult = await _modelController.PostModel(model);
            var createdResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // update
            model.Description = Guid.NewGuid().ToString();

            actionResult = await _modelController.PutModel(model.ModelId, model);
            var noContentResult = actionResult as NoContentResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task UpdateModel_NotExists_ReturnsNotFound()
        {
            var model = new Model();
            model.ModelId = 999999;
            model.Description = Guid.NewGuid().ToString();

            // try to update
            IActionResult actionResult = await _modelController.PutModel(999999, model);
            var notFoundResult = actionResult as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task GetModels()
        {
            // create
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult = await _modelController.PostModel(model);
            var createdResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // get all
            var models =  _modelController.GetModels();
            Assert.IsNotNull(models);
            Assert.IsTrue(models.Count() > 0);

            Assert.Pass();
        }

        [Test]
        public async Task GetModel_Exists_ReturnsOK()
        {
            // create
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult = await _modelController.PostModel(model);
            var createdResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // get first
            var list = _modelController.GetModels();
            model = list.FirstOrDefault<Model>();

            // get by id
            actionResult = await _modelController.GetModel(model.ModelId);
            var okResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task GetModel_NotExists_ReturnsNotFound()
        {
            // get
            IActionResult actionResult = await _modelController.GetModel(999999);
            var notFoundResult = actionResult as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task DeleteModel_Exists_ReturnsOk()
        {
            // create 
            var model = new Model();
            model.Description = Guid.NewGuid().ToString();

            IActionResult actionResult = await _modelController.PostModel(model);
            var createdResult = actionResult as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            // delete
            model.Description = Guid.NewGuid().ToString();

            actionResult = await _modelController.DeleteModel(model.ModelId);
            var okResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            Assert.Pass();
        }

        [Test]
        public async Task DeleteModel_NotExists_ReturnsNotFound()
        {
             // delete
             IActionResult actionResult = await _modelController.DeleteModel(999999);
            var notFoundResult = actionResult as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            Assert.Pass();
        }
    }
}