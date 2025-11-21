using InvestBoardAPI.Data;
using InvestBoardAPI.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add DbContext with SQL Server provider and lazy loading proxies
builder.Services.AddDbContext<InvestBoardDB>(
    options => options
    .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseLazyLoadingProxies());

// Adiciona segurança através de autenticação e autorização
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration.GetSection("Authentication").GetValue<String>("Authority");
        options.Audience = builder.Configuration.GetSection("Authentication").GetValue<String>("Audience");
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment(); // Apenas para ambiente de desenvolvimento!
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Usa middleware de telemetria
app.UseTelemetryMiddleware();

app.Run();
