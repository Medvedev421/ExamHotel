using Avalonia.Controls;
using Avalonia.Interactivity;
using ExamHotel.Models;
using System.Threading.Tasks;

namespace ExamHotel.Views
{
    public partial class BookingFormView : BaseView
    {
        public Person Person { get; private set; }

        public BookingFormView()
        {
            InitializeComponent();
        }

        private void OnConfirmButtonClick(object sender, RoutedEventArgs e)
        {
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
                PhoneNumber = EmailTextBox.Text
            };

            var mainWindow = (MainWindow)Parent;
            mainWindow.NavigateBack();
        }

        public Task<bool> GetPersonAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            return tcs.Task;
        }
    }
}