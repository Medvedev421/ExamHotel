using Avalonia.Controls;
using Avalonia.Interactivity;
using ExamHotel.Models;
using System.Threading.Tasks;
using Avalonia.VisualTree;
using System.Text.RegularExpressions; // Добавляем для работы с регулярными выражениями

namespace ExamHotel.Views
{
    public partial class BookingFormView : BaseView
    {
        public Person Person { get; private set; }
        private TaskCompletionSource<Person> _tcs;

        public BookingFormView()
        {
            InitializeComponent();
            ResetTaskCompletionSource(); // Инициализируем TaskCompletionSource
        }

        private void ResetTaskCompletionSource()
        {
            _tcs = new TaskCompletionSource<Person>();
        }

        private void OnConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            // Получаем корневое окно
            var mainWindow = (MainWindow)TopLevel.GetTopLevel(this);
            if (mainWindow == null)
            {
                // Если окно не найдено, выходим
                return;
            }

            // Проверяем, что все поля заполнены
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PassportSeriesNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(PassportIssuedByTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                // Выводим сообщение об ошибке
                mainWindow.NavigateTo(new MessageBoxView("Все поля должны быть заполнены!"));
                return;
            }

            // Проверяем корректность ввода серии и номера паспорта
            var passportParts = PassportSeriesNumberTextBox.Text.Split(' ');
            if (passportParts.Length < 2)
            {
                // Выводим сообщение об ошибке
                mainWindow.NavigateTo(new MessageBoxView("Серия и номер паспорта должны быть разделены пробелом!"));
                return;
            }

            // Проверяем, что серия и номер паспорта содержат только числа
            if (!IsNumeric(passportParts[0]) || !IsNumeric(passportParts[1]))
            {
                // Выводим сообщение об ошибке
                mainWindow.NavigateTo(new MessageBoxView("Серия и номер паспорта должны содержать только цифры!"));
                return;
            }

            // Проверяем корректность электронной почты
            if (!IsValidEmail(EmailTextBox.Text))
            {
                // Выводим сообщение об ошибке
                mainWindow.NavigateTo(new MessageBoxView("Некорректный формат электронной почты!"));
                return;
            }

            // Создаем объект Person
            Person = new Person
            {
                FirstName = FirstNameTextBox.Text,
                LastName = LastNameTextBox.Text,
                Passport = new Passport
                {
                    PassportSeries = passportParts[0], // Серия паспорта
                    PassportNumber = passportParts[1], // Номер паспорта
                    DepartmentCode = PassportIssuedByTextBox.Text
                },
                PhoneNumber = EmailTextBox.Text
            };

            if (!_tcs.Task.IsCompleted)
            {
                _tcs.SetResult(Person);
            }

            // Возвращаемся назад
            mainWindow.NavigateBack();
        }

        public Task<Person> GetPersonAsync()
        {
            return _tcs.Task;
        }

        // Метод для проверки, что строка содержит только числа
        private bool IsNumeric(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }

        // Метод для проверки корректности электронной почты
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}