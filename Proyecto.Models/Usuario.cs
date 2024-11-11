using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models;

public class Usuario
{
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }

    [MinLength(10, ErrorMessage = "La Contraseña debe tener mínimo 10 caracteres")]
    public string Password { get; set; }

    [Range(0, 130)]
    public int Age { get; set; }
    public string Gender { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Usuario()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
}
