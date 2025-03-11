using Avalonia.Controls;
using Avalonia.Interactivity;
using ExamHotel.Models;
using ExamHotel.ViewModels;
using System;
using System.Linq;

namespace ExamHotel.Views
{
    public partial class InfoWindow : Window
    {
        private BookingViewModel _viewModel;

        public InfoWindow(Hotel hotel)
        {
            InitializeComponent();
            _viewModel = new BookingViewModel(hotel);
            SetupUI(); // Настраиваем интерфейс
            BackButton.Click += GoBack; // Обработчик кнопки "Назад"
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
                ItemsSource = _viewModel.RoomTypes, // Используем ItemsSource
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
                ItemsSource = _viewModel.AvailableRooms, // Используем ItemsSource
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
                    _viewModel.CheckInDate = e.NewDate.Value.DateTime; // Преобразуем DateTimeOffset в DateTime
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
                    _viewModel.CheckOutDate = e.NewDate.Value.DateTime; // Преобразуем DateTimeOffset в DateTime
                }
            };
            MainStackPanel.Children.Add(checkOutDatePicker);

            // Добавляем кнопку бронирования
            var bookButton = new Button
            {
                Content = "Забронировать",
                Margin = new Avalonia.Thickness(0, 20, 0, 0)
            };
            bookButton.Click += BookRoom;
            MainStackPanel.Children.Add(bookButton);
        }

        private void UpdateAvailableRooms()
        {
            // Обновляем список доступных комнат
            var roomComboBox = MainStackPanel.Children.OfType<ComboBox>().FirstOrDefault(c => c.ItemsSource == _viewModel.AvailableRooms);
            if (roomComboBox != null)
            {
                roomComboBox.ItemsSource = _viewModel.AvailableRooms;
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow
            {
                DataContext = MainWindowViewModel.Instance
            };
            mainWindow.Show();
            this.Close();
        }

        private void BookRoom(object sender, RoutedEventArgs e)
        {
            // Логика бронирования
            var person = new Person
            {
                FirstName = "Иван",
                LastName = "Иванов",
                PhoneNumber = "1234567890",
                PassportID = 1
            };

            _viewModel.BookRoom(person);
            // Можно добавить уведомление об успешном бронировании
        }
    }
}