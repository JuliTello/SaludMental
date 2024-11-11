using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Persistence;
using Proyecto.Repositories.Interfaces;


namespace Proyecto.Repositories.Implementations;

public class TestRepository: RepositoryBase<Test>, ITestRepository
{
    private readonly ProyectoDbContext _db;
    public TestRepository(ProyectoDbContext db) : base(db)
    {
        _db = db;
    }

    public void Actualizar(Test test)
    {
        var testDB = _db.Tests.FirstOrDefault(t => t.TestId == test.TestId);

        if (testDB is not null) 
        {
            testDB.NombreTest = test.NombreTest;
            testDB.Descripcion = test.Descripcion;
            testDB.Pregunta1 = test.Pregunta1;
            testDB.Pregunta2 = test.Pregunta2;
            testDB.Pregunta3 = test.Pregunta3;
            testDB.Pregunta4 = test.Pregunta4;
            testDB.Pregunta5 = test.Pregunta5;

            _db.SaveChanges();
        }
    }
}
