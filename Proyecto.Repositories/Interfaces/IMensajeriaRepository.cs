using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Repositories.Interfaces;

public interface IMensajeriaRepository : IRepositoryBase<Mensajeria>
{
    void Actualizar(Mensajeria mensajeria);

    IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);
}
