using BlueRecandy.Models;
using BlueRecandy.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlueRecandy.UnitTest.Services.ProductsService
{
	public class DeleteProductTest
	{

		[Fact]
		public void DeleteProduct_HasProduct_ShouldDeleted()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			mockService.Setup(m => m.DeleteProduct(It.IsAny<Product>())).ReturnsAsync(1);

			var service = mockService.Object;

			// Act
			var actionResult = service.DeleteProduct(It.IsAny<Product>());
			var result = actionResult.Result;

			// Assert
			Assert.Equal(1, result);
		}

	}
}
