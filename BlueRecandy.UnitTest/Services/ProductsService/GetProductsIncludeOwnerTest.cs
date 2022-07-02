using BlueRecandy.Models;
using BlueRecandy.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlueRecandy.UnitTest.Services.ProductsService
{
	public class GetProductsIncludeOwnerTest
	{
		[Fact]
		public void GetProductsIncludeOwner_NoProducts_EmptyQuery()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var emptyEnum = Enumerable.Empty<Product>();
			mockService.Setup(m => m.GetProductsIncludeOwner()).Returns(emptyEnum.AsQueryable());
			var service = mockService.Object;

			// Act
			var actionResult = service.GetProductsIncludeOwner();

			// Assert
			Assert.Equal(Enumerable.Empty<Product>(), actionResult);
		}

		[Fact]
		public void GetProductsIncludeOwner_HasProducts_HasQuery()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = It.IsAny<int>();
			product.Name = It.IsAny<string>();

			var products = new List<Product>() { product };

			mockService.Setup(m => m.GetProductsIncludeOwner()).Returns(products.AsQueryable());
			var service = mockService.Object;

			// Act
			var actionResult = service.GetProductsIncludeOwner();

			// Assert
			Assert.True(actionResult.Count() > 0);
		}
	}
}
