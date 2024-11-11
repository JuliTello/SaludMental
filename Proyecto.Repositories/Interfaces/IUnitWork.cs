
namespace Proyecto.Repositories.Interfaces;

public interface IUnitWork: IDisposable
{
    IEspecialistaRepository Especialista { get; }
    ICitaRepository Cita { get; }
    IApplicationUserRepository ApplicationUser { get; }
    IMensajeriaRepository Mensajeria { get; }
    ITestRepository Test { get; }
    ITestDetailRepository TestDetail { get; }
    Task GuardarAsync();
}
