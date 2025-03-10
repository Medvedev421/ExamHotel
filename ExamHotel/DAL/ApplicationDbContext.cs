using Microsoft.EntityFrameworkCore;
using ExamHotel.Models;

namespace ExamHotel.DAL
{
    public class ApplicationDbContext : DbContext
    {
        // Таблицы базы данных
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Passport> Passports { get; set; }

        // Настройка подключения к базе данных
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=hotelexam;Username=postgres;Password=1234");
        }
    }
}