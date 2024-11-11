using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models.ViewModels;

public class MensajeriaVM
{
    public Mensajeria Mensajeria { get; set; }
   // public IEnumerable<SelectListItem>? PacienteList { get; set; }
    public IEnumerable<SelectListItem>? EspecialistaList { get; set; }
}
