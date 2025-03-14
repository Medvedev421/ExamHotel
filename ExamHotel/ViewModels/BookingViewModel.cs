using System;
using ExamHotel.Models;
using ExamHotel.DAL;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using ExamHotel.Views;

namespace ExamHotel.ViewModels
{
    public class BookingViewModel : ViewModelBase
    {
        private Hotel _hotel;
        
        public bool CanBookRoom => SelectedRoomType != null && SelectedRoom != null && CheckInDate < CheckOutDate;
        public string Name => _hotel.Name; // Название отеля
        public decimal Rating => _hotel.Rating; // Рейтинг отеля
        public string Address => _hotel.Address; // Адрес отеля
        public string Description => _hotel.Description; // Описание отеля
        public ObservableCollection<RoomType> RoomTypes { get; } = new ObservableCollection<RoomType>();
        public ObservableCollection<Room> AvailableRooms { get; } = new ObservableCollection<Room>();

        private RoomType _selectedRoomType;
        public RoomType SelectedRoomType
        {
            get => _selectedRoomType;
            set
            {
                _selectedRoomType = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanBookRoom)); // Уведомляем об изменении
                LoadAvailableRooms();
            }
        }

        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanBookRoom)); // Уведомляем об изменении
            }
        }

        private DateTime _checkInDate = DateTime.Today;
        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                _checkInDate = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanBookRoom)); // Уведомляем об изменении
            }
        }

        private DateTime _checkOutDate = DateTime.Today.AddDays(1);
        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                if (value > CheckInDate) // Проверяем, что дата выезда позже даты заезда
                {
                    _checkOutDate = value;
                }
                else
                {
                    _checkOutDate = CheckInDate.AddDays(1); // Устанавливаем минимальную дату
                }
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanBookRoom));
            }
        }

        public BookingViewModel(Hotel hotel)
        {
            _hotel = hotel;
            LoadRoomTypes();
        }

        private void LoadRoomTypes()
        {
            using (var context = new ApplicationDbContext())
            {
                var roomTypes = context.RoomTypes.ToList();
                foreach (var roomType in roomTypes)
                {
                    RoomTypes.Add(roomType);
                }
            }
        }

        private void LoadAvailableRooms()
        {
            AvailableRooms.Clear();
            if (SelectedRoomType != null)
            {
                using (var context = new ApplicationDbContext())
                {
                    var rooms = context.Rooms
                        .Where(r => r.HotelID == _hotel.HotelID && r.RoomTypeID == SelectedRoomType.RoomTypeID)
                        .ToList();
                    foreach (var room in rooms)
                    {
                        AvailableRooms.Add(room);
                    }
                }
            }
        }

        public void BookRoom(Person person, Visual visual)
        {
            if (SelectedRoom != null && CheckInDate < CheckOutDate)
            {
                using (var context = new ApplicationDbContext())
                {
                    // Сохраняем паспортные данные
                    context.Passports.Add(person.Passport);
                    context.SaveChanges();

                    // Сохраняем данные о человеке
                    person.PassportID = person.Passport.PassportID;
                    context.People.Add(person);
                    context.SaveChanges();

                    // Создаем бронирование
                    var booking = new Booking
                    {
                        RoomID = SelectedRoom.RoomID,
                        CheckInDate = CheckInDate.ToUniversalTime(), // Преобразуем в UTC
                        CheckOutDate = CheckOutDate.ToUniversalTime(), // Преобразуем в UTC
                        PersonID = person.PersonID
                    };
                    context.Bookings.Add(booking);
                    context.SaveChanges();
                }

                // Получаем корневое окно
                var mainWindow = (MainWindow)TopLevel.GetTopLevel(visual);
                if (mainWindow != null)
                {
                    // Выводим сообщение об успешном завершении бронирования
                    mainWindow.NavigateTo(new MessageBoxView("Бронирование успешно завершено!"));
                }
            }
        }
    }
}