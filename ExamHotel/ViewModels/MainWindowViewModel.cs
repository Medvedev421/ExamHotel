using System.Collections.ObjectModel;
using ExamHotel.Models;
using ExamHotel.DAL;
using System.Linq;

namespace ExamHotel.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // Singleton instance
        private static MainWindowViewModel _instance;
        public static MainWindowViewModel Instance => _instance ??= new MainWindowViewModel();

        // Список отелей
        public ObservableCollection<Hotel> Hotels { get; } = new ObservableCollection<Hotel>();

        // Приватный конструктор (чтобы нельзя было создать новый экземпляр)
        private MainWindowViewModel()
        {
            // Загружаем данные из базы данных
            LoadHotels();
        }

        private void LoadHotels()
        {
            using (var context = new ApplicationDbContext())
            {
                if (!context.Hotels.Any())
                {
                    // Добавляем тестовые данные
                    context.Hotels.Add(new Hotel { Name = "Отель Люкс", Rating = 4.5m, Address = "ул. Центральная, 123" });
                    context.Hotels.Add(new Hotel { Name = "Отель Стандарт", Rating = 3.8m, Address = "ул. Парковая, 45" });
                    context.Hotels.Add(new Hotel { Name = "Отель Эконом", Rating = 2.9m, Address = "ул. Лесная, 67" });
                    context.SaveChanges();
                }

                // Загружаем данные
                var hotels = context.Hotels.ToList();
                foreach (var hotel in hotels)
                {
                    Hotels.Add(hotel);
                }
            }
        }
    }
}