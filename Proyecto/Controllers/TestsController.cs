using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Repositories.Interfaces;
using Proyecto.Utilities;
using System.Security.Claims;

namespace Proyecto.Controllers;

public class TestsController : Controller
{
    private readonly IUnitWork _unitWork;

    public TestsController(IUnitWork unitWork)
    {
        _unitWork = unitWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente)]
    public IActionResult Create()
    {
        Test test = new Test();
        test.CreatedAt = DateTime.Now;
        return View(test);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Test test)
    {
        if (test == null) { return NotFound(); }

        if (ModelState.IsValid)
        {
            test.CreatedAt = DateTime.Now;

            await _unitWork.Test.AgregarAsync(test);
            await _unitWork.GuardarAsync();

            TempData["Successful"] = "Test creado correctamente";

            return RedirectToAction("Index");
        }

        return View(test);
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin)]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        Test test = new Test();
        test = await _unitWork.Test.ObtenerAsync(id.GetValueOrDefault());

        if (test is null) return NotFound();

        return View(test);

    }

    [HttpPost]
    public async Task<IActionResult> Edit(Test test)
    {
        if (ModelState.IsValid)
        {
            _unitWork.Test.Actualizar(test);
            await _unitWork.GuardarAsync();

            TempData["Successful"] = "Test actualizado correctamente";

            return RedirectToAction("Index");
        }

        return View(test);
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        Test test = new Test();
        test = await _unitWork.Test.ObtenerPrimeroAsync(filter: t => t.TestId == id);

        return View(test);
    }

    [HttpPost]
    public async Task<IActionResult> Details(int testId, int resultado)
    {
        var claimIdentity = (ClaimsIdentity)this.User.Identity;
        var actualUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

        Test test = new Test();
        test = await _unitWork.Test.ObtenerPrimeroAsync(filter: c => c.TestId == testId);

        TestDetail testDetails = new TestDetail();
        testDetails.ApplicationUserId = actualUser.Value;
        testDetails.TestId = testId;
        testDetails.Resultado = resultado;
        testDetails.FechaRealizacion = DateTime.Now;

        await _unitWork.TestDetail.AgregarAsync(testDetails);
        await _unitWork.GuardarAsync();
        TempData[DS.Successfull] = "Test agregado correctamente";

        return RedirectToAction("Details", new { id = testId });
    }

    #region API
    /// <summary>
    /// Listar todos los cursos registrados
    /// </summary>
    /// <returns>Json</returns>
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var tests = await _unitWork.Test.ObtenerTodosAsync(
            orderBy: c => c.OrderByDescending(c => c.TestId),
            isTracking: false);

        return Json(new { data = tests });
    }

    /// <summary>
    /// Eliminar un registro enviado por Ajax
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Json</returns>
    [HttpPost]
    [Authorize(Roles = DS.Role_Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var testDB = await _unitWork.Test.ObtenerAsync(id);

        if (testDB is null) // Si es nulo muestre un mensaje
            return Json(new { success = false, message = "Error al eliminar el Test" });

        // Si eliminó correctamente muestre mensaje
        _unitWork.Test.Remover(testDB);
        await _unitWork.GuardarAsync();

        return Json(new { success = true, message = "Test eliminado correctamente" });
    }
    #endregion
}
