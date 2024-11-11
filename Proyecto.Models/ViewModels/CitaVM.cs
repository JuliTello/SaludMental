using Microsoft.AspNetCore.Mvc.Rendering;

namespace Proyecto.Models.ViewModels;

public class CitaVM
{
    public Cita Cita { get; set; }
    public IEnumerable<SelectListItem>? EspecialistaList { get; set; }
}
