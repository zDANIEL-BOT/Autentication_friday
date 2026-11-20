
using Microsoft.EntityFrameworkCore;
using Autenticacion.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Controladores
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// HttpClient
builder.Services.AddHttpClient();

// Sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Configuración Scalar
builder.Services.AddOptions<ScalarOptions>()
    .BindConfiguration("Scalar");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseSession();

app.MapControllers();

app.Run();