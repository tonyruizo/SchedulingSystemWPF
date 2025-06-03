using System.Windows;
using System.Windows.Controls;

namespace SchedulingSystemWPF.Views
{
    /// <summary>
    /// Interaction logic for DashboardOptions.xaml
    /// </summary>
    public partial class DashboardOptions : UserControl
    {
        public DashboardOptions()
        {
            InitializeComponent();
        }

        private void ViewAllAppointments_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked show all appointments");
        }

        private void ViewAllCustomers_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked show all customers");
        }

        private void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked add new appointment.");
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked add new customer.");
        }
    }
}
