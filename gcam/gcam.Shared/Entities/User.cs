using gcam.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace gcam.Shared.Entities;

public class User : IdentityUser
{
    [Display(Name = "Tipo de documento")]
    public DocumentType DocumentType { get; set; }

    [Display(Name = "Documento")]
    [MaxLength(10, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public int Document { get; set; }

    [Display(Name = "Primer nombre")]
    [MaxLength(25, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string FirstName1 { get; set; } = null!;

    [Display(Name = "Segundo nombre")]
    [MaxLength(25, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string FirstName2 { get; set; } = null!;

    [Display(Name = "Primer apellido")]
    [MaxLength(25, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string LastName1 { get; set; } = null!;

    [Display(Name = "Segundo apellido")]
    [MaxLength(25, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string LastName2 { get; set; } = null!;

    [Display(Name = "Fecha de nacimiento")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public DateTime BirthDate { get; set; }

    [Display(Name = "Género")]
    public GenderType GenderType { get; set; }

    [Display(Name = "Dirección")]
    [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Address { get; set; } = null!;

    [Display(Name = "Foto")]
    public string? Photo { get; set; }

    [Display(Name = "Tipo de usuario")]
    public UserType UserType { get; set; }

    public City? City { get; set; }

    [Display(Name = "Ciudad")]
    [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
    public int CityId { get; set; }

    [Display(Name = "Condición Laboral")]
    public EmploymentStatus EmploymentStatus { get; set; }

    [Display(Name = "Usuario")]
    public string FullName => $"{FirstName1} {FirstName2} {LastName1} {LastName2}";
}