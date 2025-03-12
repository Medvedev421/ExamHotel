using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ExamHotel.Views
{
    public partial class MessageBox : Window
    {
        public MessageBox(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            Close(true); // Закрываем окно с результатом true
        }
    }
}