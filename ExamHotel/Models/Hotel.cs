using System.Collections.Generic;

namespace ExamHotel.Models
{
    public class Hotel
    {
        public int HotelID { get; set; }
        public string Name { get; set; }
        public decimal Rating { get; set; }
        public string Address { get; set; }
        public string Description { get; set; } // Новое поле

        // Навигационное свойство для комнат
        public ICollection<Room> Rooms { get; set; }
    }
}