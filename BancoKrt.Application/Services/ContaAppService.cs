using BancoKrt.Application.DTOs;
using BancoKrt.Domain.Entities;
using BancoKrt.Domain.Interfaces;

namespace BancoKrt.Application.Services;

public class ContaAppService(IContaRepository repository)
{
    private readonly IContaRepository _repository = repository;

    public async Task<ContaDto> CriarContaAsync(string nome, string cpf)
    {
        var conta = new Conta(nome, cpf);

        await _repository.AdicionarAsync(conta);

        return MapToDto(conta);
    }

    public async Task<ContaDto?> ObterPorIdAsync(Guid id)
    {
        var conta = await _repository.ObterPorIdAsync(id);

        if (conta == null) return null;

        return MapToDto(conta);
    }

    public async Task<ContaDto?> AtualizarAsync(Guid id, string nome, bool isAtivo)
    {
        var conta = await _repository.ObterPorIdAsync(id);

        if (conta == null) return null;

        conta.Atualizar(nome, isAtivo);

        await _repository.AtualizarAsync(conta);

        return MapToDto(conta);
    }

    public async Task DeletarAsync(Guid id)
    {
        await _repository.DeletarAsync(id);
    }

    private static ContaDto MapToDto(Conta conta)
    {
        return new ContaDto(
            conta.Id,
            conta.Nome,
            conta.Cpf,
            conta.IsAtivo
        );
    }
}
