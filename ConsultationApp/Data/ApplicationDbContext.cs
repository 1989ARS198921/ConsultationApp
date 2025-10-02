using Microsoft.EntityFrameworkCore;
using ConsultationApp.Models;

namespace ConsultationApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Consultant> Consultants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Consultation>().HasData(
                new Consultation { Id = 1, DateTime = new DateTime(2025, 4, 5, 10, 0, 0), ClientId = 1, ConsultantId = 1, Status = "booked" },
                new Consultation { Id = 2, DateTime = new DateTime(2025, 4, 6, 11, 0, 0), ClientId = 2, ConsultantId = 1, Status = "booked" }
            );

            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, Name = "Иван Иванов", Email = "ivan@example.com" },
                new Client { Id = 2, Name = "Мария Петрова", Email = "maria@example.com" }
            );

            modelBuilder.Entity<Consultant>().HasData(
                new Consultant { Id = 1, Name = "Анна Смирнова", Specialty = "Психолог" }
            );
        }
    }
}