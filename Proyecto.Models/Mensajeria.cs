using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models;

public class Mensajeria
{
    public int MensajeriaId { get; set; }
    public string Mensaje {  get; set; }
    public DateTime FechaEnvio { get; set; }
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public int EspecialistaId { get; set; }
    public Especialista? Especialista { get; set; }

    //public int PacienteId { get; set; }
    //public Paciente? Paciente { get; set; }
}
