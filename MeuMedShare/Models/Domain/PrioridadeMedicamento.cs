using System.ComponentModel.DataAnnotations;

namespace MeuMedShare.Models.Domain;

public enum NivelEscassez
{
    Baixa = 0,
    Media = 1,
    Alta = 2,
    Critica = 3
}

public class PrioridadeMedicamento
{
    public int Id { get; set; }
    public int InstituicaoId { get; set; }
    public Instituicao? Instituicao { get; set; }

    [Required, MaxLength(150)] public string NomeMedicamento { get; set; } = string.Empty;
    [MaxLength(500)] public string? Observacao { get; set; }
    public NivelEscassez Nivel { get; set; } = NivelEscassez.Baixa;
    public int QuantidadeNecessaria { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; } = true;
}