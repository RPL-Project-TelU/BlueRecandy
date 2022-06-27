using BlueRecandy.Models;
using BlueRecandy.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlueRecandy.UnitTest.Services.FeedbacksService
{
    public class FeedbacksServiceTest
    {
        [Fact]
        public void GetFeedbacksById_IdNotNull_FeedbacksIsNotNull()
        {
            // Arrange
            var mockService = new Mock<IFeedbacksService>();
            var feedback = new Feedback();
            feedback.Id = 1;
            feedback.FeedbackContent = "bagus";
            mockService.Setup(m => m.GetFeedbacksById(1)).ReturnsAsync(feedback);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetFeedbacksById(feedback.Id);
            var result = actionResult.Result;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetFeedbacksById_IdIsNull_FeedbacksIsNull()
        {
            // Arrange
            var mockService = new Mock<IFeedbacksService>();
            var feedback = new Feedback();
            feedback.Id = 1;
            feedback.FeedbackContent = "bagus";
            mockService.Setup(m => m.GetFeedbacksById(1)).ReturnsAsync(feedback);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetFeedbacksById(null);
            var result = actionResult.Result;

            // Assert
            Assert.Null(result);
        }
    }
}
