using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CareConnect.Model.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Code> Codes { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("CARECONNECT")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008951");

            entity.ToTable("BOOKINGS");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.BookingDate)
                .HasColumnType("DATE")
                .HasColumnName("BOOKING_DATE");
            entity.Property(e => e.IdService)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ID_SERVICE");
            entity.Property(e => e.IdUser)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ID_USER");
            entity.Property(e => e.TotalAmmount)
                .HasColumnType("NUMBER(8,2)")
                .HasColumnName("TOTAL_AMMOUNT");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdService)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008953");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008952");
        });

        modelBuilder.Entity<Code>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008962");

            entity.ToTable("CODES");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.DateCreated)
                .HasColumnType("DATE")
                .HasColumnName("DATE_CREATED");
            entity.Property(e => e.IdUser)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ID_USER");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Codes)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("SYS_C008963");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008959");

            entity.ToTable("PAYMENTS");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdBooking)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ID_BOOKING");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("DATE")
                .HasColumnName("PAYMENT_DATE");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PAYMENT_TYPE");
            entity.Property(e => e.PaymentValue)
                .HasColumnType("NUMBER(8,2)")
                .HasColumnName("PAYMENT_VALUE");

            entity.HasOne(d => d.IdBookingNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdBooking)
                .HasConstraintName("SYS_C008960");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008947");

            entity.ToTable("SERVICES");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.EstTimeMinutes)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("EST_TIME_MINUTES");
            entity.Property(e => e.IdDoctor)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ID_DOCTOR");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(8,2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.TimeEnd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TIME_END");
            entity.Property(e => e.TimeStart)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TIME_START");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008948");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008940");

            entity.ToTable("USERS");

            entity.HasIndex(e => e.Email, "SYS_C008941").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.UserRole)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
