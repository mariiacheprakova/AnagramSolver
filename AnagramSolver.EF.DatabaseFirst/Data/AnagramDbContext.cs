using System;
using System.Collections.Generic;
using AnagramSolver.EF.DatabaseFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.EF.DatabaseFirst.Data;

public partial class AnagramDbContext : DbContext
{
    public AnagramDbContext(DbContextOptions<AnagramDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<SearchLog> SearchLogs { get; set; }

    public virtual DbSet<Word> Words { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC070C9DA1AC");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SearchLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SearchLo__3214EC07A7B3428A");

            entity.ToTable("SearchLog");

            entity.Property(e => e.SearchText).HasMaxLength(100);
            entity.Property(e => e.SearchedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Words__3214EC0762C2B379");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Value).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Words)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Words__CategoryI__0B91BA14");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
