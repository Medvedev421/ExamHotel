using System;
using System.Linq;
using ExamHotel.DAL;
using ExamHotel.Models;
using System.Collections.Generic;

namespace ExamHotel
{
   public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();
        
        context.Hotels.RemoveRange(context.Hotels);
        context.Rooms.RemoveRange(context.Rooms);
        context.RoomTypes.RemoveRange(context.RoomTypes);
        context.SaveChanges();

        // Проверяем, есть ли уже данные в базе
        if (!context.Hotels.Any())
        {
            // Создаем типы комнат
            var roomTypes = new[]
            {
                new RoomType { TypeName = "Эконом", Price = 1000 },
                new RoomType { TypeName = "Стандарт", Price = 2000 },
                new RoomType { TypeName = "Люкс", Price = 5000 }
            };
            context.RoomTypes.AddRange(roomTypes);
            context.SaveChanges();
            Console.WriteLine("RoomTypes saved."); // Отладочный вывод

            // Создаем отели с комнатами
            var hotels = new[]
            {
                new Hotel
                {
                    Name = "Отель Люкс",
                    Rating = 4.5m,
                    Address = "ул. Центральная, 123",
                    Description = "Роскошный отель с видом на город.",
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "101", RoomType = roomTypes[0] }, // Эконом
                        new Room { RoomNumber = "102", RoomType = roomTypes[1] }, // Стандарт
                        new Room { RoomNumber = "103", RoomType = roomTypes[2] }  // Люкс
                    }
                },
                new Hotel
                {
                    Name = "Отель Стандарт",
                    Rating = 3.8m,
                    Address = "ул. Парковая, 45",
                    Description = "Комфортабельный отель в тихом районе.",
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "201", RoomType = roomTypes[0] }, // Эконом
                        new Room { RoomNumber = "202", RoomType = roomTypes[1] }, // Стандарт
                        new Room { RoomNumber = "203", RoomType = roomTypes[2] }  // Люкс
                    }
                },
                new Hotel
                {
                    Name = "Отель Эконом",
                    Rating = 2.9m,
                    Address = "ул. Лесная, 67",
                    Description = "Бюджетный отель для экономных путешественников.",
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "301", RoomType = roomTypes[0] }, // Эконом
                        new Room { RoomNumber = "302", RoomType = roomTypes[1] }, // Стандарт
                        new Room { RoomNumber = "303", RoomType = roomTypes[2] }  // Люкс
                    }
                }
            };

            context.Hotels.AddRange(hotels);
            context.SaveChanges();
            Console.WriteLine("Hotels and Rooms saved."); // Отладочный вывод
        }
    }
}
}