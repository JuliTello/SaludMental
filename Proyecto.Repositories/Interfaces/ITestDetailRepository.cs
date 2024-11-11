using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto.Models;


namespace Proyecto.Repositories.Interfaces;

public interface ITestDetailRepository: IRepositoryBase<TestDetail>
{
    void Actualizar(TestDetail testdetail);

    IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);
}
