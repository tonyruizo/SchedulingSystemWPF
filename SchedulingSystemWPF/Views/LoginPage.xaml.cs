using SchedulingSystemWPF.DatabaseAccess;
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

        private void Login_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Assign TextBox values to variables for validation
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            var userR = new UserRepository();

            if (userR.ValidateUser(username, password))
            {
                // If true, navigate to authorized dashboard page
                _mainFrame.Navigate(new DashboardPage(username, _mainFrame));
            }
            else
            {
                MessageBox.Show("Invalid credentials. Please try again.");
            }
        }
    }
}
