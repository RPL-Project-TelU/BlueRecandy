using BlueRecandy.Controllers;
using BlueRecandy.Models;
using BlueRecandy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlueRecandy.UnitTest.Controller.FeedbackController
{
	public class FeedbacksControllerTest
	{

		[Fact]
		public void Index()
		{

			// Arrange
			var mockFeedbackService = new Mock<IFeedbacksService>();
			var feedbacks = new List<Feedback>()
			{
				new Feedback(){ Id = 1, UserId = "1", ProductId = 1, FeedbackContent = "Test"}
			}.AsEnumerable();
			mockFeedbackService.Setup(x => x.GetAllFeedbacks()).Returns(feedbacks);
			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			// Act
			var result = controller.Index();

			// Assert
			Assert.NotNull(result);
			Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public void Details_HasId_HasFeedback_ShouldView()
		{
			var mockFeedbackService = new Mock<IFeedbacksService>(MockBehavior.Strict);
			var feedback = new Feedback() { Id = 1, FeedbackContent = "Wow", UserId = "aa1", ProductId = 1 };
			mockFeedbackService.Setup(x => x.GetFeedbacksById(1)).ReturnsAsync(feedback);

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Details(1);
			var result = actionResult.Result;

			Assert.NotNull(result);
			var view = Assert.IsType<ViewResult>(result);
			var model = view.Model;
			Assert.NotNull(model);
			Assert.Equal(feedback, model);
		}

		[Fact]
		public void Details_HasId_NoFeedback_ShouldNotFound()
		{
			var mockFeedbackService = new Mock<IFeedbacksService>(MockBehavior.Strict);
			mockFeedbackService.Setup(x => x.GetFeedbacksById(1)).ReturnsAsync(value: null);

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Details(1);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Details_NoId_NoFeedback_ShouldNotFound()
		{
			var mockFeedbackService = new Mock<IFeedbacksService>(MockBehavior.Strict);
			mockFeedbackService.Setup(x => x.GetFeedbacksById(null)).ReturnsAsync(value: null);

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Details(null);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Create_HasId_ShouldPrepared()
		{
			var controller = new FeedbacksController(null, null);

			var result = controller.Create(productId: 1);

			Assert.NotNull(result);
			Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public void Create_ValidFeedback_ShouldAdded()
		{
			// Arrange
			var mockFeedbackService = new Mock<IFeedbacksService>();
			var mockUserService = new Mock<IUsersService>();
			var mockHttpContext = new Mock<HttpContext>();
			var mockRequestQuery = new Mock<HttpRequest>();

			var user = new ApplicationUser() { Id = "user" };
			var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id)
			}));
			mockUserService.Setup(x => x.GetUserByClaims(principal)).ReturnsAsync(user);

			var feedback = new Feedback() { Id = 1, FeedbackContent = "wow", UserId = "user", ProductId = 1 };
			mockFeedbackService.Setup(x => x.AddFeedback(feedback)).ReturnsAsync(1);

			mockRequestQuery.Setup(x => x.Query["product"]).Returns(new string[] { $"{feedback.Id}" });

			mockHttpContext.Setup(x => x.User).Returns(principal);
			mockHttpContext.Setup(x => x.Request).Returns(mockRequestQuery.Object);

			var httpContext = mockHttpContext.Object;
			var controllerContext = new ControllerContext() { HttpContext = httpContext };
			var controller = new FeedbacksController(mockFeedbackService.Object, mockUserService.Object)
			{
				ControllerContext = controllerContext
			};

			// Act
			var actionResult = controller.Create(feedback);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<RedirectToActionResult>(result);
		}

		[Fact]
		public void Edit_HasNoId_ShouldNotFound()
		{
			// Arrange
			var mockFeedbackService = new Mock<IFeedbacksService>();

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Edit(null);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Edit_HasId_NoFeedback_ShouldNotFound()
		{
			// Arrange
			var mockFeedbackService = new Mock<IFeedbacksService>();
			mockFeedbackService.Setup(x => x.GetFeedbacksById(1)).ReturnsAsync(value: null);

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Edit(1);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Edit_HasId_HasFeedback_ShouldView()
		{
			// Arrange
			var mockFeedbackService = new Mock<IFeedbacksService>();
			mockFeedbackService.Setup(x => x.GetFeedbacksById(1)).ReturnsAsync(new Feedback() { Id = 1 });

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Edit(1);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public void Edit_HasId_HasFeedback_IsSubmiting_DifferentFeedbackId_ShouldNotFound()
		{
			// Arrange
			var mockFeedbackService = new Mock<IFeedbacksService>();

			var feedback = new Feedback() { Id = 1, FeedbackContent = "new", ProductId = 1, UserId = "aa"};
			mockFeedbackService.Setup(x => x.GetFeedbacksById(1)).ReturnsAsync(feedback);

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Edit(2, feedback);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.NotEqual(2, feedback.Id);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Edit_HasId_HasFeedback_IsSubmiting_SameFeedbackId_IsNotValid_ShouldView()
		{
			// Arrange
			var mockFeedbackService = new Mock<IFeedbacksService>();

			var feedback = new Feedback() { Id = 1, FeedbackContent = "new", ProductId = 1, UserId = "aa", Rating = -1 };
			mockFeedbackService.Setup(x => x.GetFeedbacksById(1)).ReturnsAsync(feedback);

			var controller = new FeedbacksController(mockFeedbackService.Object, null);
			controller.ModelState.AddModelError("Rating", "Rating cannot smaller than 1 or larger than 5");

			var actionResult = controller.Edit(1, feedback);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.Equal(1, feedback.Id);
			Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public void Edit_HasId_HasFeedback_IsSubmiting_SameFeedbackId_IsValid_ShouldRedirect()
		{
			// Arrange
			var mockFeedbackService = new Mock<IFeedbacksService>();

			var feedback = new Feedback() { Id = 1, FeedbackContent = "new", ProductId = 1, UserId = "aa", Rating = 3 };
			mockFeedbackService.Setup(x => x.GetFeedbacksById(1)).ReturnsAsync(feedback);
			mockFeedbackService.Setup(x => x.UpdateFeedback(feedback)).ReturnsAsync(1);

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Edit(1, feedback);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.Equal(1, feedback.Id);
			Assert.IsType<RedirectToActionResult>(result);
		}

		[Fact]
		public void Edit_HasId_HasFeedback_IsSubmiting_SameFeedbackId_IsValid_ConcurrentException_FeedbackNotExists_ShouldNotFound()
		{
			// Arrange
			var mockFeedbackService = new Mock<IFeedbacksService>();

			var feedback = new Feedback() { Id = 1, FeedbackContent = "new", ProductId = 1, UserId = "aa", Rating = 3 };
			mockFeedbackService.Setup(x => x.GetFeedbacksById(1)).ReturnsAsync(feedback);
			mockFeedbackService.Setup(x => x.IsFeedbackExists(1)).Returns(false);
			mockFeedbackService.Setup(x => x.UpdateFeedback(feedback)).Throws(new DbUpdateConcurrencyException());

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Edit(1, feedback);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.Equal(1, feedback.Id);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Delete_NoId_ShouldNotFound()
		{
			var mockFeedbackService = new Mock<IFeedbacksService>();

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Delete(null);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Delete_HasId_NoFeedback_ShouldNotFound()
		{
			var mockFeedbackService = new Mock<IFeedbacksService>();
			mockFeedbackService.Setup(x => x.GetFeedbacksById(1)).ReturnsAsync(value: null);

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Delete(1);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Delete_HasId_HasFeedback_ShouldView()
		{
			var mockFeedbackService = new Mock<IFeedbacksService>();

			var feedback = new Feedback() { Id = 1 };
			mockFeedbackService.Setup(x => x.GetFeedbacksById(1)).ReturnsAsync(feedback);

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.Delete(1);
			var result = actionResult.Result;

			Assert.NotNull(result);
			Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public void DeleteConfirmed_HasId_ShouldView()
		{
			var mockFeedbackService = new Mock<IFeedbacksService>();

			var feedback = new Feedback() { Id = 1 };
			mockFeedbackService.Setup(x => x.GetFeedbacksById(1)).ReturnsAsync(feedback);
			mockFeedbackService.Setup(x => x.DeleteFeedback(feedback)).ReturnsAsync(1);

			var controller = new FeedbacksController(mockFeedbackService.Object, null);

			var actionResult = controller.DeleteConfirmed(1);
			var result = actionResult.Result;

			Assert.NotNull(result);
			var redirect = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirect.ActionName);
		}

	}
}
