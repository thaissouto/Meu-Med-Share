using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeuMedShare.Data;
using MeuMedShare.Models.Domain;
using MeuMedShare.ViewModels.Doacao;

namespace MeuMedShare.Controllers;

[Authorize]
public class DoacoesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    private static readonly string[] FotoPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private static readonly string[] ReceitaPermitidas = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
    private const long TamanhoMaxArquivo = 5 * 1024 * 1024; // 5MB

    public DoacoesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
    {
        _context = context;
        _userManager = userManager;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        var doador = await _context.Doadores.Include(d=>d.Endereco).FirstOrDefaultAsync(d=>d.UserId == user.Id);
        if (doador == null) return RedirectToAction("Create","Perfis", new { tipo = "Doador"});
        var doacoes = await _context.Doacoes
            .Include(d=>d.Medicamento)
            .Include(d=>d.Instituicao)
            .Where(d=>d.DoadorId == doador.Id)
            .OrderByDescending(d=>d.DataDoacao)
            .ToListAsync();
        return View(doacoes);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        if (!EhDoador(user)) return Forbid();
        var doador = await _context.Doadores.FirstOrDefaultAsync(d => d.UserId == user.Id);
        if (doador == null) return RedirectToAction("Create","Perfis", new { tipo = "Doador"});
        var vm = new DoacaoCreateViewModel
        {
            DataValidade = DateTime.Today,
            InstituicoesDisponiveis = await _context.Instituicoes.AsNoTracking().ToListAsync()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DoacaoCreateViewModel vm)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        if (!EhDoador(user)) return Forbid();

        vm.InstituicoesDisponiveis = await _context.Instituicoes.AsNoTracking().ToListAsync();
        if (!ModelState.IsValid) return View(vm);

        var doador = await _context.Doadores.FirstOrDefaultAsync(d => d.UserId == user.Id);
        if (doador == null)
        {
            ModelState.AddModelError(string.Empty, "Perfil de Doador não encontrado. Complete seu cadastro.");
            return View(vm);
        }

        // Valida arquivos
        if (vm.Foto != null && !ValidarArquivo(vm.Foto, FotoPermitidas))
            ModelState.AddModelError("Foto", "Extensão ou tamanho inválido (máx 5MB, formatos: jpg, jpeg, png, gif, webp)");
        if (vm.Receita != null && !ValidarArquivo(vm.Receita, ReceitaPermitidas))
            ModelState.AddModelError("Receita", "Extensão ou tamanho inválido (máx 5MB, formatos: jpg, jpeg, png, pdf)");
        if (!ModelState.IsValid) return View(vm);

        var medicamento = new Medicamento
        {
            Nome = vm.NomeMedicamento,
            Descricao = vm.Descricao,
            DataValidade = vm.DataValidade
        };

        if (vm.Foto != null && vm.Foto.Length > 0)
            medicamento.FotoPath = await SalvarArquivo(vm.Foto, "fotos");
        if (vm.Receita != null && vm.Receita.Length > 0)
            medicamento.ReceitaPath = await SalvarArquivo(vm.Receita, "receitas");

        _context.Medicamentos.Add(medicamento);
        await _context.SaveChangesAsync();

        var doacao = new Doacao
        {
            DoadorId = doador.Id,
            InstituicaoId = vm.InstituicaoId,
            MedicamentoId = medicamento.Id,
            Quantidade = vm.Quantidade,
            DataDoacao = DateTime.UtcNow
        };

        _context.Doacoes.Add(doacao);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", new { id = doacao.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var doacao = await _context.Doacoes
            .Include(d => d.Medicamento)
            .Include(d => d.Instituicao)
            .Include(d => d.Doador)
            .FirstOrDefaultAsync(d => d.Id == id);
        if (doacao == null) return NotFound();
        return View(doacao);
    }

    private async Task<string> SalvarArquivo(IFormFile file, string pasta)
    {
        var uploadsRoot = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), pasta);
        if (!Directory.Exists(uploadsRoot)) Directory.CreateDirectory(uploadsRoot);
        var nomeArquivo = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var caminho = Path.Combine(uploadsRoot, nomeArquivo);
        using (var stream = System.IO.File.Create(caminho))
        {
            await file.CopyToAsync(stream);
        }
        return Path.Combine(pasta, nomeArquivo).Replace("\\", "/");
    }

    private bool ValidarArquivo(IFormFile? file, string[] extensoes)
    {
        if (file == null) return true;
        if (file.Length > TamanhoMaxArquivo) return false;
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        return extensoes.Contains(ext);
    }

    private bool EhDoador(ApplicationUser user) => string.Equals(user.PerfilTipo, "Doador", StringComparison.OrdinalIgnoreCase);
}