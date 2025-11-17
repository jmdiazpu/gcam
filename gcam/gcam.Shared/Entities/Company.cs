using System.ComponentModel.DataAnnotations;

namespace gcam.Shared.Entities;

public class Company
{
    public int Id { get; set; }

    [Display(Name = "Empresa")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Name { get; set; } = null!;

    [Display(Name = "Dirección")]
    [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Address { get; set; } = null!;

    [Display(Name = "Ciudad")]
    [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
    public int CityId { get; set; }

    public City? City { get; set; }

    public ICollection<CompanyContact>? CompanyContacts { get; set; }

    public int CompanyContactsNumber => CompanyContacts == null ? 0 : CompanyContacts.Count;
}