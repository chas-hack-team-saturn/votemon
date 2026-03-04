using System;
using System.Collections.Generic;
using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Data;

public partial class VotemonDbContext : DbContext
{
    public VotemonDbContext()
    {
    }

    public VotemonDbContext(DbContextOptions<VotemonDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pokemon> Pokemons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_uca1400_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.HasKey(e => e.DexId).HasName("PRIMARY");

            entity.ToTable("Pokemon");

            entity.HasIndex(e => e.Name, "UX_Pokemon_Name").IsUnique();

            entity.Property(e => e.DexId).HasColumnType("int(11)");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_uca1400_ai_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Votes)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)");
            entity.Property(e => e.EloRating)
                .HasDefaultValueSql("'1200'")
                .HasColumnType("int(11)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
