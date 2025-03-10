using Avalonia.Controls;
using Avalonia.Interactivity;
using ExamHotel.Views;
using ExamHotel.Models;

namespace ExamHotel.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик события Click для кнопки
        private void OpenNewWindow(object sender, RoutedEventArgs e)
        {
            // Получаем данные об отеле из DataContext кнопки
            if (sender is Button button && button.DataContext is Hotel hotel)
            {
                // Создаем новое окно
                var newWindow = new InfoWindow(hotel);

                // Показываем новое окно
                newWindow.Show();

                // Скрываем текущее окно
                this.Hide();
            }
        }
    }
}