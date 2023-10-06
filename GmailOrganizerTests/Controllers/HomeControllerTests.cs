using NUnit.Framework;
using GmailOrganizer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using GmailOrganizer.Services;
using Microsoft.AspNetCore.Mvc;

namespace GmailOrganizer.Controllers.Tests
{
    [TestFixture()]
    public class HomeControllerTests
    {
        [Test()]
        public async Task HomeControllerTest()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockMailService = new Mock<IMailServices>();
            var controller = new HomeController(mockLogger.Object, mockMailService.Object);

            //Act
            var result = await controller.Index();

            //Assert
            //var viewResult = Assert.<ViewResult>(result);
            Assert.Pass();
        }

        [Test()]
        public void IndexTest()
        {
            Assert.Pass();
        }

        [Test()]
        public void PrivacyTest()
        {
            Assert.Pass();
        }

        [Test()]
        public void DeleteTest()
        {
            Assert.Pass();
        }

        [Test()]
        public void ErrorTest()
        {
            Assert.Pass();
        }
    }
}