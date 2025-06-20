using SchedulingSystemWPF.DatabaseAccess;
using SchedulingSystemWPF.Services;
using System;
using System.Linq;
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
                    SessionManager.HasAlert = false;

                    // Check for upcoming appointments
                    if (!SessionManager.HasAlert) // If true
                    {
                        try
                        {
                            var appointmentR = new AppointmentRepository();
                            var now = DateTime.UtcNow;
                            var alertTime = now.AddMinutes(15);

                            var upcomingAppointments = appointmentR.GetUpcomingAppointments(authenticatedUser.UserId, now, alertTime);

                            if (upcomingAppointments.Any())
                            {
                                var localTimeZone = TimeZoneInfo.Local;
                                var message = string.Join("\n", upcomingAppointments.Select(a =>
                                    $"Appointment: {a.Title} with {a.CustomerName} at {TimeZoneInfo.ConvertTimeFromUtc(a.Start, localTimeZone):HH:mm}"));

                                MessageBox.Show($"Upcoming appointments within 15 minutes:\n{message}", "Appointment Alert");
                                SessionManager.HasAlert = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to check upcoming appointments: {ex.Message}");
                        }
                    }

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
