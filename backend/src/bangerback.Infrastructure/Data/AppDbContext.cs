using System;
using System.Collections.Generic;
using Backend.bangerback.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.bangerback.Infrastructure.Data
{ 
public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Recommendation> Recommendations { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSongFavorite> UserSongFavorites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=FinalAppTemplateDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SongId }).HasName("pk_recommendation");

            entity.Property(e => e.GeneratedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Song).WithMany(p => p.Recommendations).HasConstraintName("FK__Recommend__song___4AB81AF0");

            entity.HasOne(d => d.User).WithMany(p => p.Recommendations).HasConstraintName("FK__Recommend__user___49C3F6B7");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Sessions__69B13FDC7CDBD6DB");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Sessions__user_i__3E52440B");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.SongId).HasName("PK__Songs__A535AE1CCE8D58AD");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370F0AD01AAF");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserSongFavorite>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SongId }).HasName("pk_user_song_favorite");

            entity.Property(e => e.AddedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Song).WithMany(p => p.UserSongFavorites).HasConstraintName("FK__UserSongF__song___45F365D3");

            entity.HasOne(d => d.User).WithMany(p => p.UserSongFavorites).HasConstraintName("FK__UserSongF__user___44FF419A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
}