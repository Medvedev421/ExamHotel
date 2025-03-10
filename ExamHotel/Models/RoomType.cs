using System.Collections.Generic;

namespace ExamHotel.Models
{
    public class RoomType
    {
        public int RoomTypeID { get; set; }
        public string TypeName { get; set; }
        public decimal Price { get; set; }

        // Навигационное свойство для комнат
        public ICollection<Room> Rooms { get; set; }
    }
}