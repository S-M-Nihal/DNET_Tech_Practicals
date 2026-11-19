using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Models;
using System;
using System.Reflection.Emit;

namespace SmartCampusPortal.Data
{
    public class CampusDbContext : DbContext
    {
        public CampusDbContext(DbContextOptions<CampusDbContext> options) : base(options)
        {
        }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceRequest>().HasData(
                new ServiceRequest
                {
                    RequestId = 1,
                    RequesterName = "Nihal S. M.",
                    CampusId = "23CSE045",
                    Category = "Lab Maintenance",
                    Location = "Computer Block - Lab 3 (System #14)",
                    Description = "Visual Studio 2026 SQL LocalDB instance requires service restart and SDK path update.",
                    Priority = "High",
                    Status = "In Progress",
                    CreatedAt = new DateTime(2026, 11, 19, 9, 30, 0)
                },
                new ServiceRequest
                {
                    RequestId = 2,
                    RequesterName = "Dr. R. Subramanian",
                    CampusId = "FAC-CSE-102",
                    Category = "IT Support",
                    Location = "Main Seminar Hall - 2nd Floor",
                    Description = "Smart podium HDMI controller and wireless microphone receiver not syncing with projector.",
                    Priority = "Critical",
                    Status = "Pending",
                    CreatedAt = new DateTime(2026, 11, 19, 10, 15, 0)
                },
                new ServiceRequest
                {
                    RequestId = 3,
                    RequesterName = "Arjun Reddy",
                    CampusId = "23ECE019",
                    Category = "Library",
                    Location = "Central Digital Library - E-Zone",
                    Description = "IEEE Xplore institutional proxy authentication failing on terminal workstations 5 to 8.",
                    Priority = "Medium",
                    Status = "Resolved",
                    CreatedAt = new DateTime(2026, 11, 18, 16, 0, 0)
                },
                new ServiceRequest
                {
                    RequestId = 4,
                    RequesterName = "Vikrant Varma",
                    CampusId = "24MECH088",
                    Category = "Hostel & Campus Wi-Fi",
                    Location = "APJ Abdul Kalam Hostel - Wing B (3rd Floor)",
                    Description = "Wi-Fi Access Point AP-B302 dropping packets intermittently during evening study hours.",
                    Priority = "High",
                    Status = "Pending",
                    CreatedAt = new DateTime(2026, 11, 19, 8, 45, 0)
                }
            );
        }
    }
}