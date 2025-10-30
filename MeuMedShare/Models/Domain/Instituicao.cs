using System.ComponentModel.DataAnnotations;

namespace MeuMedShare.Models.Domain;

public class Instituicao
{
    public int Id { get; set; }
    [Required, MaxLength(150)] public string RazaoSocial { get; set; } = string.Empty;
    [Required, MaxLength(14)] public string CNPJ { get; set; } = string.Empty;
    public int EnderecoId { get; set; }
    public Endereco? Endereco { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();
    public ICollection<PrioridadeMedicamento> Prioridades { get; set; } = new List<PrioridadeMedicamento>();
}