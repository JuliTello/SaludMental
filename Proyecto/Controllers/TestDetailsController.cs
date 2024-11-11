using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models.ViewModels;
using Proyecto.Repositories.Interfaces;
using Proyecto.Utilities;
using System.Security.Claims;

namespace Proyecto.Controllers;

public class TestDetailsController : Controller
{
    private readonly IUnitWork _unitWork;

    public TestDetailsController(IUnitWork unitWork)
    {
        _unitWork = unitWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin)]
    public IActionResult Create()
    {

        TestDetailVM testDetailVM = new TestDetailVM()
        {
            TestDetail = new Models.TestDetail(),
            TestList = _unitWork.TestDetail.ObtenerTodosDropdownLista("Test")
        };       
        return View(testDetailVM);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TestDetailVM testDetailVM)
    {
        if (testDetailVM == null) { return NotFound(); }

        if (ModelState.IsValid)
        {
            testDetailVM.TestDetail.FechaRealizacion = DateTime.Now;
            await _unitWork.TestDetail.AgregarAsync(testDetailVM.TestDetail);
            await _unitWork.GuardarAsync();

            TempData[DS.Successfull] = "Detalle Test creada correctamente.";

            return RedirectToAction("Index");
        }     
        testDetailVM.TestList = _unitWork.TestDetail.ObtenerTodosDropdownLista("Test");
        TempData[DS.Error] = "Error al guardar Test, intente de nuevo.";
        return View(testDetailVM);
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        TestDetailVM testDetailVM = new TestDetailVM()
        {
            TestDetail = new Models.TestDetail(),          
            TestList = _unitWork.TestDetail.ObtenerTodosDropdownLista("Test")
        };
        testDetailVM.TestDetail = await _unitWork.TestDetail.ObtenerAsync(id.GetValueOrDefault());

        if (testDetailVM is null) return NotFound();

        return View(testDetailVM);

    }

    [HttpPost]
    public async Task<IActionResult> Edit(TestDetailVM testDetailVM)
    {
        if (ModelState.IsValid)
        {
            _unitWork.TestDetail.Actualizar(testDetailVM.TestDetail);
            await _unitWork.GuardarAsync();

            TempData[DS.Successfull] = "Detalle Test actualizado correctamente";

            return RedirectToAction("Index");
        }

        return View(testDetailVM);
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        TestDetailVM testD = new TestDetailVM();
        testD.TestDetail = await _unitWork.TestDetail.ObtenerPrimeroAsync(filter: t => t.TestDetailId == id, includeProperties: "Test");

        if(testD is null) return NotFound();

        return View(testD);
    }

    #region API
    /// <summary>
    /// Listar todos los cursos registrados
    /// </summary>
    /// <returns>Json</returns>
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var claimIdentity = (ClaimsIdentity)this.User.Identity;
        var actualUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (User.IsInRole(DS.Role_Admin))
        {
           var testdetails = await _unitWork.TestDetail.ObtenerTodosAsync(
           includeProperties: "Test",
           orderBy: c => c.OrderByDescending(c => c.TestDetailId),
           isTracking: false);

            return Json(new { data = testdetails });
        }
        else
        {
            var testdetails = await _unitWork.TestDetail.ObtenerTodosAsync(
           includeProperties: "Test",
           orderBy: c => c.OrderByDescending(c => c.TestDetailId),
           isTracking: false,
           filter: t => t.ApplicationUserId == actualUser.Value);

            return Json(new { data = testdetails });
        }
    }

    /// <summary>
    /// Eliminar un registro enviado por Ajax
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Json</returns>
    [HttpPost]
    [Authorize(Roles = DS.Role_Admin )]
    public async Task<IActionResult> Delete(int id)
    {
        var testdetailDB = await _unitWork.TestDetail.ObtenerAsync(id);

        if (testdetailDB is null) // Si es nulo muestre un mensaje
            return Json(new { success = false, message = "Error al eliminar Detalle Test" });

        // Si eliminó correctamente muestre mensaje
        _unitWork.TestDetail.Remover(testdetailDB);
        await _unitWork.GuardarAsync();

        return Json(new { success = true, message = "Detalle Test eliminado correctamente" });
    }
    #endregion
}
