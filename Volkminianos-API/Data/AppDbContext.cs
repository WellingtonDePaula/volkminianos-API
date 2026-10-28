using VolkminianosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace VolkminianosAPI.Context;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
    }

    public DbSet<Usuario>? Usuarios { get; set; }
    public DbSet<Tarifa>? Tarifas { get; set; }
    public DbSet<Bairro>? Bairros { get; set; }
    public DbSet<Ponto>? Pontos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ponto>()
            .HasOne(p => p.Bairro)
            .WithMany(b => b.Pontos)
            .HasForeignKey(p => p.BairroId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tarifa>()
            .HasOne(t => t.BairroA)
            .WithMany()
            .HasForeignKey(t => t.BairroAId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tarifa>()
            .HasOne(t => t.BairroB)
            .WithMany()
            .HasForeignKey(t => t.BairroBId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices únicos: o banco passa a garantir o que os services já checam,
        // evitando duplicados mesmo com duas requisições simultâneas.
        modelBuilder.Entity<Bairro>()
            .HasIndex(b => b.Nome)
            .IsUnique();

        modelBuilder.Entity<Ponto>()
            .HasIndex(p => p.Nome)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Uma única tarifa por par de bairros (o par é guardado sempre ordenado: A <= B).
        modelBuilder.Entity<Tarifa>()
            .HasIndex(t => new { t.BairroAId, t.BairroBId })
            .IsUnique();
    }
}
