using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Newtonsoft.Json;
using Proyecto.Controllers;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Repositories.Interfaces;
using Proyecto.Utilities;
using System.Dynamic;
using System.Security.Claims;


namespace PruebaUnitMensajeria
{
    [TestClass]
    public class MensajeriasControllerTests
    {
        private Mock<IUnitWork> _mockUnitWork;
        private MensajeriasController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitWork = new Mock<IUnitWork>();
            _controller = new MensajeriasController(_mockUnitWork.Object);

            // Simula TempData
            _controller.TempData = new Mock<ITempDataDictionary>().Object;

            // Simula el usuario en el contexto
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "TestUser")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TestMethod]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Create_Get_ReturnsViewWithModel()
        {
            // Arrange
            _mockUnitWork.Setup(u => u.Mensajeria.ObtenerTodosDropdownLista("Especialista"))
                .Returns(new List<SelectListItem>()); // Simula el retorno de la lista de especialistas

            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(MensajeriaVM));
        }

        [TestMethod]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var mensajeriaVM = new MensajeriaVM
            {
                Mensajeria = new Mensajeria
                {
                    MensajeriaId = 1,
                    ApplicationUserId = "TestUser"
                }
            };

            _mockUnitWork.Setup(u => u.Mensajeria.AgregarAsync(It.IsAny<Mensajeria>())).Returns(Task.CompletedTask);
            _mockUnitWork.Setup(u => u.GuardarAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(mensajeriaVM) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            _mockUnitWork.Verify(u => u.Mensajeria.AgregarAsync(It.IsAny<Mensajeria>()), Times.Once);
            _mockUnitWork.Verify(u => u.GuardarAsync(), Times.Once);
        }
        [TestMethod]
        public async Task Create_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model");
            var mensajeriaVM = new MensajeriaVM
            {
                Mensajeria = new Mensajeria()
            };

            // Mock User Identity
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockUnitWork.Setup(u => u.Mensajeria.ObtenerTodosDropdownLista(It.IsAny<string>()))
                .Returns(new List<SelectListItem>());

            // Act
            var result = await _controller.Create(mensajeriaVM) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mensajeriaVM, result.Model);
        }

        [TestMethod]
        public async Task Edit_Get_ValidId_ReturnsViewWithModel()
        {
            // Arrange
            var mensajeria = new Mensajeria { MensajeriaId = 1 };
            _mockUnitWork.Setup(u => u.Mensajeria.ObtenerAsync(It.IsAny<int>())).ReturnsAsync(mensajeria);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(MensajeriaVM));
        }

        [TestMethod]
        public async Task Edit_Get_InvalidId_ReturnsNotFound()
        {
            // Arrange
            //_mockUnitWork.Setup(u => u.Mensajeria.ObtenerAsync(It.IsAny<int>())).ReturnsAsync((Mensajeria)null);
            int? invalidId = -1; // o null
            _mockUnitWork.Setup(uow => uow.Mensajeria.ObtenerAsync(It.IsAny<int>()))
                .ReturnsAsync((Mensajeria)null);
            // Act
            //var result = await _controller.Edit(999);
            var result = await _controller.Edit(invalidId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var mensajeriaVM = new MensajeriaVM
            {
                Mensajeria = new Mensajeria
                {
                    MensajeriaId = 1,
                    ApplicationUserId = "TestUser"
                }
            };

            _mockUnitWork.Setup(u => u.Mensajeria.Actualizar(It.IsAny<Mensajeria>()));
            _mockUnitWork.Setup(u => u.GuardarAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(mensajeriaVM) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            _mockUnitWork.Verify(u => u.Mensajeria.Actualizar(It.IsAny<Mensajeria>()), Times.Once);
            _mockUnitWork.Verify(u => u.GuardarAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Edit_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model");

            var mensajeriaVM = new MensajeriaVM
            {
                Mensajeria = new Mensajeria()
            };

            // Act
            var result = await _controller.Edit(mensajeriaVM) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mensajeriaVM, result.Model);
        }

        [TestMethod]
        public async Task Delete_ValidId_ReturnsSuccessJson()
        {
            // Arrange
            var mensajeria = new Mensajeria { MensajeriaId = 1 };
            _mockUnitWork.Setup(u => u.Mensajeria.ObtenerAsync(It.IsAny<int>())).ReturnsAsync(mensajeria);

            // Act
            var result = await _controller.Delete(1) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(result.Value));

            Assert.IsTrue(jsonResult.ContainsKey("success"));
            Assert.AreEqual(true, Convert.ToBoolean(jsonResult["success"]));
        }

        [TestMethod]
        public async Task Delete_InvalidId_ReturnsErrorJson()
        {
            // Arrange
            _mockUnitWork.Setup(u => u.Mensajeria.ObtenerAsync(It.IsAny<int>())).ReturnsAsync((Mensajeria)null);

            // Act
            var result = await _controller.Delete(999) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(result.Value));

            Assert.IsTrue(jsonResult.ContainsKey("success"));
            Assert.AreEqual(false, Convert.ToBoolean(jsonResult["success"]));
            Assert.IsTrue(jsonResult.ContainsKey("message"));
            Assert.AreEqual("Error al eliminar mensaje", jsonResult["message"]);
        }
    }

}