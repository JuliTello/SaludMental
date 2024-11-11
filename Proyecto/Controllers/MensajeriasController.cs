using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Repositories.Interfaces;
using Proyecto.Utilities;
using System.Security.Claims;

namespace Proyecto.Controllers;

public class MensajeriasController : Controller
{
    private readonly IUnitWork _unitWork;
    public MensajeriasController(IUnitWork unitWork)
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

        MensajeriaVM mensajeriaVM = new MensajeriaVM()
        {
            Mensajeria = new Models.Mensajeria(),
            EspecialistaList = _unitWork.Mensajeria.ObtenerTodosDropdownLista("Especialista")
        };
        mensajeriaVM.Mensajeria.FechaEnvio = DateTime.Now;
        return View(mensajeriaVM);
    }

    [HttpPost]
    public async Task<IActionResult> Create(MensajeriaVM mensajeriaVM)
    {
        if (mensajeriaVM == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (User.Identity is ClaimsIdentity claimIdentity)
            {
                var actualUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (actualUser != null)
                {
                    mensajeriaVM.Mensajeria.ApplicationUserId = actualUser.Value;
                }
            }

            await _unitWork.Mensajeria.AgregarAsync(mensajeriaVM.Mensajeria);
            await _unitWork.GuardarAsync();

            TempData[DS.Successfull] = "Mensaje creado correctamente.";

            return RedirectToAction("Index");
        }

        mensajeriaVM.EspecialistaList = _unitWork.Mensajeria.ObtenerTodosDropdownLista("Especialista");
        TempData[DS.Error] = "Error al guardar el mensaje, intente de nuevo.";
        return View(mensajeriaVM);
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var mensajeria = await _unitWork.Mensajeria.ObtenerAsync(id.GetValueOrDefault());

        if (mensajeria is null) return NotFound();

        MensajeriaVM mensajeriaVM = new MensajeriaVM()
        {
            Mensajeria = mensajeria,
            EspecialistaList = _unitWork.Mensajeria.ObtenerTodosDropdownLista("Especialista")
        };

        return View(mensajeriaVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(MensajeriaVM mensajeriaVM)
    {
        if (ModelState.IsValid)
        {
            _unitWork.Mensajeria.Actualizar(mensajeriaVM.Mensajeria);
            await _unitWork.GuardarAsync();

            TempData[DS.Successfull] = "Mensaje actualizado correctamente";

            return RedirectToAction("Index");
        }

        return View(mensajeriaVM);
    }

    [HttpGet]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Paciente + "," + DS.Role_Especialista)]
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
            return NotFound();

        MensajeriaVM mensajeria = new MensajeriaVM();
        mensajeria.Mensajeria = await _unitWork.Mensajeria.ObtenerPrimeroAsync(filter: t => t.MensajeriaId == id, includeProperties: "Especialista");

        return View(mensajeria);
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
            var mensajerias = await _unitWork.Mensajeria.ObtenerTodosAsync(
            includeProperties: "Especialista",
            orderBy: c => c.OrderByDescending(c => c.MensajeriaId),
            isTracking: false);

            return Json(new { data = mensajerias });
        }
        else
        {
            var mensajerias = await _unitWork.Mensajeria.ObtenerTodosAsync(
            includeProperties: "Especialista",
            orderBy: c => c.OrderByDescending(c => c.MensajeriaId),
            isTracking: false,
            filter: m => m.ApplicationUserId == actualUser.Value);

            return Json(new { data = mensajerias });
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
        var mensajeriaDB = await _unitWork.Mensajeria.ObtenerAsync(id);

        if (mensajeriaDB is null)
            return Json(new { success = false, message = "Error al eliminar mensaje" });

        _unitWork.Mensajeria.Remover(mensajeriaDB);
        await _unitWork.GuardarAsync();

        return Json(new { success = true, message = "Mensaje eliminado correctamente" });
    }
    #endregion
}
