using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeuMedShare.Data;
using MeuMedShare.Models.Domain;
using MeuMedShare.ViewModels.Perfis;

namespace MeuMedShare.Controllers;

[Authorize]
public class PerfisController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PerfisController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Create(string tipo)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        if (!string.Equals(tipo, user.PerfilTipo, StringComparison.OrdinalIgnoreCase))
        {
            // Ajusta perfil do usuário caso tenha sido alterado
            user.PerfilTipo = tipo;
            await _userManager.UpdateAsync(user);
        }
        var vm = new PerfilCreateViewModel { Tipo = tipo };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PerfilCreateViewModel vm)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        if (!ModelState.IsValid) return View(vm);

        if (string.Equals(vm.Tipo, "Doador", StringComparison.OrdinalIgnoreCase))
        {
            if (!await _context.Doadores.AnyAsync(d=>d.UserId == user.Id))
            {
                var end = new Endereco { Logradouro = vm.Logradouro!, Cidade = vm.Cidade!, UF = vm.UF!, CEP = vm.CEP! };
                _context.Enderecos.Add(end);
                await _context.SaveChangesAsync();
                _context.Doadores.Add(new Doador { Nome = vm.NomeOuRazao!, CPF = vm.CpfOuCnpj!, EnderecoId = end.Id, UserId = user.Id });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index","Doacoes");
        }
        else if (string.Equals(vm.Tipo, "Instituicao", StringComparison.OrdinalIgnoreCase))
        {
            if (!await _context.Instituicoes.AnyAsync(i=>i.UserId == user.Id))
            {
                var end = new Endereco { Logradouro = vm.Logradouro!, Cidade = vm.Cidade!, UF = vm.UF!, CEP = vm.CEP! };
                _context.Enderecos.Add(end);
                await _context.SaveChangesAsync();
                _context.Instituicoes.Add(new Instituicao { RazaoSocial = vm.NomeOuRazao!, CNPJ = vm.CpfOuCnpj!, EnderecoId = end.Id, UserId = user.Id });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index","Home");
        }

        ModelState.AddModelError(string.Empty, "Tipo inválido");
        return View(vm);
    }
}