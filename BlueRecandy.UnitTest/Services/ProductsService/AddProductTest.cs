using BlueRecandy.Models;
using BlueRecandy.Services;
using Moq;
using Xunit;

namespace BlueRecandy.UnitTest.Services.ProductsService
{
	public class AddProductTest
	{

		[Fact]
		public void AddProduct_AnyProduct_ShouldAdded()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			mockService.Setup(x => x.AddProduct(It.IsAny<Product>())).ReturnsAsync(1);

			var service = mockService.Object;
			// Act
			var actionResult = service.AddProduct(It.IsAny<Product>());
			var result = actionResult.Result;

			// Assert
			Assert.Equal(1, result);
		}

	}
}
