
using Microsoft.EntityFrameworkCore;
using Autenticacion.Models;

namespace Autenticacion.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Persona> Personas { get; set; }
}
