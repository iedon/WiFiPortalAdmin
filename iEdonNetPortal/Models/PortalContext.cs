using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace iEdonNetPortal.Models
{
    public partial class PortalContext : DbContext
    {
        public PortalContext()
        {
        }

        public PortalContext(DbContextOptions<PortalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<AuthMacs> AuthMacs { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
               //optionsBuilder.UseMySql("");
            }*/
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.ToTable("accounts");

                entity.HasIndex(e => e.User)
                    .HasName("idx_user")
                    .IsUnique();

                entity.Property(e => e.Uid).HasColumnName("uid");

                entity.Property(e => e.Online)
                    .IsRequired()
                    .HasColumnName("online")
                    .HasColumnType("tinyint unsigned");

                entity.Property(e => e.Credit)
                    .IsRequired()
                    .HasColumnName("credit")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Lastip)
                    .HasColumnName("lastip")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.Lastlogin)
                    .HasColumnName("lastlogin")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasColumnName("user")
                    .HasColumnType("varchar(64)");
            });

            modelBuilder.Entity<AuthMacs>(entity =>
            {
                entity.ToTable("auth_macs");

                entity.HasIndex(e => e.Uid)
                    .HasName("auth_macs_uid_fk");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Traffic).HasColumnName("traffic");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Mac)
                    .IsRequired()
                    .HasColumnName("mac")
                    .HasColumnType("char(17)");

                entity.Property(e => e.Uid).HasColumnName("uid");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.AuthMacs)
                    .HasForeignKey(d => d.Uid)
                    .HasConstraintName("auth_macs_uid_fk");
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.ToTable("logs");

                entity.HasIndex(e => e.Uid)
                    .HasName("uid_fk");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action).HasColumnName("action");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Log)
                    .HasColumnName("log")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Uid).HasColumnName("uid");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.Uid)
                    .HasConstraintName("uid_fk");
            });
        }
    }
}
