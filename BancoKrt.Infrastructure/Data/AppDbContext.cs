using BancoKrt.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BancoKrt.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Conta> Contas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Conta>(entity => 
        {
            entity.ToTable("contas");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Nome)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Cpf)
                  .IsRequired()
                  .HasMaxLength(11);

            entity.Property(e => e.IsAtivo)
                  .IsRequired();
        });
    }
}
