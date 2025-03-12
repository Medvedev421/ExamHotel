using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ExamHotel.Views
{
    public partial class MessageBoxView : BaseView
    {
        public MessageBoxView(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Parent;
            mainWindow.NavigateBack();
        }
    }
}