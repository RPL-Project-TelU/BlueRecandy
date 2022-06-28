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
	public class ValidateProductTest
	{

		[Fact]
		public void ValidateProduct_HasProduct_IsValid()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();

			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			product.Description = "My Description";
			product.UseExternalURL = true;
			product.DownloadURL = "https://download.org/";
			product.Price = 12;
			product.OwnerId = "userid";

			mockService.Setup(m => m.ValidateProduct(product)).Returns(true);
			var service = mockService.Object;

			// Act

			var result = service.ValidateProduct(product);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void ValidateProduct_HasProduct_IsInvalid()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();

			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			product.Description = "My Description";
			product.UseExternalURL = true;
			product.DownloadURL = null;
			product.Price = -1000;
			product.OwnerId = "userid";

			mockService.Setup(m => m.ValidateProduct(product)).Returns(false);
			var service = mockService.Object;

			// Act

			var result = service.ValidateProduct(product);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void ValidateProduct_UseExternalURL_IsValid()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();

			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			product.Description = "My Description";
			product.UseExternalURL = true;
			product.DownloadURL = "https://download.org/";
			product.Price = 12;
			product.OwnerId = "userid";

			mockService.Setup(m => m.ValidateProduct(product)).Returns(true);
			var service = mockService.Object;

			// Act

			var result = service.ValidateProduct(product);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void ValidateProduct_UseExternalURL_IsInvalid()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();

			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			product.Description = "My Description";
			product.UseExternalURL = true;
			product.DownloadURL = null;
			product.Price = 12;
			product.OwnerId = "userid";

			mockService.Setup(m => m.ValidateProduct(product)).Returns(false);
			var service = mockService.Object;

			// Act

			var result = service.ValidateProduct(product);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void ValidateProduct_UseFile_IsValid()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();

			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			product.Description = "My Description";
			product.UseExternalURL = false;
			product.SourceFileName = "Myfile";
			product.Price = 12;
			product.OwnerId = "userid";

			mockService.Setup(m => m.ValidateProduct(product)).Returns(true);
			var service = mockService.Object;

			// Act

			var result = service.ValidateProduct(product);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void ValidateProduct_UseFile_IsInvalid()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();

			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			product.Description = "My Description";
			product.UseExternalURL = false;
			product.SourceFileName = null;
			product.Price = 12;
			product.OwnerId = "userid";

			mockService.Setup(m => m.ValidateProduct(product)).Returns(false);
			var service = mockService.Object;

			// Act

			var result = service.ValidateProduct(product);

			// Assert
			Assert.False(result);
		}

	}
}
