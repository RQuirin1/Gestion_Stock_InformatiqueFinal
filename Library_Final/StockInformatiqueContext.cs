using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Library_Final;

public partial class StockInformatiqueContext : DbContext
{
    public StockInformatiqueContext()
    {
    }

    public StockInformatiqueContext(DbContextOptions<StockInformatiqueContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Materiel> Materiels { get; set; }

    public virtual DbSet<Origine> Origines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("data source=stock_informatique.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Materiel>(entity =>
        {
            entity.ToTable("Materiel");

            entity.Property(e => e.Caracteristique).HasColumnType("TEXT (50)");
            entity.Property(e => e.Commentaire).HasColumnType("TEXT (50)");
            entity.Property(e => e.Destination).HasColumnType("TEXT (50)");
            entity.Property(e => e.Etat).HasColumnType("TEXT (50)");
            entity.Property(e => e.Marque).HasColumnType("TEXT (50)");
            entity.Property(e => e.Modele).HasColumnType("TEXT (50)");
            entity.Property(e => e.Produit).HasColumnType("TEXT (50)");
            entity.Property(e => e.Rangement).HasColumnType("TEXT (50)");
            entity.Property(e => e.Type).HasColumnType("TEXT (50)");
        });

        modelBuilder.Entity<Origine>(entity =>
        {
            entity.ToTable("Origine");

            entity.HasIndex(e => e.IdProduit, "IX_Origine_Id_Produit");

            entity.Property(e => e.DateAchat)
                .HasColumnType("TEXT (50)")
                .HasColumnName("Date_Achat");
            entity.Property(e => e.Fournisseur).HasColumnType("TEXT (50)");
            entity.Property(e => e.IdProduit).HasColumnName("Id_Produit");
            entity.Property(e => e.Observation).HasColumnType("TEXT (50)");
            entity.Property(e => e.PrixHt)
                .HasColumnType("NUMERIC")
                .HasColumnName("Prix_HT");
            entity.Property(e => e.PrixTtc)
                .HasColumnType("NUMERIC")
                .HasColumnName("Prix_TTC");

            entity.HasOne(d => d.IdProduitNavigation).WithMany(p => p.Origines)
                .HasForeignKey(d => d.IdProduit)
                .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
