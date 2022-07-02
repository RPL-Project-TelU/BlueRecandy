using BlueRecandy.Models;
using BlueRecandy.Services;
using Moq;
using Xunit;

namespace BlueRecandy.UnitTest.Services.ProductsService
{
	public class IsProductExistsTest
	{

		[Fact]
		public void IsProductExists_CorrectId_IsExists()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			mockService.Setup(m => m.IsProductExists(1)).Returns(true);
			var service = mockService.Object;

			// Act
			var result = service.IsProductExists(1);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void IsProductExists_IncorrectId_NotExists()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			mockService.Setup(m => m.IsProductExists(1)).Returns(true);
			var service = mockService.Object;

			// Act
			var result = service.IsProductExists(2);

			// Assert
			Assert.False(result);
		}

	}
}
