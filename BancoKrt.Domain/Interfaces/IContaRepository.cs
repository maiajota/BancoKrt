using BancoKrt.Domain.Entities;

namespace BancoKrt.Domain.Interfaces;

public interface IContaRepository
{
    Task<Conta?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Conta conta);
    Task AtualizarAsync(Conta conta);
    Task DeletarAsync(Guid id);
}
