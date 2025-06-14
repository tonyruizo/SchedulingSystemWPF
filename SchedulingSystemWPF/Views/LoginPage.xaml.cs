using SchedulingSystemWPF.DatabaseAccess;
using SchedulingSystemWPF.Services;
using System.Windows;
using System.Windows.Controls;

namespace SchedulingSystemWPF.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly Frame _mainFrame;

        public LoginPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Assign TextBox values to variables for validation
            var username = UsernameBox.Text?.Trim();
            var password = PasswordBox.Password?.Trim();

            if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
            {
                var userR = new UserRepository();

                var authenticatedUser = userR.ValidateUser(username, password);

                if (authenticatedUser != null)
                {
                    // set current session for the user
                    SessionManager.LoggedInUser = authenticatedUser;

                    // Navigate to authorized dashboard page
                    _mainFrame.Navigate(new DashboardPage(username, _mainFrame));
                }
                else
                {
                    MessageBox.Show("The username and password do not match. Please try again.");
                }
            }
            else
            {
                MessageBox.Show("Please fill out BOTH username and password.");
            }

        }

    }
}
