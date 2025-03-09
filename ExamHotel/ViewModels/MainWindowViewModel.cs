using Avalonia.Controls;
using ExamHotel.Models;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace ExamHotel.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // Список отелей
        public ObservableCollection<Hotel> Hotels { get; } = new ObservableCollection<Hotel>();

        public MainWindowViewModel()
        {
            // Заполняем список отелей (пока тестовыми данными)
            Hotels.Add(new Hotel { HotelID = 1, Name = "Отель Люкс", Rating = 4.5m, Address = "ул. Центральная, 123" });
            Hotels.Add(new Hotel { HotelID = 2, Name = "Отель Стандарт", Rating = 3.8m, Address = "ул. Парковая, 45" });
            Hotels.Add(new Hotel { HotelID = 3, Name = "Отель Эконом", Rating = 2.9m, Address = "ул. Лесная, 67" });
        }
    }
}