using Proyecto.Models;

namespace Proyecto.Repositories.Interfaces;

public interface IEspecialistaRepository: IRepositoryBase<Especialista>
{
    void Actualizar(Especialista espesialista);
}
