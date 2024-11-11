using Proyecto.Models;


namespace Proyecto.Repositories.Interfaces;

public interface ITestRepository: IRepositoryBase<Test>
{
    void Actualizar(Test test);
}
