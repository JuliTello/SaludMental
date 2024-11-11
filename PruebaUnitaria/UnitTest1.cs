using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Proyecto.Controllers;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Repositories.Interfaces;
using Proyecto.Utilities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MSTest_Citas
{
    [TestClass]
    public class CitasControllerTests
    {
        private Mock<IUnitWork> _mockUnitWork;
        private CitasController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitWork = new Mock<IUnitWork>();
            _controller = new CitasController(_mockUnitWork.Object);
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }

        [TestMethod]
        public async Task Create_Post_ValidCita_ReturnsRedirectToAction()
        {
            // Arrange
            var citaVM = new CitaVM
            {
                Cita = new Cita
                {
                    SessionDate = DateTime.Now,
                    Estado = "ACTIVA",
                    EspecialistaId = 1
                }
            };

            // Mock the dropdown list
            _mockUnitWork.Setup(u => u.Cita.ObtenerTodosDropdownLista(It.IsAny<string>()))
                .Returns(new List<SelectListItem>());

            // Mock the user identity
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "user-id-123")
    };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Mock agregar cita y guardar
            _mockUnitWork.Setup(u => u.Cita.AgregarAsync(It.IsAny<Cita>())).Returns(Task.CompletedTask);
            _mockUnitWork.Setup(u => u.GuardarAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(citaVM) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }


        [TestMethod]
        public async Task Edit_Post_ValidCita_ReturnsRedirectToAction()
        {
            // Arrange
            var citaVM = new CitaVM
            {
                Cita = new Cita
                {
                    CitaId = 1,
                    SessionDate = DateTime.Now,
                    Estado = "ACTIVA",
                    EspecialistaId = 1
                }
            };

            _mockUnitWork.Setup(u => u.Cita.Actualizar(It.IsAny<Cita>()));
            _mockUnitWork.Setup(u => u.GuardarAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(citaVM) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

    }
}