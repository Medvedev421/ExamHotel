using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ExamHotel.Models;
using ExamHotel.ViewModels;

namespace ExamHotel.Views
{
    public partial class HotelInfoView : BaseView
    {
        private BookingViewModel _viewModel;

        public HotelInfoView(Hotel hotel)
        {
            InitializeComponent();
            _viewModel = new BookingViewModel(hotel);
            DataContext = _viewModel;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            // Получаем текущее окно
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel is MainWindow mainWindow)
            {
                // Возвращаемся к списку отелей
                mainWindow.NavigateTo(new HotelListView());
            }
        }

        private async void BookRoom(object sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel is MainWindow mainWindow)
            {
                // Переходим к форме бронирования
                var bookingFormView = new BookingFormView();
                mainWindow.NavigateTo(bookingFormView);

                // Ожидаем завершения ввода данных
                var person = await bookingFormView.GetPersonAsync();
                if (person != null)
                {
                    _viewModel.BookRoom(person, this);
                }
            }
        }
    }
}