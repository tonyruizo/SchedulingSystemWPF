using System.Windows;
using System.Windows.Controls;

namespace SchedulingSystemWPF.Views
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        private readonly Frame _mainFrame;
        public DashboardPage(string username, Frame mainframe)
        {
            InitializeComponent();
            _mainFrame = mainframe;
            DataContext = new ViewModels.DashboardViewModel(username);
        }

        public void Logout_Click(object sender, RoutedEventArgs e)
        {
            var confirmLogout = MessageBox.Show("Are you sure you want to log out?", "Confirm logout", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirmLogout == MessageBoxResult.Yes)
            {
                _mainFrame.Navigate(new LoginPage(_mainFrame));
            }

        }
    }
}
