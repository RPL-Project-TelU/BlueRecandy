using BlueRecandy.Models;
using BlueRecandy.Services;
using Moq;
using Xunit;

namespace BlueRecandy.UnitTest.Services.ProductsService
{
	public class GetProductByIdTest
	{

		[Fact]
		public void GetProductById_IdNotNull_ProductIsNotNull()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			mockService.Setup(m => m.GetProductById(1)).ReturnsAsync(product);
			var service = mockService.Object;

			// Act
			var actionResult = service.GetProductById(product.Id);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
		}

		[Fact]
		public void GetProductById_IdIsNull_ProductIsNull()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			mockService.Setup(m => m.GetProductById(1)).ReturnsAsync(product);
			var service = mockService.Object;

			// Act
			var actionResult = service.GetProductById(null);
			var result = actionResult.Result;

			// Assert
			Assert.Null(result);
		}

	}
}
