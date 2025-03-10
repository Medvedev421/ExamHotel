using Avalonia.Controls;
using Avalonia.Interactivity;
using ExamHotel.Models;
using ExamHotel.ViewModels;

namespace ExamHotel.Views
{
    public partial class InfoWindow : Window
    {
        public InfoWindow(Hotel hotel)
        {
            InitializeComponent();
            DataContext = hotel; // Устанавливаем DataContext
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            // Получаем ViewModel текущего окна (InfoWindow)
            var mainViewModel = this.DataContext as MainWindowViewModel;

            // Создаем новое окно MainWindow
            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel // Устанавливаем существующий ViewModel
            };

            // Показываем новое окно
            mainWindow.Show();

            // Закрываем текущее окно
            this.Close();
        }
    }
}