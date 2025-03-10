namespace ExamHotel.Models
{
    public class Passport
    {
        public int PassportID { get; set; }
        public string PassportNumber { get; set; }
        public string PassportSeries { get; set; }
        public string DepartmentCode { get; set; }

        // Навигационное свойство для человека
        public Person Person { get; set; }
    }
}