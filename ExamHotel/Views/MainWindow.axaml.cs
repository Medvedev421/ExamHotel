using Avalonia.Controls;

namespace ExamHotel.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigateTo(new HotelListView());
        }

        public void NavigateTo(BaseView view)
        {
            MainContent.Content = view;
        }

        public void NavigateBack()
        {
            if (MainContent.Content is BaseView currentView)
            {
                // Логика для возврата на предыдущее представление
            }
        }
    }
}