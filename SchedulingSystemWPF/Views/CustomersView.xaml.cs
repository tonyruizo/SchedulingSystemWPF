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
            MessageBox.Show("Edit appointment");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete appointment");
        }

    }
}
