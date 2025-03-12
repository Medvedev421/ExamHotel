using Avalonia.Controls;
using Avalonia.Interactivity;
using ExamHotel.Models;
using ExamHotel.ViewModels;
using Avalonia.VisualTree;

namespace ExamHotel.Views
{
    public partial class HotelListView : BaseView
    {
        public HotelListView()
        {
            InitializeComponent();
            DataContext = MainWindowViewModel.Instance;
        }

        private void OpenHotelInfo(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Hotel hotel)
            {
                // Получаем текущее окно
                var topLevel = TopLevel.GetTopLevel(this);
                if (topLevel is MainWindow mainWindow)
                {
                    // Переходим к представлению информации об отеле
                    mainWindow.NavigateTo(new HotelInfoView(hotel));
                }
            }
        }
    }
}