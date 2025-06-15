using SchedulingSystemWPF.DatabaseAccess;
using SchedulingSystemWPF.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SchedulingSystemWPF.Views
{
    /// <summary>
    /// Interaction logic for CustomersView.xaml
    /// </summary>
    public partial class CustomersView : UserControl
    {
        private readonly CustomerRepository _customerRepo = new CustomerRepository();
        private readonly ContentControl _parentContainer;
        public CustomersView(ContentControl ParentContainer)
        {
            InitializeComponent();
            _parentContainer = ParentContainer;
            LoadCustomers();
        }

        public void LoadCustomers()
        {
            List<Customer> customers = _customerRepo.GetAllCustomers();
            CustomerGrid.ItemsSource = customers;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _parentContainer.Content = new DashboardOptions(_parentContainer);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _parentContainer.Content = new AddCustomerView(_parentContainer);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit customer");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerGrid.SelectedItem is Customer selectedCustomer)
            {
                int customerId = selectedCustomer.CustomerId;

                var confirmDelete = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (confirmDelete == MessageBoxResult.Yes)
                {
                    var customerR = new CustomerRepository();

                    customerR.DeleteCustomer(customerId);

                    // Refresh Grid
                    CustomerGrid.ItemsSource = customerR.GetAllCustomers();
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.");
            }
        }
    }
}
