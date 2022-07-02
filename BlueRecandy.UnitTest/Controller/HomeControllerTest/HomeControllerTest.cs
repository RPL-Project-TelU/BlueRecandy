using BlueRecandy.Controllers;
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
    }
}
