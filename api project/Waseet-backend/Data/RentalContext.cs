using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Waseet.Models;

// Context class
public class RentalContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer("Server=.;Database=Waseet;Trusted_connection=true;TrustServerCertificate=Yes");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Composite key for Rent entity
        modelBuilder.Entity<Rent>()
            .HasKey(r => new { r.StudentId, r.ApartmentId });

        // Composite key for Request entity
        modelBuilder.Entity<Request>()
            .HasKey(r => new { r.StudentId, r.ApartmentId });

        // Composite key for Images entity
        modelBuilder.Entity<Apartment_Images>()
            .HasKey(r => new { r.ImageId, r.ApartmentId });

        // Composite key for ApprovalStatus entity
        modelBuilder.Entity<ApprovalStatus>()
            .HasKey(a => new { a.ApartmentId, a.AdminId });
    }
}