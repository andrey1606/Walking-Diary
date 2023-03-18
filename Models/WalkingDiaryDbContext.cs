using Microsoft.EntityFrameworkCore;

namespace WalkingDiary.Models;

public partial class WalkingDiaryDbContext : DbContext
{
    public WalkingDiaryDbContext() { }

    public WalkingDiaryDbContext(DbContextOptions<WalkingDiaryDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Walk> Walks { get; set; }

    public virtual DbSet<WalkPhoto> WalkPhotos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Name, "users_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Walk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("walks_pkey");

            entity.ToTable("walks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AirTemperature).HasColumnName("air_temperature");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Distance).HasColumnName("distance");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.WalkType)
                .HasMaxLength(15)
                .HasColumnName("walk_type");

            entity.HasOne(d => d.User).WithMany(p => p.Walks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("walks_user_id_fkey");
        });

        modelBuilder.Entity<WalkPhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("walk_photos_pkey");

            entity.ToTable("walk_photos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Url).HasColumnName("url");
            entity.Property(e => e.WalkId).HasColumnName("walk_id");

            entity.HasOne(d => d.Walk).WithMany(p => p.WalkPhotos)
                .HasForeignKey(d => d.WalkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("walk_photos_walk_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
