using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Proyecto.Controllers;
using Proyecto.Models;
using Proyecto.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaUnitTest

{
    [TestClass]
    public class TestsControllerTests
    {
        private Mock<IUnitWork> _mockUnitWork;
        private TestsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitWork = new Mock<IUnitWork>();
            _controller = new TestsController(_mockUnitWork.Object);

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }

        [TestMethod]
        public async Task Create_Post_ValidTest_ReturnsRedirectToAction()
        {
            // Arrange
            var test = new Test
            {
                NombreTest = "Test de ejemplo",
                Descripcion = "Descripción del test",
                Pregunta1 = "Pregunta 1",
                Pregunta2 = "Pregunta 2",
                Pregunta3 = "Pregunta 3",
                Pregunta4 = "Pregunta 4",
                Pregunta5 = "Pregunta 5"
            };

            _mockUnitWork.Setup(u => u.Test.AgregarAsync(It.IsAny<Test>())).Returns(Task.CompletedTask);
            _mockUnitWork.Setup(u => u.GuardarAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(test) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

        }

        [TestMethod]
        public async Task Edit_Get_ValidId_ReturnsViewWithTest()
        {
            // Arrange
            int testId = 1;
            var test = new Test
            {
                TestId = testId,
                NombreTest = "Test de ejemplo",
                Descripcion = "Descripción del test"
            };

            _mockUnitWork.Setup(u => u.Test.ObtenerAsync(testId)).ReturnsAsync(test);

            // Act
            var result = await _controller.Edit(testId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as Test;
            Assert.IsNotNull(model);
            Assert.AreEqual(testId, model.TestId);
        }

        [TestMethod]
        public async Task Edit_Post_ValidTest_ReturnsRedirectToAction()
        {
            // Arrange
            var test = new Test
            {
                TestId = 1,
                NombreTest = "Test actualizado",
                Descripcion = "Descripción actualizada"
            };

            _mockUnitWork.Setup(u => u.Test.Actualizar(It.IsAny<Test>()));
            _mockUnitWork.Setup(u => u.GuardarAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(test) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            _mockUnitWork.Verify(u => u.Test.Actualizar(It.IsAny<Test>()), Times.Once);
            _mockUnitWork.Verify(u => u.GuardarAsync(), Times.Once);
        }

        //[TestMethod]
        //public async Task Delete_ValidId_ReturnsJsonSuccess()
        //{
        //    // Arrange
        //    int testId = 1;
        //    var test = new Test { TestId = testId };

        //    _mockUnitWork.Setup(u => u.Test.ObtenerAsync(testId)).ReturnsAsync(test);
        //    _mockUnitWork.Setup(u => u.Test.Remover(test));
        //    _mockUnitWork.Setup(u => u.GuardarAsync()).Returns(Task.CompletedTask);

        //    // Act
        //    var result = await _controller.Delete(testId) as JsonResult;

        //    // Assert
        //    Assert.IsNotNull(result);

        //    // Cast the result value to a dictionary
        //    var data = result.Value as Dictionary<string, object>;

        //    Assert.IsTrue((bool)data["success"]);
        //    Assert.AreEqual("Test eliminado correctamente", data["message"].ToString());

        //    _mockUnitWork.Verify(u => u.Test.Remover(test), Times.Once);
        //    _mockUnitWork.Verify(u => u.GuardarAsync(), Times.Once);
        //}


        //[TestMethod]
        //public async Task Delete_InvalidId_ReturnsJsonError()
        //{
        //    // Arrange
        //    int invalidTestId = 99;

        //    _mockUnitWork.Setup(u => u.Test.ObtenerAsync(invalidTestId)).ReturnsAsync((Test)null);

        //    // Act
        //    var result = await _controller.Delete(invalidTestId) as JsonResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    dynamic data = result.Value;
        //    Assert.IsFalse(data.success);
        //    Assert.AreEqual("Error al eliminar el Test", data.message);
        //    _mockUnitWork.Verify(u => u.Test.Remover(It.IsAny<Test>()), Times.Never);
        //    _mockUnitWork.Verify(u => u.GuardarAsync(), Times.Never);
        //}

    }
}