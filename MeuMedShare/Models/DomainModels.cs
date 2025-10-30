using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MeuMedShare.Models;

public class ApplicationUser : IdentityUser
{
    // Campo para distinguir tipo de perfil: Doador ou Instituicao
    [MaxLength(20)]
    public string PerfilTipo { get; set; } = string.Empty; // "Doador" ou "Instituicao"
}

public class Endereco
{
    public int Id { get; set; }
    [Required, MaxLength(150)] public string Logradouro { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string Cidade { get; set; } = string.Empty;
    [Required, MaxLength(2)] public string UF { get; set; } = string.Empty;
    [Required, MaxLength(8)] public string CEP { get; set; } = string.Empty;
}

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
}

public class Medicamento
{
    public int Id { get; set; }
    [Required, MaxLength(150)] public string Nome { get; set; } = string.Empty;
    [MaxLength(500)] public string? Descricao { get; set; }
    public DateTime DataValidade { get; set; }
    public ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();
}

public class Doacao
{
    public int Id { get; set; }
    public int DoadorId { get; set; }
    public Doador? Doador { get; set; }
    public int? InstituicaoId { get; set; }
    public Instituicao? Instituicao { get; set; }
    public int MedicamentoId { get; set; }
    public Medicamento? Medicamento { get; set; }
    public int Quantidade { get; set; }
    public DateTime DataDoacao { get; set; } = DateTime.UtcNow;
}
