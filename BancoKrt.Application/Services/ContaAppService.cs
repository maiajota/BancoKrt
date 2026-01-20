using BancoKrt.Application.DTOs;
using BancoKrt.Domain.Entities;
using BancoKrt.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BancoKrt.Application.Services;

public class ContaAppService(IContaRepository repository, ILogger<ContaAppService> logger)
{
    private readonly IContaRepository _repository = repository;
    private readonly ILogger<ContaAppService> _logger = logger;

    public async Task<ContaDto> CriarContaAsync(string nome, string cpf)
    {
        try
        {
            _logger.LogInformation("Iniciando abertura de conta para o CPF: {Cpf}", cpf);

            var conta = new Conta(nome, cpf);
            await _repository.AdicionarAsync(conta);

            _logger.LogInformation("A conta para o CPF {cpf} foi criada com sucesso.", conta.Cpf);

            return new ContaDto(conta.Id, conta.Nome, conta.Cpf, conta.IsAtivo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar conta para o CPF: {Cpf}", cpf);
            throw;
        }
    }

    public async Task<ContaDto?> ObterPorIdAsync(Guid id)
    {
        _logger.LogInformation("Consultando dados da conta: {Id}", id);

        var conta = await _repository.ObterPorIdAsync(id);

        if (conta == null)
        {
            _logger.LogWarning("Conta {Id} não encontrada no banco.", id);
            return null;
        }

        return new ContaDto(conta.Id, conta.Nome, conta.Cpf, conta.IsAtivo);
    }

    public async Task<ContaDto?> AtualizarAsync(Guid id, string nome)
    {
        _logger.LogInformation("Atualizando conta {Id}. Novo Nome: {Nome}", id, nome);

        var conta = await _repository.ObterPorIdAsync(id);
        if (conta == null) return null;

        conta.Atualizar(nome);
        await _repository.AtualizarAsync(conta);

        _logger.LogInformation("Conta {Id} atualizada com sucesso.", id);
        return new ContaDto(conta.Id, conta.Nome, conta.Cpf, conta.IsAtivo);
    }

    public async Task DeletarAsync(Guid id)
    {
        _logger.LogInformation("Solicitação da remoção da conta {Id}", id);

        var conta = await _repository.ObterPorIdAsync(id);
        if (conta != null)
        {
            conta.Desativar();
            await _repository.AtualizarAsync(conta);

            _logger.LogInformation("Conta {Id} removida do sistema.", id);

        }
    }

    public async Task<bool> ReativarAsync(Guid id)
    {
        _logger.LogInformation("Solicitação de reativação da conta {Id}", id);

        var conta = await _repository.ObterPorIdAsync(id);

        if (conta == null)
        {
            _logger.LogWarning("Falha ao reativar: Conta {Id} não encontrada.", id);
            return false;
        }

        if (conta.IsAtivo)
        {
            _logger.LogInformation("Conta {Id} já se encontra ativa.", id);
            return true;
        }

        conta.Reativar();
        await _repository.AtualizarAsync(conta);

        _logger.LogInformation("Conta {Id} reativada com sucesso.", id);
        return true;
    }
}
