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
            DataContext = hotel; // Устанавливаем DataContext для текущего отеля
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            // Создаем новое окно MainWindow с Singleton ViewModel
            var mainWindow = new MainWindow
            {
                DataContext = MainWindowViewModel.Instance // Используем Singleton
            };

            // Показываем новое окно
            mainWindow.Show();

            // Закрываем текущее окно
            this.Close();
        }
    }
}