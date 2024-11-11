using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto.Models;
using Proyecto.Persistence;
using Proyecto.Repositories.Interfaces;

namespace Proyecto.Repositories.Implementations;

public class CitaRepository: RepositoryBase<Cita>, ICitaRepository
{
    private readonly ProyectoDbContext _db;
    public CitaRepository(ProyectoDbContext db) : base(db)
    {
        _db = db;
    }

    public void Actualizar(Cita cita)
    {
        var citaDB = _db.Citas.FirstOrDefault(t => t.CitaId == cita.CitaId);

        if (citaDB is not null) 
        {
            citaDB.SessionDate = cita.SessionDate;
            citaDB.ApplicationUserId = cita.ApplicationUserId;
            citaDB.EspecialistaId = cita.EspecialistaId;

            _db.SaveChanges();
        }
    }

    public void ObtenerTodosAsync(string v1, Func<IQueryable<Cita>, IOrderedQueryable<Cita>> func, bool v2)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
    {
        //if (obj == "Paciente")
        //    return _db.Pacientes.Select(t => new SelectListItem
        //    {
        //        Text = t.UserName,
        //        Value = t.PacienteId.ToString()
        //    });

        if (obj == "Especialista")
            return _db.Especialistas.Where(t => t.Status == true).Select(t => new SelectListItem
            {
                Text = t.FirstName + " " + t.LastName,
                Value = t.EspecialistaId.ToString()
            });
        return null;
    }
}
