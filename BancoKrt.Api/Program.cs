using BancoKrt.Application.Services;
using BancoKrt.Domain.Interfaces;
using BancoKrt.Infrastructure.Caching;
using BancoKrt.Infrastructure.Data;
using BancoKrt.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddMemoryCache();

builder.Services.AddScoped<ContaRepository>();

builder.Services.AddScoped<IContaRepository>(provider =>
{
    var repository = provider.GetRequiredService<ContaRepository>();
    var memoryCache = provider.GetRequiredService<IMemoryCache>();
    return new CachedContaRepository(repository, memoryCache);
});

builder.Services.AddScoped<ContaAppService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
