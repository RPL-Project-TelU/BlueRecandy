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
	public class UpdateProductTest
	{

		[Fact]
		public void UpdateProduct_ValidProduct_ShouldValid()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var mockProduct = new Product();
			mockProduct.Id = 1;
			mockProduct.Name = "Tes";
			mockProduct.Price = 0;
			mockProduct.UseExternalURL = true;
			mockProduct.DownloadURL = "Web here..";

			mockService.Setup(x => x.UpdateProduct(mockProduct)).ReturnsAsync(1);

			var service = mockService.Object;

			// Act
			mockProduct.Price = 1000;
			var actionResult = service.UpdateProduct(mockProduct);
			var result = actionResult.Result;

			// Assert
			Assert.Equal(1, result);
		}

		[Fact]
		public void UpdateProduct_InvalidProduct_ShouldInvalid()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var mockProduct = new Product();
			mockProduct.Id = 1;
			mockProduct.Name = "Tes";
			mockProduct.Price = 0;
			mockProduct.UseExternalURL = true;
			mockProduct.DownloadURL = "Web here..";

			mockService.Setup(x => x.UpdateProduct(mockProduct)).ReturnsAsync(0);

			var service = mockService.Object;

			// Act
			mockProduct.Price = -10;
			var actionResult = service.UpdateProduct(mockProduct);
			var result = actionResult.Result;

			// Assert
			Assert.Equal(0, result);
		}

	}
}
