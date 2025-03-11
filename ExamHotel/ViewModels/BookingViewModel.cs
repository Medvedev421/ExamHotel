using System;
using ExamHotel.Models;
using ExamHotel.DAL;
using System.Collections.ObjectModel;
using System.Linq;

namespace ExamHotel.ViewModels
{
    public class BookingViewModel : ViewModelBase
    {
        private Hotel _hotel;
        
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
            }
        }

        private DateTime _checkOutDate = DateTime.Today.AddDays(1);
        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                _checkOutDate = value;
                RaisePropertyChanged();
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

        public void BookRoom(Person person)
        {
            if (SelectedRoom != null && CheckInDate < CheckOutDate)
            {
                using (var context = new ApplicationDbContext())
                {
                    var booking = new Booking
                    {
                        RoomID = SelectedRoom.RoomID,
                        CheckInDate = CheckInDate,
                        CheckOutDate = CheckOutDate,
                        PersonID = person.PersonID
                    };
                    context.Bookings.Add(booking);
                    context.SaveChanges();
                }
            }
        }
    }
}