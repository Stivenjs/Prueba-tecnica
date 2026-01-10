using Microsoft.EntityFrameworkCore;
using SegurosAPI.Models;

namespace SegurosAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Asegurado> Asegurados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Asegurado
            modelBuilder.Entity<Asegurado>(entity =>
            {
                // Configurar la tabla
                entity.ToTable("Asegurados");

                // Configurar la llave primaria
                entity.HasKey(e => e.NumeroIdentificacion);

                // Configurar índices
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.NumeroIdentificacion);

                // Configurar propiedades
                entity.Property(e => e.PrimerNombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SegundoNombre)
                    .HasMaxLength(50);

                entity.Property(e => e.PrimerApellido)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SegundoApellido)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TelefonoContacto)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ValorEstimadoSolicitud)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(500);

                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
