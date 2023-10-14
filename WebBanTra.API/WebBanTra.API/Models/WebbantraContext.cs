using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebBanTra.API.Models;

public partial class WebbantraContext : DbContext
{
    public WebbantraContext()
    {
    }

    public WebbantraContext(DbContextOptions<WebbantraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblContact> TblContacts { get; set; }

    public virtual DbSet<TblNew> TblNews { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblProductImage> TblProductImages { get; set; }

    public virtual DbSet<TblSlider> TblSliders { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-KP2J1QN\\SQLEXPRESS;Database=webbantra;Trusted_Connection=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.ToTable("tblCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblContact>(entity =>
        {
            entity.ToTable("tblContact");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.RepliedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblNew>(entity =>
        {
            entity.ToTable("tblNew");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.ToTable("tblProduct");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CateId).HasColumnName("CateID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Cate).WithMany(p => p.TblProducts)
                .HasForeignKey(d => d.CateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tblProduct_tblCategory");
        });

        modelBuilder.Entity<TblProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblProdu__3214EC07B9F77C06");

            entity.ToTable("tblProductImages");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.TblProductImages)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tblProductImages_tblProduct");
        });

        modelBuilder.Entity<TblSlider>(entity =>
        {
            entity.ToTable("tblSlider");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.ToTable("tblUser");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
