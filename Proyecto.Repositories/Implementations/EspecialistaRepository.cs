using Proyecto.Models;
using Proyecto.Persistence;
using Proyecto.Repositories.Interfaces;

namespace Proyecto.Repositories.Implementations;

public class EspecialistaRepository: RepositoryBase<Especialista>, IEspecialistaRepository
{
    private readonly ProyectoDbContext _db;
    public EspecialistaRepository(ProyectoDbContext db) : base(db)
    {
        _db = db;
    }

    public void Actualizar(Especialista especialista)
    {
        var especialistaDB = _db.Especialistas.FirstOrDefault(t => t.EspecialistaId == especialista.EspecialistaId);

        if (especialistaDB is not null) 
        {
            especialistaDB.UserName = especialista.UserName;
            especialistaDB.PhoneNumber = especialista.PhoneNumber;
            especialistaDB.Email = especialista.Email;
            especialistaDB.Password = especialista.Password;
            especialistaDB.Age = especialista.Age;
            especialistaDB.Gender = especialista.Gender;
            especialistaDB.UpdatedAt = DateTime.Now;

            especialistaDB.FirstName = especialista.FirstName;
            especialistaDB.LastName = especialista.LastName;
            especialistaDB.CodigoColegiatura = especialista.CodigoColegiatura;
            especialistaDB.Info = especialista.Info;
            especialistaDB.Status = especialista.Status;

            _db.SaveChanges();
        }
    }
}
