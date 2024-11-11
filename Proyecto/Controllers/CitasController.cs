using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Repositories.Interfaces;
using Proyecto.Utilities;
using System.Security.Claims;

namespace Proyecto.Controllers;

//[Authorize(Roles = DS.Role_Admin)]
public class CitasController : Controller
{
    private readonly IUnitWork _unitWork;
    public CitasController(IUnitWork unitWork)
    {
        _unitWork = unitWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public IActionResult Create()
    {

        CitaVM citaVM = new CitaVM()
        {
            Cita = new Models.Cita(),
            EspecialistaList = _unitWork.Cita.ObtenerTodosDropdownLista("Especialista")
        };
        //citaVM.Cita.Estado = "ACTIVA";
        return View(citaVM);
    }

    [HttpPost]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Create(CitaVM citaVM)
    {
        if (citaVM is null) { return NotFound(); }

        if (ModelState.IsValid)
        {
            var claimIdentity = (ClaimsIdentity)this.User.Identity;
            var actualUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            citaVM.Cita.ApplicationUserId = actualUser.Value;

            await _unitWork.Cita.AgregarAsync(citaVM.Cita);
            await _unitWork.GuardarAsync();

            TempData[DS.Successfull] = "Cita creada correctamente.";

            return RedirectToAction("Index");
        }
        citaVM.EspecialistaList = _unitWork.Cita.ObtenerTodosDropdownLista("Especialista");
        TempData[DS.Error] = "Error al guardar la Cita, intente de nuevo.";
        return View(citaVM);
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        CitaVM citaVM = new CitaVM()
        {
            Cita = new Models.Cita(),
            EspecialistaList = _unitWork.Cita.ObtenerTodosDropdownLista("Especialista")
        };
        citaVM.Cita = await _unitWork.Cita.ObtenerAsync(id.GetValueOrDefault());

        if (citaVM is null) return NotFound();

        return View(citaVM);

    }

    [HttpPost]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Edit(CitaVM citaVM)
    {
        if (ModelState.IsValid)
        {
            _unitWork.Cita.Actualizar(citaVM.Cita);
            await _unitWork.GuardarAsync();

            TempData[DS.Successfull] = "Cita actualizado correctamente";

            return RedirectToAction("Index");
        }

        return View(citaVM);
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        CitaVM cita = new CitaVM();
        cita.Cita = await _unitWork.Cita.ObtenerPrimeroAsync(filter: t => t.CitaId == id, includeProperties: "Especialista");

        return View(cita);
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
            var citas = await _unitWork.Cita.ObtenerTodosAsync(
            includeProperties: "Especialista",
            orderBy: c => c.OrderByDescending(c => c.CitaId),
            isTracking: false);
            return Json(new { data = citas });
        }
        else
        {
            var citas = await _unitWork.Cita.ObtenerTodosAsync(
            includeProperties: "Especialista",
            orderBy: c => c.OrderByDescending(c => c.CitaId),
            isTracking: false,
            filter: c => c.ApplicationUserId == actualUser.Value);
            return Json(new { data = citas });
        }

    }

    /// <summary>
    /// Eliminar un registro enviado por Ajax
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Json</returns>
    [HttpPost]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Delete(int id)
    {
        var citaDB = await _unitWork.Cita.ObtenerAsync(id);

        if (citaDB is null) // Si es nulo muestre un mensaje
            return Json(new { success = false, message = "Error al eliminar citas" });

        // Si eliminó correctamente muestre mensaje
        _unitWork.Cita.Remover(citaDB);
        await _unitWork.GuardarAsync();

        return Json(new { success = true, message = "Citas eliminado correctamente" });
    }
    #endregion
}