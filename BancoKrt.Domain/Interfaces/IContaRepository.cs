using BancoKrt.Domain.Entities;

namespace BancoKrt.Domain.Interfaces;

public interface IContaRepository
{
    Task<Conta?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Conta>> ObterTodosAsync();
    Task AdicionarAsync(Conta conta);
    Task AtualizarAsync(Conta conta);
}
