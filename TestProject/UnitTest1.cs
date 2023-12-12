using RefitInterface;
using Moq;
using To_Do_List.API.Models;
using Microsoft.AspNetCore.Mvc;
using To_Do_List.Web.Models;
using Microsoft.AspNetCore.Http;
using To_Do_List.API.Controllers;
using To_Do_List.API.Repository;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task Login_ValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var authRepositoryMock = new Mock<IAuthRepository>();
            authRepositoryMock.Setup(repo => repo.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(new User { Id = new Guid(), Username = "validUser" });

            authRepositoryMock.Setup(repo => repo.CreateToken(It.IsAny<User>()))
                              .Returns("validToken");

            authRepositoryMock.Setup(repo => repo.GenerateRefreshToken())
                              .Returns(new RefreshToken { Token = "validRefreshToken" });

            var controller = new AuthController(authRepositoryMock.Object);

            // —оздаем фиктивный HttpContext с фиктивным Response
            var httpContextMock = new Mock<HttpContext>();
            var responseMock = new Mock<HttpResponse>();
            httpContextMock.Setup(c => c.Response).Returns(responseMock.Object);

            // Ќастраиваем ControllerContext с использованием фиктивного HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object,
            };

            // Act
            var result = await controller.Login(new UserDTO { Username = "validUser", Password = "validPassword" }) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("validToken", result.Value);
        }

        [TestMethod]
        public async Task Login_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var authRepositoryMock = new Mock<IAuthRepository>();
            authRepositoryMock.Setup(repo => repo.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync((User)null);

            var controller = new AuthController(authRepositoryMock.Object);

            // Act
            var result = await controller.Login(new UserDTO { Username = string.Empty, Password = string.Empty }) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Invalid username or password.", result.Value);
        }
    }
}