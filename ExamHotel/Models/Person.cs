namespace ExamHotel.Models
{
    public class Person
    {
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        // Внешний ключ для паспорта
        public int PassportID { get; set; }
        public Passport Passport { get; set; }
    }
}