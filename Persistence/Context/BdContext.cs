using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence.Domain;

namespace Persistence.Context
{
    public partial class BdContext : DbContext
    {
        public BdContext()
        {
        }

        public BdContext(DbContextOptions<BdContext> options)
            : base(options)
        {
        }

        public virtual DbSet<seguimiento> seguimiento { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-6U9O0NL;Database=DbSeguimiento;User Id=sa;Password=123456;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<seguimiento>(entity =>
            {
                entity.Property(e => e.correlativo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.fechaemision).HasColumnType("datetime");

                entity.Property(e => e.fechaenvio).HasColumnType("datetime");

                entity.Property(e => e.importe).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.pcname).HasMaxLength(150);

                entity.Property(e => e.rucempresa)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.serie)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.sysver).HasMaxLength(10);

                entity.Property(e => e.urlservice).HasMaxLength(250);

                entity.Property(e => e.username)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
