using Avalonia.Controls;
using Avalonia.Interactivity;
using ExamHotel.Models;
using System.Threading.Tasks;
using Avalonia.VisualTree;

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
    }
}