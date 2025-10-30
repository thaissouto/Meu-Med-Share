using System.ComponentModel.DataAnnotations;

namespace MeuMedShare.Models.Domain;

public class Doador
{
    public int Id { get; set; }
    [Required, MaxLength(120)] public string Nome { get; set; } = string.Empty;
    [Required, MaxLength(11)] public string CPF { get; set; } = string.Empty;
    public int EnderecoId { get; set; }
    public Endereco? Endereco { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();
}