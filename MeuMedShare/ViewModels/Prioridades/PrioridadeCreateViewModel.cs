using System.ComponentModel.DataAnnotations;
using MeuMedShare.Models.Domain;

namespace MeuMedShare.ViewModels.Prioridades;

public class PrioridadeCreateViewModel
{
    [Required, MaxLength(150)] public string NomeMedicamento { get; set; } = string.Empty;
    [MaxLength(500)] public string? Observacao { get; set; }
    [Required] public NivelEscassez Nivel { get; set; } = NivelEscassez.Baixa;
    [Range(1,int.MaxValue)] public int QuantidadeNecessaria { get; set; } = 1;
}