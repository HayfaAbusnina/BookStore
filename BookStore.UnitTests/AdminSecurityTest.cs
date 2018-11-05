using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Models;
using BookStore.WebUI.Controllers;
using System.Web.Mvc;

namespace BookStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTest
    {
        [TestMethod]
        public void Can_Login_With_Valid_Crediential()
        {
            //Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin","admin")).Returns(true);

            LoginViewModel model = new LoginViewModel
            {
                Username = "admin",
                Password = "admin"
            };
            AccountController target = new AccountController(mock.Object);

            //act
            ActionResult result = target.Login(model, "/myUrl");

            //Assert
            Assert.IsInstanceOfType(result,typeof(RedirectResult));
            Assert.AreEqual("/myUrl", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Can_Login_With_InValid_Crediential()
        {
            //Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("adminX", "adminX")).Returns(false);

            LoginViewModel model = new LoginViewModel
            {
                Username = "adminX",
                Password = "adminX"
            };
            AccountController target = new AccountController(mock.Object);

            //act
            ActionResult result = target.Login(model, "/myUrl");

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
