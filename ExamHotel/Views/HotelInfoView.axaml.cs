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
            SetupUI();
        }

        private void SetupUI()
        {
        // Добавляем информацию об отеле
        var hotelName = new TextBlock
        {
            Text = _viewModel.Name,
            FontSize = 24,
            FontWeight = Avalonia.Media.FontWeight.Bold,
            TextAlignment = Avalonia.Media.TextAlignment.Center
        };
        MainStackPanel.Children.Add(hotelName);

        var hotelRating = new TextBlock
        {
            Text = $"Рейтинг: {_viewModel.Rating}",
            FontSize = 18,
            TextAlignment = Avalonia.Media.TextAlignment.Center
        };
        MainStackPanel.Children.Add(hotelRating);

        var hotelAddress = new TextBlock
        {
            Text = _viewModel.Address,
            FontSize = 18,
            TextAlignment = Avalonia.Media.TextAlignment.Center
        };
        MainStackPanel.Children.Add(hotelAddress);

        var hotelDescription = new TextBlock
        {
            Text = _viewModel.Description,
            FontSize = 16,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Avalonia.Thickness(0, 20, 0, 0)
        };
        MainStackPanel.Children.Add(hotelDescription);

        // Добавляем ComboBox для выбора типа номера
        var roomTypeComboBox = new ComboBox
        {
            ItemsSource = _viewModel.RoomTypes,
            DisplayMemberBinding = new Avalonia.Data.Binding("TypeName"),
            SelectedItem = _viewModel.SelectedRoomType,
            Margin = new Avalonia.Thickness(0, 20, 0, 0)
        };
        roomTypeComboBox.SelectionChanged += (s, e) =>
        {
            _viewModel.SelectedRoomType = (RoomType)roomTypeComboBox.SelectedItem;
            UpdateAvailableRooms();
        };
        MainStackPanel.Children.Add(roomTypeComboBox);

        // Добавляем ComboBox для выбора номера
        var roomComboBox = new ComboBox
        {
            ItemsSource = _viewModel.AvailableRooms,
            DisplayMemberBinding = new Avalonia.Data.Binding("RoomNumber"),
            SelectedItem = _viewModel.SelectedRoom,
            Margin = new Avalonia.Thickness(0, 10, 0, 0)
        };
        roomComboBox.SelectionChanged += (s, e) =>
        {
            _viewModel.SelectedRoom = (Room)roomComboBox.SelectedItem;
        };
        MainStackPanel.Children.Add(roomComboBox);

        // Добавляем DatePicker для выбора даты заезда
        var checkInDatePicker = new DatePicker
        {
            SelectedDate = _viewModel.CheckInDate,
            Margin = new Avalonia.Thickness(0, 10, 0, 0)
        };
        checkInDatePicker.SelectedDateChanged += (s, e) =>
        {
            if (e.NewDate.HasValue)
            {
                _viewModel.CheckInDate = e.NewDate.Value.DateTime;
            }
        };
        MainStackPanel.Children.Add(checkInDatePicker);

        // Добавляем DatePicker для выбора даты выезда
        var checkOutDatePicker = new DatePicker
        {
            SelectedDate = _viewModel.CheckOutDate,
            Margin = new Avalonia.Thickness(0, 10, 0, 0)
        };
        checkOutDatePicker.SelectedDateChanged += (s, e) =>
        {
            if (e.NewDate.HasValue)
            {
                _viewModel.CheckOutDate = e.NewDate.Value.DateTime;
            }
        };
        MainStackPanel.Children.Add(checkOutDatePicker);

        // Кнопка "Забронировать" теперь только в XAML, её не нужно добавлять в код
        }

        private void UpdateAvailableRooms()
        {
            var roomComboBox = MainStackPanel.Children.OfType<ComboBox>().FirstOrDefault(c => c.ItemsSource == _viewModel.AvailableRooms);
            if (roomComboBox != null)
            {
                roomComboBox.ItemsSource = _viewModel.AvailableRooms;
            }
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