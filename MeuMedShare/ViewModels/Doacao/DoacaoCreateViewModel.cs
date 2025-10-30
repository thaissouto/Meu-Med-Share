using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MeuMedShare.Models.Domain;

namespace MeuMedShare.ViewModels.Doacao;

public class DoacaoCreateViewModel
{
    // Dados do Medicamento
    [Required, MaxLength(150)] public string NomeMedicamento { get; set; } = string.Empty;
    [MaxLength(500)] public string? Descricao { get; set; }
    [Required, DataType(DataType.Date)] public DateTime DataValidade { get; set; }
    [Required, Range(1,int.MaxValue)] public int Quantidade { get; set; }

    public IFormFile? Foto { get; set; }
    public IFormFile? Receita { get; set; }

    // Seleção da instituição
    [Required] public int InstituicaoId { get; set; }
    public List<Instituicao>? InstituicoesDisponiveis { get; set; }
}