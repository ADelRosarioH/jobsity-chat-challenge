using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.WebAPI.Controllers;
using JobsityStocksChat.WebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JobsityStocksChat.UnitTests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private NewUserViewModel newUser;
        private ApplicationUser appUser;
        private UserLoginViewModel loginUser;

        private AuthController authController;

        private Mock<UserManager<ApplicationUser>> userManagerMock;
        private Mock<SignInManager<ApplicationUser>> signInManagerMock;
        private Mock<ITokenClaimService> tokenClaimServiceMock;

        [SetUp]
        public void SetUp()
        {
            newUser = new NewUserViewModel
            {
                UserName = "test",
                Email = "test@test.com",
                Password = "password"
            };

            appUser = new ApplicationUser
            {
                UserName = "test",
                Email = "test@test.com",
            };

            loginUser = new UserLoginViewModel { 
                UserName = "test",
                Password = "Test123*"
            };

            IUserStore<ApplicationUser> userStoreMock = Mock.Of<IUserStore<ApplicationUser>>();

            userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock, null, null, null, null, null, null, null, null);
            userManagerMock.SetReturnsDefault(Task.FromResult(IdentityResult.Success));

            signInManagerMock = new Mock<SignInManager<ApplicationUser>>(userManagerMock.Object,
                  new Mock<IHttpContextAccessor>().Object,
                  new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                  new Mock<IAuthenticationSchemeProvider>().Object,
                  new Mock<IUserConfirmation<ApplicationUser>>().Object);

            signInManagerMock.Setup(m => m.PasswordSignInAsync(appUser.UserName, "password", false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            tokenClaimServiceMock = new Mock<ITokenClaimService>();
            tokenClaimServiceMock.Setup(m => m.GetToken(appUser.UserName)).Returns("token_string");

            authController = new AuthController(userManagerMock.Object, signInManagerMock.Object, tokenClaimServiceMock.Object, null);
        }

        [Test]
        public async Task Should_RegisterNewUser()
        {
            // Act
            var result = await authController.Register(newUser);

            // Assert
            var okResult = (OkObjectResult)result;

            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.IsTrue(okResult.Value.ToString().Contains("New user was registered."));
        }

        [Test]
        public async Task Should_ReturnBadRequest_When_ModelInvalid()
        {
            // Arrange

            var invalidNewUser = new NewUserViewModel
            {
                UserName = "test"
            };

            // Act
            authController.ModelState.AddModelError("Email", "Email Is Required.");
            var result = await authController.Register(invalidNewUser);

            // Assert
            var badResult = (BadRequestObjectResult)result;

            Assert.AreEqual((int)HttpStatusCode.BadRequest, badResult.StatusCode);
            Assert.IsTrue(badResult.Value.ToString().Contains("User registration failed."));
        }

        [Test]
        public async Task Should_ReturnBadRequest_When_UserManagerFails()
        {
            // Arranges
            IUserStore<ApplicationUser> userStoreMock = Mock.Of<IUserStore<ApplicationUser>>();

            Mock<UserManager<ApplicationUser>> userManagerMockThatFails =
                new Mock<UserManager<ApplicationUser>>(userStoreMock, null, null, null, null, null, null, null, null);

            userManagerMockThatFails.SetReturnsDefault(Task.FromResult(IdentityResult.Failed()));

            authController = new AuthController(userManagerMockThatFails.Object, signInManagerMock.Object, tokenClaimServiceMock.Object, null);

            // Act
            var result = await authController.Register(newUser);

            // Assert
            var badResult = (BadRequestObjectResult)result;

            Assert.AreEqual((int)HttpStatusCode.BadRequest, badResult.StatusCode);
            Assert.IsTrue(badResult.Value.ToString().Contains("User registration failed."));
        }

        [Test]
        public async Task Should_ReturnBadRequest_When_UserLoginCredentialsAreMissing()
        {
            //Arranges
            authController.ModelState.AddModelError("UserName", "Empty");

            // Act
            var result = await authController.Login(new UserLoginViewModel { });

            // Assert
            var badResult = (BadRequestObjectResult)result;

            Assert.AreEqual((int)HttpStatusCode.BadRequest, badResult.StatusCode);
            Assert.IsTrue(badResult.Value.ToString().Contains("Missing UserName or Password."));
        }

        [Test]
        public async Task Should_ReturnBadRequest_When_UserLoginCredentialsAreInvalid()
        {
            // Arranges
            signInManagerMock.Setup(m => m.PasswordSignInAsync(loginUser.UserName, loginUser.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            authController = new AuthController(userManagerMock.Object, signInManagerMock.Object, tokenClaimServiceMock.Object, null);

            // Act
            var result = await authController.Login(loginUser);

            // Assert
            var badResult = (UnauthorizedObjectResult)result;

            Assert.AreEqual((int)HttpStatusCode.Unauthorized, badResult.StatusCode);
            Assert.IsTrue(badResult.Value.ToString().Contains("Invalid UserName or Password."));
        }

        [Test]
        public async Task Should_ReturnToken_When_UserLoginCredentialsAreValid()
        {
            // Arranges
            signInManagerMock.Setup(m => m.PasswordSignInAsync(loginUser.UserName, loginUser.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            authController = new AuthController(userManagerMock.Object, signInManagerMock.Object, tokenClaimServiceMock.Object, null);

            // Act
            var result = await authController.Login(loginUser);

            // Assert
            var okResult = (OkObjectResult)result;

            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.IsTrue(okResult.Value.ToString().Contains("token_string"));
        }
    }
}
