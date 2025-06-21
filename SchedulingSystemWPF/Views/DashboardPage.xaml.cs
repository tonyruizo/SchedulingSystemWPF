using SchedulingSystemWPF.Resources;
using SchedulingSystemWPF.Services;
using System.Globalization;
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

            // Load Dashboard options view
            MainContent.Content = new DashboardOptions(MainContent);
        }

        public void Logout_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo culture = CultureInfo.CurrentUICulture;

            var confirmLogout = MessageBox.Show(Lang.ConfirmLogout,
                Lang.ConfirmLogoutTitle,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmLogout == MessageBoxResult.Yes)
            {
                // Log logout activity
                ActivityLogger.LogActivity(SessionManager.Username, Lang.UserLoggedOut);

                SessionManager.ClearSession();
                _mainFrame.Navigate(new LoginPage(_mainFrame));
            }

        }


    }
}
