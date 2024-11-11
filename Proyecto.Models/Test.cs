using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models;

public class Test
{
    public int TestId { get; set; }
    public string NombreTest { get; set; }
    public string Descripcion { get; set; }
    public string Pregunta1 { get; set; }
    public string Pregunta2 { get; set; }
    public string Pregunta3 { get; set; }
    public string Pregunta4 { get; set; }
    public string Pregunta5 { get; set; }
    public DateTime CreatedAt { get; set; }
    public Test()
    {
        CreatedAt = DateTime.Now;
    }
}
