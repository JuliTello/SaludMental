using Proyecto.Models;
using Proyecto.Persistence;
using Proyecto.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Proyecto.Repositories.Implementations;

public class MensajeriaRepository : RepositoryBase<Mensajeria>, IMensajeriaRepository
{
    private readonly ProyectoDbContext _db;

    public MensajeriaRepository(ProyectoDbContext db) : base(db)
    {
        _db = db;
    }

    public void Actualizar(Mensajeria mensajeria)
    {
        var mensajeriaDB = _db.Mensajerias.FirstOrDefault(t => t.MensajeriaId == mensajeria.MensajeriaId);

        if (mensajeriaDB is not null)
        {
            mensajeriaDB.Mensaje = mensajeria.Mensaje;
            mensajeriaDB.FechaEnvio = mensajeria.FechaEnvio;
            mensajeriaDB.ApplicationUserId = mensajeria.ApplicationUserId;
            mensajeriaDB.EspecialistaId = mensajeria.EspecialistaId;

            _db.SaveChanges();
        }
    }

    public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
    {
        if (obj == "Especialista")
            return _db.Especialistas.Where(t => t.Status == true).Select(t => new SelectListItem
            {
                Text = t.FirstName + " " + t.LastName,
                Value = t.EspecialistaId.ToString()
            });
        return null;
    }
}
