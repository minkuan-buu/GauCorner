using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GauCorner.Data.Entities;

public partial class GauCornerContext : DbContext
{
    public GauCornerContext(DbContextOptions<GauCornerContext> options)
        : base(options)
    {
    }

    public GauCornerContext()
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Donate> Donates { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<ProductVariantAttachment> ProductVariantAttachments { get; set; }

    public virtual DbSet<StreamConfig> StreamConfigs { get; set; }

    public virtual DbSet<StreamConfigType> StreamConfigTypes { get; set; }

    public virtual DbSet<Uiconfig> Uiconfigs { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC073C907694");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Categorie__Paren__68487DD7");
        });

        modelBuilder.Entity<Donate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Donate__3214EC0712A16F7A");

            entity.ToTable("Donate");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Message)
                .HasMaxLength(600)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TransId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Donates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Donate__UserId__534D60F1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC078F91026D");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__693CA210");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Create__6D0D32F4");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Produc__6A30C649");
        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductS__3214EC07EB4E01B1");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Size).HasMaxLength(20);
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .HasColumnName("SKU");
            entity.Property(e => e.StockQuantity).HasDefaultValue(0);

            entity.HasOne(d => d.Variant).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.VariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSi__Varia__6C190EBB");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductT__3214EC07C0906DD7");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductV__3214EC0762B4E185");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Style).HasMaxLength(50);
            entity.Property(e => e.VariantName).HasMaxLength(200);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductVa__Produ__6B24EA82");
        });

        modelBuilder.Entity<ProductVariantAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ProductVariantAttachment_pk");

            entity.ToTable("ProductVariantAttachment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AttachmentUrl)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.ProductVariantAttachments)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductVariantAttachment_ProductVariants_Id_fk");
        });

        modelBuilder.Entity<StreamConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StreamCo__3214EC07BE14EC09");

            entity.ToTable("StreamConfig");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Value)
                .HasMaxLength(400)
                .IsUnicode(false);

            entity.HasOne(d => d.StreamConfigType).WithMany(p => p.StreamConfigs)
                .HasForeignKey(d => d.StreamConfigTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StreamCon__Strea__5441852A");

            entity.HasOne(d => d.User).WithMany(p => p.StreamConfigs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StreamCon__UserI__5535A963");
        });

        modelBuilder.Entity<StreamConfigType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StreamCo__3214EC072C22E8A1");

            entity.ToTable("StreamConfigType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AlternativeName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.DefaultValue)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Uiconfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UIConfig__3214EC0754E0038E");

            entity.ToTable("UIConfig");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BackgroundUrl)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("BackgroundURL");
            entity.Property(e => e.ColorTone)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("LogoURL");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Uiconfigs)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UIConfig__Create__5629CD9C");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAcco__3214EC075CECBD9B");

            entity.ToTable("UserAccount");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Attachment)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Path)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserToke__3214EC072980FC7D");

            entity.ToTable("UserToken");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccesToken)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserToken__UserI__571DF1D5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
