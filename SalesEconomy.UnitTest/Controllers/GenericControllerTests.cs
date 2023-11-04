using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SalesEconomy.BackEnd.Controllers;
using SalesEconomy.BackEnd.Data;
using SalesEconomy.BackEnd.Intertfaces;
using SalesEconomy.Shared.DTOs;
using SalesEconomy.Shared.Entities;
using SalesEconomy.Shared.Responses;

namespace SalesEconomy.UnitTest.Controllers
{
    [TestClass]
    public class GenericControllerTests
    {
        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<IGenericUnitOfWork<Category>> _unitOfWorkMock;

        public GenericControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _unitOfWorkMock = new Mock<IGenericUnitOfWork<Category>>();
        }

        [TestMethod]
        public async Task GetAsync_ReturnsOkResult()
        {
            // Arrange
            using var context = new DataContext(_options);
            var controller = new GenericController<Category>(_unitOfWorkMock.Object, context);
            var pagination = new PaginationDTO();

            // Act
            var result = await controller.GetAsync(pagination) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            // Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsOkResult()
        {
            // Arrange
            using var context = new DataContext(_options);
            var controller = new GenericController<Category>(_unitOfWorkMock.Object, context);
            var pagination = new PaginationDTO();

            // Act
            var result = await controller.GetPagesAsync(pagination) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            // Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAsync_ReturnsNotFoundWhenEntityNotFound()
        {
            // Arrange
            using var context = new DataContext(_options);
            var controller = new GenericController<Category>(_unitOfWorkMock.Object, context);

            // Act
            var result = await controller.GetAsync(1) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);

            // Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetAsync_ReturnsRecord()
        {
            // Arrange
            using var context = new DataContext(_options);
            var category = new Category { Id = -1, Name = "Any" };

            _unitOfWorkMock.Setup(x => x.GetAsync(category.Id))
                .ReturnsAsync(category);

            var controller = new GenericController<Category>(_unitOfWorkMock.Object, context);

            // Act
            var result = await controller.GetAsync(category.Id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            _unitOfWorkMock.Verify(x => x.GetAsync(category.Id), Times.Once());

            // Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task PostAsync_ReturnsOkResult()
        {
            // Arrange
            using var context = new DataContext(_options);
            var category = new Category { Id = 1, Name = "Any" };
            var response = new Response<Category> { WasSuccess = true, Result = category };

            _unitOfWorkMock.Setup(x => x.AddAsync(category))
                .ReturnsAsync(response);

            var controller = new GenericController<Category>(_unitOfWorkMock.Object, context);

            // Act
            var result = await controller.PostAsync(category) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var okResult = result.Value as Category;
            Assert.AreEqual(category.Name, okResult!.Name);
            _unitOfWorkMock.Verify(x => x.AddAsync(category), Times.Once());

            // Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task PostAsync_ReturnsBadRequest()
        {
            // Arrange
            using var context = new DataContext(_options);
            var category = new Category { Id = 1, Name = "Any" };
            var response = new Response<Category> { WasSuccess = false };

            _unitOfWorkMock.Setup(x => x.AddAsync(category))
                .ReturnsAsync(response);

            var controller = new GenericController<Category>(_unitOfWorkMock.Object, context);

            // Act
            var result = await controller.PostAsync(category) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            _unitOfWorkMock.Verify(x => x.AddAsync(category), Times.Once());

            // Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task PutAsync_ReturnsOkResult()
        {
            // Arrange
            using var context = new DataContext(_options);
            var category = new Category { Id = 1, Name = "test" };
            var response = new Response<Category> { WasSuccess = true };
            _unitOfWorkMock.Setup(x => x.UpdateAsync(category)).ReturnsAsync(response);
            var controller = new GenericController<Category>(_unitOfWorkMock.Object, context);

            // Act
            var result = await controller.PutAsync(category) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            _unitOfWorkMock.Verify(x => x.UpdateAsync(category), Times.Once());

            // Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task PutAsync_ReturnsBadRequest()
        {
            // Arrange
            using var context = new DataContext(_options);
            var category = new Category { Id = 1, Name = "test" };
            var response = new Response<Category> { WasSuccess = false };
            _unitOfWorkMock.Setup(x => x.UpdateAsync(category)).ReturnsAsync(response);
            var controller = new GenericController<Category>(_unitOfWorkMock.Object, context);

            // Act
            var result = await controller.PutAsync(category) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            _unitOfWorkMock.Verify(x => x.UpdateAsync(category), Times.Once());

            // Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnsNoContentWhenEntityDeleted()
        {
            // Arrange
            using var context = new DataContext(_options);
            var category = new Category { Id = 1, Name = "test" };
            _unitOfWorkMock.Setup(x => x.GetAsync(category.Id)).ReturnsAsync(category);
            var controller = new GenericController<Category>(_unitOfWorkMock.Object, context);

            // Act
            var result = await controller.DeleteAsync(category.Id) as NoContentResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);
            _unitOfWorkMock.Verify(x => x.GetAsync(category.Id), Times.Once());

            // Clean up (if needed)
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnsNoContentWhenEntityNotFound()
        {
            // Arrange
            using var context = new DataContext(_options);
            var category = new Category { Id = 1, Name = "test" };
            var controller = new GenericController<Category>(_unitOfWorkMock.Object, context);

            // Act
            var result = await controller.DeleteAsync(category.Id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);

            // Clean up (if needed)
            context.Database.EnsureDeleted();
        }
    }
}
