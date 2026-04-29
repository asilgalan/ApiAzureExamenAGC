using ApiAzureExamenAGC.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAzureExamenAGC.Data
{
    public class ContextCubo : DbContext
    {

        public ContextCubo(DbContextOptions<ContextCubo> options) : base(options)
        {

        }

        public DbSet<Cubo> cubos {get;set;}
        public DbSet<Usuario> usuarios {get;set;}
        public DbSet<Compra> compras { get; set; }
    }
}
