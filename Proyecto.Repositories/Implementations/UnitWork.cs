using Proyecto.Models;
using Proyecto.Persistence;
using Proyecto.Repositories.Interfaces;

namespace Proyecto.Repositories.Implementations;

public class UnitWork : IUnitWork
{
    private readonly ProyectoDbContext _db;
    public IEspecialistaRepository Especialista { get; private set; }
    public ICitaRepository Cita { get; private set; }
    public IApplicationUserRepository ApplicationUser { get; private set; }
    public IMensajeriaRepository Mensajeria { get; private set; }
    public ITestRepository Test { get; private set; }
    public ITestDetailRepository TestDetail { get; private set; }

    public UnitWork(ProyectoDbContext db)
    {
        _db = db;
        Especialista= new EspecialistaRepository(_db);
        Cita= new CitaRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);
        Mensajeria= new MensajeriaRepository(_db);
        Test = new TestRepository(_db);
        TestDetail = new TestDetailRepository(_db);
    }
    public void Dispose()
    {
        _db.Dispose();
    }

    public async Task GuardarAsync()
    {
        await _db.SaveChangesAsync();
    }
}
