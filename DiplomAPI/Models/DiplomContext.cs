using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace DiplomAPI.Models;

public partial class DiplomContext : DbContext
{
    public DiplomContext()
    {
    }

    public DiplomContext(DbContextOptions<DiplomContext> options, bool useInMemoryDatabase = false)
        : base(options)
    {
    }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("InMemoryDatabase");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<File>(entity =>
        {
            entity.ToTable("File");

            entity.Property(e => e.FileId)
                .ValueGeneratedOnAdd()
                .HasColumnName("FileID");
            entity.Property(e => e.Iduser).HasColumnName("IDUser");

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.Files)
                .HasForeignKey(d => d.Iduser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_File_Users");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.Property(e => e.GroupId)
                .ValueGeneratedNever()
                .HasColumnName("GroupID");

            entity.HasOne(d => d.GroupTeacherNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.GroupTeacher)
                .HasConstraintName("FK_Groups_Users");

            entity.HasData(
                new Group { GroupId = 4437, GroupTeacher = 2 },
                new Group { GroupId = 4435, GroupTeacher = 2 }
            );
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);

            entity.HasData(
                new Role { RoleId = 1, RoleName = "Студент" },
                new Role { RoleId = 2, RoleName = "Преподаватель" }
            );
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("UserID");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("FK_Users_Role");

            entity.HasData(
                new User { UserId = 1, UserName = "User1", GroupNumber = 4437, Role = 1, Login = "login1", Password = "password" },
                new User { UserId = 2, UserName = "User2", Role = 2, Login = "login2", Password = "password" }
            );
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
