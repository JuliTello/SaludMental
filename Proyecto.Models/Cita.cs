
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models;

public class Cita
{
    public int CitaId { get; set; }
    [DataType(DataType.Date)]
    public DateTime SessionDate { get; set; }
    public string Estado { get; set; }
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public int EspecialistaId { get; set; }
    public Especialista? Especialista { get; set; }

    //public int PacienteId { get; set; }
    //public Paciente? Paciente { get; set; }
}
