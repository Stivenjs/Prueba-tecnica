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

        public DbSet<Insured> Insureds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Asegurado
            modelBuilder.Entity<Insured>(entity =>
            {
                // Configurar la tabla
                entity.ToTable("Insureds");

                // Configurar la llave primaria
                entity.HasKey(e => e.IdentificationNumber);

                // Configurar índices
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.IdentificationNumber);

                // Configurar propiedades
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50);

                entity.Property(e => e.FirstLastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SecondLastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ContactPhone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EstimatedRequestValue)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.Observations)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
