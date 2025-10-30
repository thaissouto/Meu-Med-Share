using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MeuMedShare.Models.Domain;

namespace MeuMedShare.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Doador> Doadores => Set<Doador>();
    public DbSet<Instituicao> Instituicoes => Set<Instituicao>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();
    public DbSet<Medicamento> Medicamentos => Set<Medicamento>();
    public DbSet<Doacao> Doacoes => Set<Doacao>();
    public DbSet<PrioridadeMedicamento> PrioridadesMedicamentos => Set<PrioridadeMedicamento>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Configurações específicas
        builder.Entity<Doador>()
            .HasOne(d => d.Endereco)
            .WithMany()
            .HasForeignKey(d => d.EnderecoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Instituicao>()
            .HasOne(i => i.Endereco)
            .WithMany()
            .HasForeignKey(i => i.EnderecoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Doacao>()
            .HasOne(d => d.Doador)
            .WithMany(d => d.Doacoes)
            .HasForeignKey(d => d.DoadorId);

        builder.Entity<Doacao>()
            .HasOne(d => d.Instituicao)
            .WithMany(i => i.Doacoes)
            .HasForeignKey(d => d.InstituicaoId);

        builder.Entity<Doacao>()
            .HasOne(d => d.Medicamento)
            .WithMany(m => m.Doacoes)
            .HasForeignKey(d => d.MedicamentoId);

        builder.Entity<PrioridadeMedicamento>()
            .HasOne(p => p.Instituicao)
            .WithMany(i => i.Prioridades)
            .HasForeignKey(p => p.InstituicaoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}