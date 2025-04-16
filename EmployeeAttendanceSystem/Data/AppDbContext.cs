using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendanceSystem.Data;

public class AppDbContext : IdentityDbContext<Employee>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Attendance> Attendances { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Attendance>()
            .HasIndex(a => new { a.EmployeeId, a.Date })
            .IsUnique();
    }
}