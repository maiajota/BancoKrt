using BancoKrt.Application.Services;
using BancoKrt.Domain.Interfaces;
using BancoKrt.Infrastructure.Data;
using BancoKrt.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BancoKrt API",
        Version = "v1",
        Description = "API de Gestão de Contas KRT"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
