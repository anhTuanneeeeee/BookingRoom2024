using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BOs.Entity;

public partial class BookingRoommContext : DbContext
{
    public BookingRoommContext()
    {
    }

    public BookingRoommContext(DbContextOptions<BookingRoommContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentResponse> PaymentResponses { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<SlotBooking> SlotBookings { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<WebhookResponse> WebhookResponses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnection"];
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__73951ACD96330FDC");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.BookingDate).HasColumnType("date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.SlotId).HasColumnName("SlotID");
            entity.Property(e => e.TotalFee).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("FK_Booking_Guest");

            entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_Booking_Room");

            entity.HasOne(d => d.Slot).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("FK_Booking_Slot");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branch__A1682FA53F62AC3F");

            entity.ToTable("Branch");

            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.BranchName).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Guest__3214EC274EF5819E");

            entity.ToTable("Guest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateUser).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(200);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.PhoneNumber).HasMaxLength(30);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            entity.Property(e => e.UserName).HasMaxLength(30);

            entity.HasOne(d => d.Role).WithMany(p => p.Guests)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Role");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A384A85CF9C");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.StatusPayment)
                .HasMaxLength(20)
                .HasColumnName("statusPayment");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Booking");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Guest");

            entity.HasOne(d => d.Status).WithMany(p => p.Payments)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Status");
        });

        modelBuilder.Entity<PaymentResponse>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PaymentResponse");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Signature).HasMaxLength(255);
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Price__3214EC271A913D8C");

            entity.ToTable("Price");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DayOfWeek).HasMaxLength(10);
            entity.Property(e => e.Price1)
                .HasMaxLength(100)
                .HasColumnName("Price");
            entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Prices)
                .HasForeignKey(d => d.RoomTypeId)
                .HasConstraintName("FK_Price_RoomType");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3A52124E35");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(20);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__328639192F6DCB14");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.IsAvailable).HasDefaultValueSql("((1))");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RoomName).HasMaxLength(50);
            entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

            entity.HasOne(d => d.Branch).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_Room_Branch");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomTypeId)
                .HasConstraintName("FK_Room_RoomType");

            entity.HasOne(d => d.Status).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_Room_Status");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.RoomTypeId).HasName("PK__RoomType__BCC896112925F84F");

            entity.ToTable("RoomType");

            entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.TypeName).HasMaxLength(50);
            entity.Property(e => e.Utilities).HasMaxLength(255);
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__Slot__0A124A4FFE8DA6BB");

            entity.ToTable("Slot");

            entity.Property(e => e.SlotId).HasColumnName("SlotID");
            entity.Property(e => e.EndTime).HasMaxLength(20);
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.StartTime).HasMaxLength(20);

            entity.HasOne(d => d.Room).WithMany(p => p.Slots)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Slot_Room");

            entity.HasOne(d => d.Status).WithMany(p => p.Slots)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_Slot_Status");
        });

        modelBuilder.Entity<SlotBooking>(entity =>
        {
            entity.HasKey(e => e.SlotBookingId).HasName("PK__SlotBook__F7BA695A25385901");

            entity.ToTable("SlotBooking");

            entity.Property(e => e.SlotBookingId).HasColumnName("SlotBookingID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.SlotId).HasColumnName("SlotID");

            entity.HasOne(d => d.Booking).WithMany(p => p.SlotBookings)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SlotBooking_Booking");

            entity.HasOne(d => d.Slot).WithMany(p => p.SlotBookings)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SlotBooking_Slot");

            entity.HasOne(d => d.Status).WithMany(p => p.SlotBookings)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SlotBooking_Status");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Status__3A15923E145A830A");

            entity.ToTable("Status");

            entity.Property(e => e.StatusName).HasMaxLength(100);
        });

        modelBuilder.Entity<WebhookResponse>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("WebhookResponse");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Desc).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
