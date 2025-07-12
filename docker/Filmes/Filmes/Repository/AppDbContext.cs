using Filmes.Model;
using Microsoft.EntityFrameworkCore;

namespace Filmes.Repository
{

    public class AppDbContext : DbContext
    {
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Filme>()
                .HasOne(f => f.Genero)
                .WithMany(g => g.Filmes)
                .HasForeignKey(f => f.idGenero)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }


}
