using System;
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
                Console.WriteLine("Calling DbInitializer.Initialize...");
                DbInitializer.Initialize(context); // Инициализируем базу данных

                var hotels = context.Hotels.ToList();
                foreach (var hotel in hotels)
                {
                    Hotels.Add(hotel);
                }
            }
        }
    }
}