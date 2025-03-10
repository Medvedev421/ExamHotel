using Avalonia.Controls;
using Avalonia.Interactivity;
using ExamHotel.Views;

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
            // Создаем новое окно
            var newWindow = new InfoWindow();
            
            // Показываем новое окно
            newWindow.Show();
        }
    }
}