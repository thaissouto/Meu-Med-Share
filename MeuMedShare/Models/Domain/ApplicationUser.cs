using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MeuMedShare.Models.Domain;

public class ApplicationUser : IdentityUser
{
    [MaxLength(20)]
    public string PerfilTipo { get; set; } = string.Empty; // "Doador" ou "Instituicao"
}