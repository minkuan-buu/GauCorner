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

    public virtual DbSet<Donate> Donates { get; set; }

    public virtual DbSet<StreamConfig> StreamConfigs { get; set; }

    public virtual DbSet<StreamConfigType> StreamConfigTypes { get; set; }

    public virtual DbSet<Uiconfig> Uiconfigs { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Donate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Donate__3214EC07C9229887");

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
                .HasConstraintName("FK__Donate__UserId__440B1D61");
        });

        modelBuilder.Entity<StreamConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StreamCo__3214EC077A82F47D");

            entity.ToTable("StreamConfig");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Value)
                .HasMaxLength(400)
                .IsUnicode(false);

            entity.HasOne(d => d.StreamConfigType).WithMany(p => p.StreamConfigs)
                .HasForeignKey(d => d.StreamConfigTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StreamCon__Strea__44FF419A");

            entity.HasOne(d => d.User).WithMany(p => p.StreamConfigs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StreamCon__UserI__4316F928");
        });

        modelBuilder.Entity<StreamConfigType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StreamCo__3214EC07291D9300");

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
            entity.HasKey(e => e.Id).HasName("PK__UIConfig__3214EC07601EDBE9");

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
                .HasConstraintName("FK__UIConfig__Create__4222D4EF");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAcco__3214EC07E1C71908");

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
            entity.HasKey(e => e.Id).HasName("PK__UserToke__3214EC07A901E97A");

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
                .HasConstraintName("FK__UserToken__UserI__412EB0B6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
