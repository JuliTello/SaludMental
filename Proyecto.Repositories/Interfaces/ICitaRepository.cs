using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto.Models;

namespace Proyecto.Repositories.Interfaces;

public interface ICitaRepository: IRepositoryBase<Cita>
{
    void Actualizar(Cita cita);
    void ObtenerTodosAsync(string v1, Func<IQueryable<Cita>, IOrderedQueryable<Cita>> func, bool v2);
    IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);
}
