using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PostalTracking.DAL.Entities
{
    public partial class PostalTrackingContext : DbContext
    {
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Package> Package { get; set; }
        public virtual DbSet<PackageTracking> PackageTracking { get; set; }
        public virtual DbSet<Status> Status { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Data Source=localhost\SQL2014;Initial Catalog=PostalTracking;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Ponumber)
                    .HasColumnName("PONumber")
                    .HasMaxLength(50);

                entity.Property(e => e.State).HasMaxLength(50);
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");

                entity.Property(e => e.SenderId).HasColumnName("SenderID");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.PackageReceiver)
                    .HasForeignKey(d => d.ReceiverId)
                    .HasConstraintName("FK_Package_Customer1");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.PackageSender)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("FK_Package_Customer");
            });

            modelBuilder.Entity<PackageTracking>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.StatusTime).HasColumnType("datetime");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.PackageTracking)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK_PackageTracking_Package");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.PackageTracking)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_PackageTracking_Status");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.StatusDescription).HasMaxLength(50);
            });
        }
    }
}
