using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Persistence;
using Proyecto.Repositories.Interfaces;
using Proyecto.Utilities;
using System.Security.Claims;

namespace Proyecto.Controllers;

[Authorize(Roles = DS.Role_Admin)]
public class UsersController : Controller
{
    private readonly IUnitWork _unitWork;
    private readonly ProyectoDbContext _context;
    public UsersController(IUnitWork unitWork, ProyectoDbContext context)
    {
        _unitWork = unitWork;
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    #region API para JAVASCRIPT
    /// <summary>
    /// Lista todos los usuarios existentes
    /// </summary>
    /// <returns>Json</returns>
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var claimIdentity = (ClaimsIdentity)this.User.Identity;
        var actualUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var users = await _unitWork.ApplicationUser.ObtenerTodosAsync(filter: u => u.Id != actualUser.Value);

        // Los Roles
        var userRoles = await _context.UserRoles.ToListAsync();
        var roles = await _context.Roles.ToListAsync();

        // Llenar la propiedad del Role del Modelo ApplicationUser
        foreach (var user in users)
        {
            var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
            user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
        }

        return Json(new { data = users });
    }

    [HttpPost]
    [Authorize(Roles = DS.Role_Admin)]
    public async Task<IActionResult> CambiarRol(string userId, string nuevoRol)
    {
        var user = await _unitWork.ApplicationUser.ObtenerAsync(userId);
        if (user == null)
        {
            return Json(new { success = false, message = "Usuario no encontrado." });
        }

        var currentRoles = await _context.UserRoles.Where(ur => ur.UserId == userId).ToListAsync();
        _context.UserRoles.RemoveRange(currentRoles);
        await _context.SaveChangesAsync();

        var newRoleId = _context.Roles.FirstOrDefault(r => r.Name == nuevoRol)?.Id;
        if (newRoleId == null)
        {
            return Json(new { success = false, message = "Rol no encontrado." });
        }

        _context.UserRoles.Add(new IdentityUserRole<string> { UserId = userId, RoleId = newRoleId });
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Rol cambiado correctamente." }); 
    }
    #endregion
}
