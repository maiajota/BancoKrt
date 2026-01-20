using BancoKrt.Domain.Entities;
using BancoKrt.Domain.Interfaces;
using BancoKrt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BancoKrt.Infrastructure.Repositories;

public class ContaRepository(AppDbContext context) : IContaRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Conta?> ObterPorIdAsync(Guid id)
    {
        return await _context.Contas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AdicionarAsync(Conta conta)
    {
        await _context.Contas.AddAsync(conta);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Conta conta)
    {
        _context.Contas.Update(conta);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(Guid id)
    {
        var conta = await _context.Contas.FindAsync(id);

        if (conta != null)
        {
            _context.Contas.Remove(conta);
            await _context.SaveChangesAsync();
        }
    }
}
