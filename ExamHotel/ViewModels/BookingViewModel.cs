using System;
using System.Collections.ObjectModel;
using System.Linq;
using ExamHotel.Models;
using ExamHotel.DAL;
using Avalonia;
using Avalonia.Controls;
using ExamHotel.Views;

namespace ExamHotel.ViewModels
{
    public class BookingViewModel : ViewModelBase
    {
        private Hotel _hotel;

        public bool CanBookRoom => SelectedRoomType != null && CheckInDate < CheckOutDate;
        public string Name => _hotel.Name; // Название отеля
        public decimal Rating => _hotel.Rating; // Рейтинг отеля
        public string Address => _hotel.Address; // Адрес отеля
        public string Description => _hotel.Description; // Описание отеля
        public ObservableCollection<RoomType> RoomTypes { get; } = new ObservableCollection<RoomType>();

        // Варианты питания
        public ObservableCollection<string> MealOptions { get; } = new ObservableCollection<string>
        {
            "Питание включено",
            "Питание не включено"
        };

        private RoomType _selectedRoomType;
        public RoomType SelectedRoomType
        {
            get => _selectedRoomType;
            set
            {
                _selectedRoomType = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanBookRoom)); // Уведомляем об изменении
                UpdatePrice(); // Обновляем цену при изменении типа номера
            }
        }

        private string _selectedMealOption;
        public string SelectedMealOption
        {
            get => _selectedMealOption;
            set
            {
                _selectedMealOption = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanBookRoom)); // Уведомляем об изменении
                UpdatePrice(); // Обновляем цену при изменении питания
            }
        }

        private DateTimeOffset? _checkInDate = DateTimeOffset.Now;
        public DateTimeOffset? CheckInDate
        {
            get => _checkInDate;
            set
            {
                _checkInDate = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanBookRoom)); // Уведомляем об изменении
                UpdateNumberOfDays(); // Обновляем количество дней
                UpdatePrice(); // Обновляем цену
            }
        }

        private DateTimeOffset? _checkOutDate = DateTimeOffset.Now.AddDays(1);
        public DateTimeOffset? CheckOutDate
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
                    _checkOutDate = CheckInDate?.AddDays(1); // Устанавливаем минимальную дату
                }
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanBookRoom));
                UpdateNumberOfDays(); // Обновляем количество дней
                UpdatePrice(); // Обновляем цену
            }
        }

        private int _numberOfDays;
        public int NumberOfDays
        {
            get => _numberOfDays;
            private set
            {
                _numberOfDays = value;
                RaisePropertyChanged();
                UpdatePrice(); // Обновляем цену при изменении количества дней
            }
        }

        private decimal? _price;
        public decimal? Price
        {
            get => _price;
            private set
            {
                _price = value;
                RaisePropertyChanged();
            }
        }

        public BookingViewModel(Hotel hotel)
        {
            _hotel = hotel;
            LoadRoomTypes();
            UpdateNumberOfDays(); // Инициализируем количество дней
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

        // Метод для обновления количества дней
        private void UpdateNumberOfDays()
        {
            if (CheckInDate.HasValue && CheckOutDate.HasValue)
            {
                NumberOfDays = (int)(CheckOutDate.Value - CheckInDate.Value).TotalDays;
            }
            else
            {
                NumberOfDays = 0;
            }
        }

        // Метод для обновления цены
        private void UpdatePrice()
        {
            if (SelectedRoomType == null || SelectedMealOption == null || NumberOfDays <= 0)
            {
                Price = null; // Если тип номера, питание или количество дней не выбраны, цена не отображается
                return;
            }

            // Базовая цена из типа номера
            decimal basePrice = SelectedRoomType.Price;

            // Добавляем стоимость питания, если выбрано "Питание включено"
            if (SelectedMealOption == "Питание включено")
            {
                basePrice += 500; // Например, 500 рублей за питание в день
            }

            // Умножаем на количество дней
            Price = basePrice * NumberOfDays;
        }

        public void BookRoom(Person person, Visual visual)
        {
            if (SelectedRoomType != null && CheckInDate < CheckOutDate)
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

                    // Выбираем первый доступный номер выбранного типа
                    var room = context.Rooms
                        .FirstOrDefault(r => r.HotelID == _hotel.HotelID && r.RoomTypeID == SelectedRoomType.RoomTypeID);

                    if (room != null)
                    {
                        // Создаем бронирование
                        var booking = new Booking
                        {
                            RoomID = room.RoomID,
                            CheckInDate = CheckInDate?.UtcDateTime ?? DateTime.UtcNow,
                            CheckOutDate = CheckOutDate?.UtcDateTime ?? DateTime.UtcNow.AddDays(1),
                            PersonID = person.PersonID
                        };
                        context.Bookings.Add(booking);
                        context.SaveChanges();
                    }
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