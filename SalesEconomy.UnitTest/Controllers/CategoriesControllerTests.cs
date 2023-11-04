using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SalesEconomy.BackEnd.Controllers;
using SalesEconomy.BackEnd.Data;
using SalesEconomy.BackEnd.Intertfaces;
using SalesEconomy.Shared.DTOs;
using SalesEconomy.Shared.Entities;

namespace SalesEconomy.UnitTest.Controllers
{
    [TestClass]
    public class CategoriesControllerTests
    {
        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<IGenericUnitOfWork<Category>> _unitOfWorkMock;
        public CategoriesControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _unitOfWorkMock = new Mock<IGenericUnitOfWork<Category>>();
        }

        [TestMethod]
        public async Task GetAsync_ReturnsOkResult()
        {
            // Arrange
            using var context = new DataContext(_options);
            var controller = new CategoriesController(_unitOfWorkMock.Object, context);
            var pagination = new PaginationDTO { Filter = "Some" };

            // Act
            var result = await controller.GetAsync(pagination) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            // Clean up (if needed)
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsOkResult()
        {
            // Arrange
            using var context = new DataContext(_options);
            var controller = new CategoriesController(_unitOfWorkMock.Object, context);
            var pagination = new PaginationDTO { Filter = "Some" };

            // Act
            var result = await controller.GetPagesAsync(pagination) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            // Clean up (if needed)
            context.Database.EnsureDeleted();
            context.Dispose();
        }

    }
}
