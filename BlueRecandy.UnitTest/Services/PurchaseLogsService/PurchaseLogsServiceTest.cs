using BlueRecandy.Models;
using BlueRecandy.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlueRecandy.UnitTest.Services.PurchaseLogsService
{
	public class PurchaseLogsServiceTest
	{

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

	}
}
