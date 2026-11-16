using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Models;

namespace StudentManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial records for testing
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    RollNo = "2026CS101",
                    FullName = "Amit Sharma",
                    Email = "amit.sharma@college.edu",
                    Department = "CSE",
                    AcademicYear = 4,
                    EnrolledDate = new DateTime(2023, 8, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Student
                {
                    Id = 2,
                    RollNo = "2026EC205",
                    FullName = "Neha Reddy",
                    Email = "neha.reddy@college.edu",
                    Department = "ECE",
                    AcademicYear = 3,
                    EnrolledDate = new DateTime(2024, 8, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}