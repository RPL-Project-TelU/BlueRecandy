using BlueRecandy.Controllers;
using BlueRecandy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlueRecandy.UnitTest.Controller.HomeControllerTest
{
    public class HomeControllerTest
    {
        [Fact]
        public void Index_ReturnsAViewResult()
        {
            // Assert
            var mockService = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(mockService.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsAViewResult()
        {
            // Assert
            var mockService = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(mockService.Object);

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error()
        {
            // Assert
            var mockService = new Mock<ILogger<HomeController>>();
            var mockHttpContext = new Mock<HttpContext>();
            var httpContext = mockHttpContext.Object;
            var controllerContext = new ControllerContext() { HttpContext = httpContext };
            var controller = new HomeController(mockService.Object)
            {
                ControllerContext = controllerContext
            };

            // Act
            var result = controller.Error();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
