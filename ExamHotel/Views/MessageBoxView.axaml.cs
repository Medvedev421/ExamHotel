using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree; // Добавьте этот using

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
            // Получаем корневое окно
            var mainWindow = (MainWindow)TopLevel.GetTopLevel(this);
            if (mainWindow == null)
            {
                // Если окно не найдено, выходим
                return;
            }

            // Возвращаемся назад
            mainWindow.NavigateBack();
        }
    }
}