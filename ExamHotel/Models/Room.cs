namespace ExamHotel.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; }

        // Внешний ключ для отеля
        public int HotelID { get; set; }
        public Hotel Hotel { get; set; }

        // Внешний ключ для типа комнаты
        public int RoomTypeID { get; set; }
        public RoomType RoomType { get; set; }
    }
}