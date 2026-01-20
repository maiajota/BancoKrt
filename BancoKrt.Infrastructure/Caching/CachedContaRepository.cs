using BancoKrt.Domain.Entities;
using BancoKrt.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BancoKrt.Infrastructure.Repositories;

public class CachedContaRepository(IContaRepository inner, IMemoryCache cache) : IContaRepository
{
    private readonly IContaRepository _inner = inner;
    private readonly IMemoryCache _cache = cache;

    public async Task<Conta?> ObterPorIdAsync(Guid id)
    {
        string key = $"conta-{id}";

        return await _cache.GetOrCreateAsync(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            return _inner.ObterPorIdAsync(id);
        });
    }

    public async Task<IEnumerable<Conta>> ObterTodosAsync()
    {
        return await _inner.ObterTodosAsync();
    }

    public async Task AdicionarAsync(Conta conta)
    {
        await _inner.AdicionarAsync(conta);
    }

    public async Task AtualizarAsync(Conta conta)
    {
        _cache.Remove($"conta-{conta.Id}");

        await _inner.AtualizarAsync(conta);
    }
}
