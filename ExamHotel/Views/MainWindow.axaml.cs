using System.Collections.Generic;
using Avalonia.Controls;

namespace ExamHotel.Views
{
    public partial class MainWindow : Window
    {
        private Stack<BaseView> _navigationStack = new Stack<BaseView>();
        
        public MainWindow()
        {
            InitializeComponent();
            NavigateTo(new HotelListView()); 
        }
        public void NavigateTo(BaseView view)
        {
            if (MainContent.Content is BaseView currentView)
            {
                _navigationStack.Push(currentView);
            }

            MainContent.Content = view;
        }

        public void NavigateBack()
        {
            if (_navigationStack.Count > 0)
            {
                MainContent.Content = _navigationStack.Pop();
            }
        }
    }
}