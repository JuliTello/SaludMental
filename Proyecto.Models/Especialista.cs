using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models;

public class Especialista : Usuario
{
    [Key]
    public int EspecialistaId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CodigoColegiatura { get; set; }
    public string Especialidad { get; set; }
    public string? Info {  get; set; }
    public bool Status { get; set; }
}
