using SchedulingSystemWPF.DatabaseAccess;
using SchedulingSystemWPF.Resources;
using SchedulingSystemWPF.Services;
using System;
using System.Globalization;
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

            SetLocalizedStrings();
        }

        /// <summary>
        /// Set Localization
        /// </summary>
        private void SetLocalizedStrings()
        {
            LoginTitle.Text = Lang.LoginTitle;
            LoginSubTitle.Text = Lang.LoginSubTitle;
            LoginDescription1.Text = Lang.LoginDescription1;
            LoginDescription2.Text = Lang.LoginDescription2;
            UsernameLabel.Text = Lang.UsernameLabel;
            PasswordLabel.Text = Lang.PasswordLabel;
            LoginButton.Content = Lang.LoginButton;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Culture for Localization
            CultureInfo culture = CultureInfo.CurrentUICulture;

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

                    // Log login activity
                    ActivityLogger.LogActivity(username, Lang.UserLoggedIn);

                    // Navigate to authorized dashboard page
                    _mainFrame.Navigate(new DashboardPage(username, _mainFrame));

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
                                    string.Format(Lang.AppointmentMessageFormat,
                                        a.Title,
                                        a.CustomerName,
                                        TimeZoneInfo.ConvertTimeFromUtc(a.Start, localTimeZone).ToString(culture.DateTimeFormat.ShortTimePattern))));
                                MessageBox.Show($"{Lang.UpcomingAppointments}\n{message}", Lang.AppointmentAlert, MessageBoxButton.OK, MessageBoxImage.Information);

                                SessionManager.HasAlert = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(string.Format(Lang.AlertError, ex.Message), Lang.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(Lang.InvalidCredentials, Lang.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(Lang.MissingCredentials, Lang.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
