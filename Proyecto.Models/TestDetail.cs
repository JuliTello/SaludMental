using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models;

public class TestDetail
{
    public int TestDetailId { get; set; }
    public DateTime FechaRealizacion { get; set; }
    public int Resultado { get; set; }
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public int TestId { get; set; }
    public Test? Test { get; set; }

    //public int PacienteId { get; set; }
    //public Paciente? Paciente { get; set; }
}
