using BancoKrt.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BancoKrt.Api.Controllers;

public class ContasController(ContaAppService service) : Controller
{
    private readonly ContaAppService _service = service;

    // GET: /Contas
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var contas = await _service.ObterTodosAsync();
        return View(contas);
    }

    // GET: /Contas/Criar
    [HttpGet]
    public IActionResult Criar()
    {
        return View();
    }

    // POST: /Contas/Criar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(string nome, string cpf)
    {
        var cpfLimpo = cpf.Replace(".", "").Replace("-", "");

        if (cpfLimpo.Length != 11)
        {
            ModelState.AddModelError("cpf", "O CPF deve ter 11 dígitos.");
            return View();
        }

        await _service.CriarContaAsync(nome, cpfLimpo);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Contas/Atualizar/{id}
    [HttpGet]
    public async Task<IActionResult> Atualizar(Guid id)
    {
        var conta = await _service.ObterPorIdAsync(id);
        if (conta == null) return NotFound();

        return View(conta);
    }

    // POST: /Contas/Atualizar/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Atualizar(Guid id, string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            ModelState.AddModelError("nome", "O nome é obrigatório.");
            return View(await _service.ObterPorIdAsync(id));
        }

        await _service.AtualizarAsync(id, nome);

        return RedirectToAction(nameof(Index));
    }

    // POST: /Contas/Desativar/id
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Desativar(Guid id)
    {
        await _service.DeletarAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // POST: /Contas/Reativar/id
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reativar(Guid id)
    {
        await _service.ReativarAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
