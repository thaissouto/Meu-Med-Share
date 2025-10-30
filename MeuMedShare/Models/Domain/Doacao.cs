namespace MeuMedShare.Models.Domain;

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