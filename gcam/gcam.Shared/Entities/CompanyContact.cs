using System.ComponentModel.DataAnnotations;

namespace gcam.Shared.Entities;

public class CompanyContact
{
    public int Id { get; set; }

    [Display(Name = "Nombres y Apellidos")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Celular")]
    [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string CelularNumber { get; set; } = null!;

    [Display(Name = "Correo Electrónico")]
    [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Cargo")]
    [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Position { get; set; } = null!;

    public int CompanyId { get; set; }
    public Company? Company { get; set; }
}