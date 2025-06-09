using System.Windows;
using System.Windows.Controls;

namespace SchedulingSystemWPF.Views
{
    /// <summary>
    /// Interaction logic for DashboardOptions.xaml
    /// </summary>
    public partial class DashboardOptions : UserControl
    {
        private readonly ContentControl _mainContent;
        public DashboardOptions(ContentControl mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;
        }

        /// <summary>
        /// Display all appointments
        /// </summary>
        private void ViewAllAppointments_Click(object sender, RoutedEventArgs e)
        {
            _mainContent.Content = new AppointmentsView(_mainContent);
        }

        /// <summary>
        /// Display all customers
        /// </summary>
        private void ViewAllCustomers_Click(object sender, RoutedEventArgs e)
        {
            _mainContent.Content = new CustomersView(_mainContent);
        }

        ///// <summary>
        ///// Add a new appointment
        ///// </summary>
        //private void AddAppointment_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Clicked add new appointment.");
        //}

        ///// <summary>
        ///// Add a new customer
        ///// </summary>
        //private void AddCustomer_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Clicked add new customer.");
        //}
    }
}
