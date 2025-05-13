using System;
using System.Collections.Generic;
using CinemaBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Data;

public partial class CinemaBookingDbContext : DbContext
{
    public CinemaBookingDbContext()
    {
    }

    public CinemaBookingDbContext(DbContextOptions<CinemaBookingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditorium> Auditoriums { get; set; }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Showtime> Showtimes { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MSI\\SQLEXPRESS;Initial Catalog=CinemaBookingDB;TrustServerCertificate=True;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.AuditoriumId).HasName("PK__Auditori__6E91B1A589462A40");

            entity.Property(e => e.AuditoriumId).HasColumnName("AuditoriumID");
            entity.Property(e => e.CinemaId).HasColumnName("CinemaID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(true);

            entity.HasOne(d => d.Cinema).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.CinemaId)
                .HasConstraintName("FK__Auditoriu__Cinem__403A8C7D");
        });

        modelBuilder.Entity<Cinema>(entity =>
        {
            entity.HasKey(e => e.CinemaId).HasName("PK__Cinemas__59C92626A5709701");

            entity.Property(e => e.CinemaId).HasColumnName("CinemaID");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(true);
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(true);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(true);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(true);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movies__4BD2943A200AEED4");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Director)
                .HasMaxLength(255)
                .IsUnicode(true);
            entity.Property(e => e.Genre)
                .HasMaxLength(100)
                .IsUnicode(true);
            entity.Property(e => e.Poster)
                .HasMaxLength(255)
                .IsUnicode(true);
            entity.Property(e => e.Rating)
                .HasMaxLength(10)
                .IsUnicode(true);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(true);
            entity.Property(e => e.TrailerUrl)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("TrailerURL");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A581D35BFAE");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(true);
            entity.Property(e => e.TicketId).HasColumnName("TicketID");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Payments)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__Payments__Ticket__5070F446");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__52C42F2FA20C85B4");

            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(true);
            entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AE7A7E48D1");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.Comment).HasColumnType("text");
            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.ReviewDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Movie).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK__Reviews__MovieID__5812160E");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__UserID__571DF1D5");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PK__Seats__311713D306EDDCE1");

            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.AuditoriumId).HasColumnName("AuditoriumID");
            entity.Property(e => e.Row)
                .HasMaxLength(1)
                .IsUnicode(true)
                .IsFixedLength();

            entity.HasOne(d => d.Auditorium).WithMany(p => p.Seats)
                .HasForeignKey(d => d.AuditoriumId)
                .HasConstraintName("FK__Seats__Auditoriu__4316F928");
        });

        modelBuilder.Entity<Showtime>(entity =>
        {
            entity.HasKey(e => e.ShowtimeId).HasName("PK__Showtime__32D31FC0AB7AFDAB");

            entity.Property(e => e.ShowtimeId).HasColumnName("ShowtimeID");
            entity.Property(e => e.AuditoriumId).HasColumnName("AuditoriumID");
            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.ShowTime1).HasColumnName("ShowTime");

            entity.HasOne(d => d.Auditorium).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.AuditoriumId)
                .HasConstraintName("FK__Showtimes__Audit__46E78A0C");

            entity.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK__Showtimes__Movie__45F365D3");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Tickets__712CC627554C7467");

            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.BookingTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.ShowtimeId).HasColumnName("ShowtimeID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .HasConstraintName("FK__Tickets__SeatID__4BAC3F29");

            entity.HasOne(d => d.Showtime).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ShowtimeId)
                .HasConstraintName("FK__Tickets__Showtim__4AB81AF0");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Tickets__UserID__4CA06362");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC546A80B1");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534C75D1001").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(true);
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(true);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(true);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(true);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(true)
                .HasDefaultValue("User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
