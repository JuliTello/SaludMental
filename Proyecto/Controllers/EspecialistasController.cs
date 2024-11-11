using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Repositories.Interfaces;
using Proyecto.Utilities;

namespace Proyecto.Controllers;

public class EspecialistasController : Controller
{
    private readonly IUnitWork _unitWork;
    public EspecialistasController(IUnitWork unitWork)
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
    public IActionResult Create() {
        Especialista especialista = new Especialista();
        especialista.CreatedAt = DateTime.Now;
        especialista.UpdatedAt = DateTime.Now;
        especialista.Status = true;
        return View(especialista);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Especialista especialista)
    {
        if (especialista == null) { return NotFound(); }

        if (ModelState.IsValid)
        {
            especialista.CreatedAt = DateTime.Now;
            especialista.UpdatedAt = DateTime.Now;
            especialista.Status = true;

            await _unitWork.Especialista.AgregarAsync(especialista);
            await _unitWork.GuardarAsync();

            TempData["Successful"] = "Especialista creado correctamente";

            return RedirectToAction("Index");
        }
        
        return View(especialista);
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        Especialista especialista = new Especialista();
        especialista = await _unitWork.Especialista.ObtenerAsync(id.GetValueOrDefault());

        if (especialista is null) return NotFound(); 

        return View(especialista);

    }

    [HttpPost]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Edit(Especialista especialista)
    {
        if (ModelState.IsValid)
        {
            _unitWork.Especialista.Actualizar(especialista);
            await _unitWork.GuardarAsync();

            TempData["Successful"] = "Especialista actualizado correctamente";

            return RedirectToAction("Index");
        }

        return View(especialista);
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
            return NotFound();

        Especialista especialista = new Especialista();
        especialista = await _unitWork.Especialista.ObtenerPrimeroAsync(filter: t => t.EspecialistaId == id);

        if (especialista is null)
            return NotFound();

        return View(especialista);
    }

    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Tarjetas()
    {
        var especialista = await _unitWork.Especialista.ObtenerTodosAsync(
            isTracking: false,
            orderBy: t => t.OrderByDescending(t => t.EspecialistaId)
           );
        return View(especialista);
    }

    #region API
    /// <summary>
    /// Listar todos los cursos registrados
    /// </summary>
    /// <returns>Json</returns>
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var especialistas = await _unitWork.Especialista.ObtenerTodosAsync(
            orderBy: c => c.OrderByDescending(c => c.EspecialistaId),
            isTracking: false);

        return Json(new { data = especialistas });
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
        var especialistaDB = await _unitWork.Especialista.ObtenerAsync(id);
        
        if (especialistaDB is null) // Si es nulo muestre un mensaje
            return Json(new { success = false, message="Error al eliminar el Especialista" });

        // Si eliminó correctamente muestre mensaje
        _unitWork.Especialista.Remover(especialistaDB);
        await _unitWork.GuardarAsync();

        return Json(new { success = true, message = "Especialista eliminado correctamente" });    
    }
    #endregion
}
