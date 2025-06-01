using System.ComponentModel;

namespace SchedulingSystemWPF.ViewModels
{
    /// <summary>
    /// Represents the view model for the dashboard page.
    /// </summary>
    /// <remarks>It implements <see cref="INotifyPropertyChanged"/> to support data binding</remarks>
    public class DashboardViewModel : INotifyPropertyChanged
    {
        // Store username from the login process.
        private readonly string _username;

        /// <summary>
        /// Initialize constructor.
        /// </summary>
        /// <param name="username">Username.</param>
        public DashboardViewModel(string username)
        {
            _username = username;
        }

        // Display message
        public string WelcomeUser => $"Hi, {_username}";

        // Binding that updates on property changes.
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
