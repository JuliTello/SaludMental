using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Proyecto.Controllers;
using Proyecto.Models;
using Proyecto.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MSTest_Especialista
{
    [TestClass]
    public class EspecialistasControllerTests
    {
        private Mock<IUnitWork> _mockUnitWork;
        private EspecialistasController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitWork = new Mock<IUnitWork>();
            _controller = new EspecialistasController(_mockUnitWork.Object);

            // Simula TempData
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
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
        public void Create_Get_ReturnsViewResultWithEspecialista()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(Especialista));
            var especialista = result.Model as Especialista;
            Assert.IsNotNull(especialista);
            Assert.IsTrue(especialista.CreatedAt > DateTime.MinValue);
            Assert.IsTrue(especialista.UpdatedAt > DateTime.MinValue);
        }
        [TestMethod]
        public async Task Create_Post_ValidEspecialista_ReturnsRedirectToAction()
        {
            // Arrange
            var especialista = new Especialista
            {
                UserName = "testuser",
                PhoneNumber = "123456789",
                Email = "test@example.com",
                Password = "password",
                Age = 30, // Asumiendo que Age es un entero
                Gender = "Male",
                FirstName = "First",
                LastName = "Last",
                CodigoColegiatura = "123",
                Especialidad = "Specialty",
                Status = true
            };

            // Configurar el mock para los métodos que se llaman en la acción
            _mockUnitWork.Setup(u => u.Especialista.AgregarAsync(especialista)).Returns(Task.CompletedTask);
            _mockUnitWork.Setup(u => u.GuardarAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(especialista) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }


        [TestMethod]
        public async Task Edit_Get_ValidId_ReturnsViewResultWithEspecialista()
        {
            // Arrange
            var especialistaId = 1;
            var especialista = new Especialista { EspecialistaId = especialistaId };
            _mockUnitWork.Setup(u => u.Especialista.ObtenerAsync(especialistaId)).ReturnsAsync(especialista);

            // Act
            var result = await _controller.Edit(especialistaId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(Especialista));
            var model = result.Model as Especialista;
            Assert.AreEqual(especialistaId, model.EspecialistaId);
        }

        [TestMethod]
        public async Task Edit_Post_ValidEspecialista_ReturnsRedirectToAction()
        {
            // Arrange
            var especialista = new Especialista { EspecialistaId = 1, UserName = "testuser" };
            _mockUnitWork.Setup(u => u.Especialista.Actualizar(especialista));
            _mockUnitWork.Setup(u => u.GuardarAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(especialista) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        //[TestMethod]
        //public async Task Delete_ValidId_ReturnsJsonResult()
        //{
        //    // Arrange
        //    int validId = 1;
        //    var especialista = new Especialista { EspecialistaId = validId };

        //    // Configura el mock para devolver un especialista válido
        //    _mockUnitWork.Setup(u => u.Especialista.ObtenerAsync(validId))
        //                 .ReturnsAsync(especialista);

        //    _mockUnitWork.Setup(u => u.Especialista.Remover(especialista));
        //    _mockUnitWork.Setup(u => u.GuardarAsync()).Returns(Task.CompletedTask);

        //    // Act
        //    var result = await _controller.Delete(validId) as JsonResult;

        //    // Assert
        //    Assert.IsNotNull(result, "El resultado de la acción Delete es null.");

        //    var response = result.Value as IDictionary<string, object>;
        //    Assert.IsNotNull(response, "El resultado JSON no se deserializó correctamente.");

        //    // Verifica la propiedad 'success'
        //    Assert.IsTrue(response.ContainsKey("success"), "El resultado JSON no contiene la propiedad 'success'.");
        //    var success = Convert.ToBoolean(response["success"]);
        //    Assert.IsTrue(success, "El valor de 'success' no es verdadero.");

        //    // Verifica la propiedad 'message'
        //    Assert.IsTrue(response.ContainsKey("message"), "El resultado JSON no contiene la propiedad 'message'.");
        //    var message = response["message"].ToString();
        //    Assert.AreEqual("Especialista eliminado correctamente", message);
        //}

    }
}