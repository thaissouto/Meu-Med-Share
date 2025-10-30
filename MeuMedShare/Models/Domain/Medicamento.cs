using System.ComponentModel.DataAnnotations;

namespace MeuMedShare.Models.Domain;

public class Medicamento
{
    public int Id { get; set; }
    [Required, MaxLength(150)] public string Nome { get; set; } = string.Empty;
    [MaxLength(500)] public string? Descricao { get; set; }
    public DateTime DataValidade { get; set; }
    // Caminho da foto do medicamento (opcional)
    [MaxLength(300)] public string? FotoPath { get; set; }
    // Caminho do arquivo da receita (opcional)
    [MaxLength(300)] public string? ReceitaPath { get; set; }
    public ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();
}