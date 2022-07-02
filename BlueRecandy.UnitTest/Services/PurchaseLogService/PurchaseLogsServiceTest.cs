using BlueRecandy.Data;
using BlueRecandy.Models;
using BlueRecandy.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlueRecandy.UnitTest.Services.PurchaseLogService
{
	public class PurchaseLogsServiceTest
	{

		[Fact]
		public void AddPurchaseLog()
		{
			var mockLogService = new Mock<IPurchaseLogsService>();

			var log = new PurchaseLog() { ProductId = 1, UserId = "user" };
			mockLogService.Setup(x => x.AddPurchaseLog(log)).ReturnsAsync(1);

			var service = mockLogService.Object;

			var actionResult = service.AddPurchaseLog(log);
			var result = actionResult.Result;

			Assert.NotNull(actionResult);
			Assert.Equal(1, result);
		}

		[Fact]
		public void GetPurchaseLogs_EmptyLogs_ShouldReturnEmpty()
		{
			// Arrange
			var logs = Enumerable.Empty<PurchaseLog>();

			var mockLogService = new Mock<IPurchaseLogsService>();
			mockLogService.Setup(x => x.GetPurchaseLogs()).Returns(logs);

			var service = mockLogService.Object;
			// Act
			var result = service.GetPurchaseLogs();

			// Assert
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void GetPurchaseLogs_HasLogs_ShouldNotEmpty()
		{
			// Arrange
			var logs = new List<PurchaseLog>()
			{
				new PurchaseLog(){ UserId = "user_id",  ProductId = 1 }
			}.AsEnumerable();

			var mockLogService = new Mock<IPurchaseLogsService>();
			mockLogService.Setup(x => x.GetPurchaseLogs()).Returns(logs);

			var service = mockLogService.Object;
			// Act
			var result = service.GetPurchaseLogs();

			// Assert
			Assert.NotNull(result);
			Assert.Single(result);
		}

		[Fact]
		public void GetPurchaseLogsByProductId_NoLogs_ShouldEmpty()
		{
			// Arrange
			var mockLogService = new Mock<IPurchaseLogsService>(MockBehavior.Strict);
			
			var logs = Enumerable.Empty<PurchaseLog>();
			mockLogService.Setup(x => x.GetPurchaseLogsByProductId(1)).Returns(logs);

			var service = mockLogService.Object;
			// Act
			var results = service.GetPurchaseLogsByProductId(1);

			// Assert
			Assert.NotNull(results);
			Assert.Empty(results);
		}

		[Fact]
		public void GetPurchaseLogsByProductId_HasLogs_ShouldNotEmpty()
		{
			// Arrange
			var mockLogService = new Mock<IPurchaseLogsService>(MockBehavior.Strict);

			var logs = new List<PurchaseLog>(){
				new PurchaseLog() { ProductId = 1, UserId = "a" }
			}.AsEnumerable();
			mockLogService.Setup(x => x.GetPurchaseLogsByProductId(1)).Returns(logs);

			var service = mockLogService.Object;
			// Act
			var results = service.GetPurchaseLogsByProductId(1);

			// Assert
			Assert.NotNull(results);
			Assert.NotEmpty(results);
		}

		[Fact]
		public void GetPurchaseLogsByUserId_HasUser_EmptyLog_ShouldEmpty()
		{
			var mockLogService = new Mock<IPurchaseLogsService>();

			var logs = Enumerable.Empty<PurchaseLog>();
			mockLogService.Setup(x => x.GetPurchaseLogsByUserId("user")).Returns(logs);

			var service = mockLogService.Object;

			var result = service.GetPurchaseLogsByUserId("user");

			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void GetPurchaseLogsByUserId_HasUser_HasLog_ShouldNotEmpty()
		{
			var mockLogService = new Mock<IPurchaseLogsService>();

			var logs = new List<PurchaseLog>()
			{
				new PurchaseLog(){ProductId = 1, UserId = "user"}
			}.AsEnumerable();
			mockLogService.Setup(x => x.GetPurchaseLogsByUserId("user")).Returns(logs);

			var service = mockLogService.Object;

			var result = service.GetPurchaseLogsByUserId("user");

			Assert.NotNull(result);
			Assert.NotEmpty(result);
		}
	}
}
