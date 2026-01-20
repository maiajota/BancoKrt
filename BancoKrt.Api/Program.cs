using BancoKrt.Application.Services;
using BancoKrt.Domain.Interfaces;
using BancoKrt.Infrastructure.Data;
using BancoKrt.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews(); 

builder.Services.AddScoped<ContaRepository>();
builder.Services.AddScoped<IContaRepository>(provider =>
{
    var repository = provider.GetRequiredService<ContaRepository>();
    var memoryCache = provider.GetRequiredService<IMemoryCache>();
    return new CachedContaRepository(repository, memoryCache);
});

builder.Services.AddScoped<ContaAppService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); 

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contas}/{action=Index}/{id?}");

app.Run();
