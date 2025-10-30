using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeuMedShare.Data;
using MeuMedShare.Models.Domain;
using MeuMedShare.ViewModels.Prioridades;

namespace MeuMedShare.Controllers;

[Authorize]
public class PrioridadesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PrioridadesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        if (!EhInstituicao(user)) return Forbid();
        var inst = await _context.Instituicoes.Include(i=>i.Prioridades).FirstOrDefaultAsync(i=>i.UserId == user.Id);
        if (inst == null) return RedirectToAction("Create","Perfis", new { tipo = "Instituicao"});
        return View(inst.Prioridades.OrderByDescending(p=>p.CriadoEm));
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        if (!EhInstituicao(user)) return Forbid();
        return View(new PrioridadeCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PrioridadeCreateViewModel vm)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        if (!EhInstituicao(user)) return Forbid();
        if (!ModelState.IsValid) return View(vm);
        var inst = await _context.Instituicoes.FirstOrDefaultAsync(i=>i.UserId == user.Id);
        if (inst == null) return RedirectToAction("Create","Perfis", new { tipo = "Instituicao"});
        var prioridade = new PrioridadeMedicamento
        {
            InstituicaoId = inst.Id,
            NomeMedicamento = vm.NomeMedicamento,
            Observacao = vm.Observacao,
            Nivel = vm.Nivel,
            QuantidadeNecessaria = vm.QuantidadeNecessaria
        };
        _context.PrioridadesMedicamentos.Add(prioridade);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        if (!EhInstituicao(user)) return Forbid();
        var prioridade = await _context.PrioridadesMedicamentos.Include(p=>p.Instituicao).FirstOrDefaultAsync(p=>p.Id == id && p.Instituicao!.UserId == user.Id);
        if (prioridade == null) return NotFound();
        prioridade.Ativo = !prioridade.Ativo;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EhInstituicao(ApplicationUser user) => string.Equals(user.PerfilTipo, "Instituicao", StringComparison.OrdinalIgnoreCase);
}