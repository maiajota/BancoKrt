using BancoKrt.Domain.Entities;
using BancoKrt.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BancoKrt.Infrastructure.Caching;

public class CachedContaRepository(IContaRepository inner, IMemoryCache cache) : IContaRepository
{
    private readonly IContaRepository _inner = inner;
    private readonly IMemoryCache _cache = cache;

    public async Task<Conta?> ObterPorIdAsync(Guid id)
    {
        string chaveCache = $"conta-{id}";

        if (!_cache.TryGetValue(chaveCache, out Conta? conta))
        {
            conta = await _inner.ObterPorIdAsync(id);

            if (conta != null)
                _cache.Set(chaveCache, conta, TimeSpan.FromHours(24)); 
        }

        return conta;
    }

    public async Task AdicionarAsync(Conta conta) => await _inner.AdicionarAsync(conta);

    public async Task AtualizarAsync(Conta conta)
    {
        _cache.Remove($"conta-{conta.Id}");
        await _inner.AtualizarAsync(conta);
    }

    public async Task DeletarAsync(Guid id)
    {
        _cache.Remove($"conta-{id}");
        await _inner.DeletarAsync(id);
    }
}
