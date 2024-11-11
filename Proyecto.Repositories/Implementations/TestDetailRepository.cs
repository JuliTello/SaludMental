using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Persistence;
using Proyecto.Repositories.Interfaces;

namespace Proyecto.Repositories.Implementations;

public class TestDetailRepository: RepositoryBase<TestDetail>, ITestDetailRepository
{
    private readonly ProyectoDbContext _db;
    public TestDetailRepository(ProyectoDbContext db) : base(db)
    {
        _db = db;
    }

    public void Actualizar(TestDetail testdetail)
    {
        var testdetailDB = _db.TestsDetails.FirstOrDefault(t => t.TestDetailId == testdetail.TestDetailId);

        if (testdetailDB is not null) 
        {
            testdetailDB.Resultado = testdetail.Resultado;
            testdetailDB.FechaRealizacion = testdetail.FechaRealizacion;
            testdetailDB.ApplicationUserId = testdetail.ApplicationUserId;
            testdetailDB.TestId = testdetail.TestId;


            _db.SaveChanges();
        }
    }

    public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
    {
        //if (obj == "Paciente")
        //    return _db.Pacientes.Select(t => new SelectListItem
        //    {
        //        Text = t.UserName,
        //        Value = t.PacienteId.ToString()
        //    });

        if (obj == "Test")
            return _db.Tests.Select(t => new SelectListItem
            {
                Text = t.NombreTest,
                Value = t.TestId.ToString()
            });
        return null;
    }
}
