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
        public void DeleteFeedback_HasFeedback_ShouldDelete()
        {
            var mockService = new Mock<IFeedbacksService>();
            var feedback = new Feedback() { Id = 1, UserId = "aaaa", FeedbackContent = "Wow", ProductId = 1 };
            mockService.Setup(x => x.DeleteFeedback(feedback)).ReturnsAsync(1);
            var service = mockService.Object;

            var actionResult = service.DeleteFeedback(feedback);
            var result = actionResult.Result;

            Assert.Equal(1, result);
        }

        [Fact]
        public void DeleteFeedback_NoFeedback_ShouldNotDelete()
        {
            var mockService = new Mock<IFeedbacksService>();
            var feedback = new Feedback() { Id = 1, UserId = "aaaa", FeedbackContent = "Wow", ProductId = 1 };
            mockService.Setup(x => x.DeleteFeedback(feedback)).ReturnsAsync(0);
            var service = mockService.Object;

            var actionResult = service.DeleteFeedback(feedback);
            var result = actionResult.Result;

            Assert.Equal(0, result);
        }

        [Fact]
        public void AddFeedback_HasFeedback_ShouldAdded()
		{
            var mockService = new Mock<IFeedbacksService>();
            var feedback = new Feedback() { Id = 1, UserId = "aaaa", FeedbackContent = "Wow", ProductId = 1};
            mockService.Setup(x => x.AddFeedback(feedback)).ReturnsAsync(1);
            var service = mockService.Object;

            var actionResult = service.AddFeedback(feedback);
            var result = actionResult.Result;

            Assert.Equal(1, result);
		}

        [Fact]
        public void UpdateFeedback_HasFeedback_ShouldUpdated()
        {
            var mockService = new Mock<IFeedbacksService>();
            var feedback = new Feedback() { Id = 1, UserId = "aaaa", FeedbackContent = "Wow", ProductId = 1 };
            mockService.Setup(x => x.UpdateFeedback(feedback)).ReturnsAsync(1);
            var service = mockService.Object;

            var actionResult = service.UpdateFeedback(feedback);
            var result = actionResult.Result;

            Assert.Equal(1, result);
        }

        [Fact]
        public void IsFeedbackExists_HasFeedback_ShouldExists()
        {
            var mockService = new Mock<IFeedbacksService>();
            mockService.Setup(x => x.IsFeedbackExists(1)).Returns(true);
            var service = mockService.Object;

            var result = service.IsFeedbackExists(1);

            Assert.True(result);
        }

        [Fact]
        public void IsFeedbackExists_NoFeedback_ShouldNotExists()
        {
            var mockService = new Mock<IFeedbacksService>();
            mockService.Setup(x => x.IsFeedbackExists(1)).Returns(false);
            var service = mockService.Object;

            var result = service.IsFeedbackExists(1);

            Assert.False(result);
        }

        [Fact]
        public void GetAllFeedbacks_HasFeedback_ShouldNotEmpty()
        {
            var mockService = new Mock<IFeedbacksService>();
            var feedback = new Feedback() { Id = 1, UserId = "aaaa", FeedbackContent = "Wow", ProductId = 1 };
            var feedbacks = new List<Feedback>() { feedback }.AsEnumerable();
            mockService.Setup(x => x.GetAllFeedbacks()).Returns(feedbacks);
            var service = mockService.Object;

            var result = service.GetAllFeedbacks();

            Assert.NotNull(result);
            Assert.Single(result);
        }


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
            var actionResult = service.GetFeedbacksById(1);
            var result = actionResult.Result;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetFeedbacksById_IdNotNull_FeedbacksIsNull()
        {
            // Arrange
            var mockService = new Mock<IFeedbacksService>();
            var feedback = new Feedback();
            feedback.Id = 1;
            feedback.FeedbackContent = "bagus";
            mockService.Setup(m => m.GetFeedbacksById(1)).ReturnsAsync(value: null);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetFeedbacksById(1);
            var result = actionResult.Result;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetFeedbacksById_IdIsNull_FeedbacksIsNull()
        {
            // Arrange
            var mockService = new Mock<IFeedbacksService>();
            var feedback = new Feedback();
            feedback.Id = 1;
            feedback.FeedbackContent = "bagus";
            mockService.Setup(m => m.GetFeedbacksById(null)).ReturnsAsync(value: null);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetFeedbacksById(null);
            var result = actionResult.Result;

            // Assert
            Assert.Null(result);
        }
    }
}
