using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Database;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = CustomRoles.Admin },
            new Role { Id = 2, Name = CustomRoles.User }
        );

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.RoleId);
            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles).HasForeignKey(d => d.RoleId);
            entity.HasOne(d => d.User).WithMany(p => p.UserRoles).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.EmailAddress, "email_address_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UserId, "uid_UNIQUE").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(45)
                .HasColumnName("user_id");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(45)
                .HasColumnName("email_address");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
