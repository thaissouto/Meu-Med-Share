using System.ComponentModel.DataAnnotations;

namespace MeuMedShare.ViewModels.Perfis;

public class PerfilCreateViewModel
{
    [Required]
    public string Tipo { get; set; } = string.Empty; // Doador ou Instituicao

    [Required, MaxLength(150)]
    public string? NomeOuRazao { get; set; }

    [Required, MaxLength(14)]
    public string? CpfOuCnpj { get; set; }

    [Required, MaxLength(150)]
    public string? Logradouro { get; set; }

    [Required, MaxLength(50)]
    public string? Cidade { get; set; }

    [Required, MaxLength(2)]
    public string? UF { get; set; }

    [Required, MaxLength(8)]
    public string? CEP { get; set; }
}