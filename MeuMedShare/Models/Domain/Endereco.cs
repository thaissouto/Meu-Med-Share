using System.ComponentModel.DataAnnotations;

namespace MeuMedShare.Models.Domain;

public class Endereco
{
    public int Id { get; set; }
    [Required, MaxLength(150)] public string Logradouro { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string Cidade { get; set; } = string.Empty;
    [Required, MaxLength(2)] public string UF { get; set; } = string.Empty;
    [Required, MaxLength(8)] public string CEP { get; set; } = string.Empty;
}