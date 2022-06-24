using Xunit;
using Moq;
using BlueRecandy.Services;
using BlueRecandy.Models;
using BlueRecandy.Controllers;
using BlueRecandy.Data;
using Microsoft.AspNetCore.Identity;

namespace BlueRecandy.UnitTest
{
	public class ProductsServiceTest
	{
		[Fact]
		public void GetProductById_IdNotNull_ProductIsNotNull()
		{
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			mockService.Setup(m => m.GetProductById(1)).ReturnsAsync(product);
			var service = mockService.Object;

			var actionResult = service.GetProductById(product.Id);
			var result = actionResult.Result;

			Assert.NotNull(result);
		}

		[Fact]
		public void GetProductById_IdIsNull_ProductIsNull()
		{
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			mockService.Setup(m => m.GetProductById(1)).ReturnsAsync(product);
			var service = mockService.Object;

			var actionResult = service.GetProductById(null);
			var result = actionResult.Result;

			Assert.Null(result);
		}
	}
}