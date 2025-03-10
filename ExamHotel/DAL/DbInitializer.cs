using System.Linq;
using ExamHotel.DAL;
using ExamHotel.Models;

namespace ExamHotel
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Добавляем тестовые данные
            if (!context.Hotels.Any())
            {
                var hotels = new[]
                {
                    new Hotel
                    {
                        Name = "Отель Люкс",
                        Rating = 4.5m,
                        Address = "ул. Центральная, 123",
                        Description = "Роскошный отель с видом на город. Идеально подходит для отдыха и деловых поездок."
                    },
                    new Hotel
                    {
                        Name = "Отель Стандарт",
                        Rating = 3.8m,
                        Address = "ул. Парковая, 45",
                        Description = "Комфортабельный отель в тихом районе. Отличное соотношение цены и качества."
                    },
                    new Hotel
                    {
                        Name = "Отель Эконом",
                        Rating = 2.9m,
                        Address = "ул. Лесная, 67",
                        Description = "Бюджетный отель для экономных путешественников. Все необходимое для комфортного проживания."
                    }
                };

                context.Hotels.AddRange(hotels);
                context.SaveChanges();
            }
        }
    }
}