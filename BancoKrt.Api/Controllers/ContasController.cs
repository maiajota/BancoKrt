using BancoKrt.Application.DTOs;
using BancoKrt.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BancoKrt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContasController(ContaAppService service) : ControllerBase
{
    private readonly ContaAppService _service = service;

    /// <summary>
    /// Cria uma nova conta bancária e dispara os processos de análise.
    /// </summary>
    /// <param name="request">Dados para abertura da conta (Nome e CPF).</param>
    /// <returns>Os dados da conta recém-criada.</returns>
    /// <response code="201">Conta criada com sucesso.</response>
    /// <response code="400">Dados inválidos enviados na requisição.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContaDto>> Criar([FromBody] CriarContaRequest request)
    {
        var conta = await _service.CriarContaAsync(request.Nome, request.Cpf);

        return CreatedAtAction(nameof(ObterPorId), new { id = conta.Id }, conta);
    }

    /// <summary>
    /// Obtém os detalhes de uma conta específica pelo ID.
    /// </summary>
    /// <param name="id">ID da conta a ser buscada.</param>
    /// <returns>Dados da conta encontrada.</returns>
    /// <response code="200">Retorna a conta solicitada.</response>
    /// <response code="404">Conta não encontrada para o ID informado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContaDto>> ObterPorId(Guid id)
    {
        var conta = await _service.ObterPorIdAsync(id);

        if (conta == null) return NotFound();

        return Ok(conta);
    }

    /// <summary>
    /// Atualiza os dados cadastrais e o status de uma conta.
    /// </summary>
    /// <param name="id">ID da conta a ser alterada.</param>
    /// <param name="request">Novos dados para Nome e Status Ativo.</param>
    /// <response code="204">Conta atualizada com sucesso.</response>
    /// <response code="404">Conta não encontrada.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarContaRequest request)
    {
        var contaAtualizada = await _service.AtualizarAsync(id, request.Nome);

        if (contaAtualizada == null) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Realiza o soft delete de uma conta.
    /// </summary>
    /// <param name="id">ID da conta a ser deletada.</param>
    /// <response code="204">Conta removida com sucesso.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await _service.DeletarAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Reativa uma conta que foi desativada.
    /// </summary>
    /// <param name="id">ID da conta.</param>
    /// <response code="204">Conta reativada com sucesso.</response>
    /// <response code="404">Conta não encontrada.</response>
    [HttpPatch("{id}/reativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reativar(Guid id)
    {
        var sucesso = await _service.ReativarAsync(id);

        if (!sucesso) return NotFound();

        return NoContent();
    }
}

/// <summary> Objeto para criação de conta </summary>
public record CriarContaRequest(string Nome, string Cpf);

/// <summary> Objeto para atualização de conta </summary>
public record AtualizarContaRequest(string Nome);
