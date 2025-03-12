using Avalonia.Controls;
using Avalonia.Interactivity;
using ExamHotel.Models;

namespace ExamHotel.Views
{
    public partial class BookingFormWindow : Window
    {
        public Person Person { get; private set; }

        public BookingFormWindow()
        {
            InitializeComponent();
        }

        private void OnConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            // Создаем объект Person с введенными данными
            Person = new Person
            {
                FirstName = FirstNameTextBox.Text,
                LastName = LastNameTextBox.Text,
                Passport = new Passport
                {
                    PassportSeries = PassportSeriesNumberTextBox.Text.Split(' ')[0],
                    PassportNumber = PassportSeriesNumberTextBox.Text.Split(' ')[1],
                    DepartmentCode = PassportIssuedByTextBox.Text
                },
                PhoneNumber = EmailTextBox.Text // Используем Email как PhoneNumber для примера
            };

            // Закрываем окно с результатом OK
            Close(true);
        }
    }
}