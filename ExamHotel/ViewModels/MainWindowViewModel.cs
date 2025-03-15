using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ExamHotel.Models;
using ExamHotel.DAL;

namespace ExamHotel.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // Singleton instance
        private static MainWindowViewModel _instance;
        public static MainWindowViewModel Instance => _instance ??= new MainWindowViewModel();

        // Список всех отелей
        private ObservableCollection<Hotel> _allHotels = new ObservableCollection<Hotel>();

        // Список отфильтрованных отелей
        public ObservableCollection<Hotel> Hotels { get; } = new ObservableCollection<Hotel>();

        // Фильтры
        private string _selectedHotelType;
        public string SelectedHotelType
        {
            get => _selectedHotelType;
            set
            {
                _selectedHotelType = value;
                RaisePropertyChanged();
                ApplyFilters(); // Применяем фильтры при изменении
            }
        }

        private string _selectedRatingFilter;
        public string SelectedRatingFilter
        {
            get => _selectedRatingFilter;
            set
            {
                _selectedRatingFilter = value;
                RaisePropertyChanged();
                ApplyFilters(); // Применяем фильтры при изменении
            }
        }

        // Варианты для фильтров
        public ObservableCollection<string> HotelTypes { get; } = new ObservableCollection<string>
        {
            "Все", // Опция "Все" для типа отеля
            "Люкс",
            "Стандарт",
            "Эконом"
        };

        // Текстовые значения для фильтра рейтинга
        public ObservableCollection<string> RatingFilters { get; } = new ObservableCollection<string>
        {
            "Все", // Опция "Все" для рейтинга
            "Больше 3.0",
            "Больше 4.0",
            "Больше 4.5"
        };

        // Словарь для связи текстовых значений с числовыми
        private readonly Dictionary<string, decimal?> _ratingFilterValues = new Dictionary<string, decimal?>
        {
            { "Все", null },
            { "Больше 3.0", 3.0m },
            { "Больше 4.0", 4.0m },
            { "Больше 4.5", 4.5m }
        };

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
                    _allHotels.Add(hotel);
                }
            }

            // Применяем фильтры после загрузки данных
            ApplyFilters();
        }

        // Метод для применения фильтров
        private void ApplyFilters()
        {
            var filteredHotels = _allHotels.AsEnumerable();

            // Фильтр по типу отеля (если выбрано не "Все")
            if (!string.IsNullOrEmpty(SelectedHotelType) && SelectedHotelType != "Все")
            {
                filteredHotels = filteredHotels.Where(h => h.Name.Contains(SelectedHotelType));
            }

            // Фильтр по рейтингу (если выбрано не "Все")
            if (!string.IsNullOrEmpty(SelectedRatingFilter) && SelectedRatingFilter != "Все")
            {
                // Получаем числовое значение из словаря
                if (_ratingFilterValues.TryGetValue(SelectedRatingFilter, out var rating) && rating.HasValue)
                {
                    filteredHotels = filteredHotels.Where(h => h.Rating > rating.Value);
                }
            }

            // Обновляем список отелей
            Hotels.Clear();
            foreach (var hotel in filteredHotels)
            {
                Hotels.Add(hotel);
            }
        }
    }
}