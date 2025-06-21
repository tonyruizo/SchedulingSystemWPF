using SchedulingSystemWPF.DatabaseAccess;
using SchedulingSystemWPF.Models;
using SchedulingSystemWPF.Resources;
using System;
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
            if (CustomerGrid.SelectedItem is Customer selectedView)
            {
                var customer = new Customer
                {
                    CustomerId = selectedView.CustomerId,
                    CustomerName = selectedView.CustomerName,
                    AddressId = selectedView.AddressId,
                    AddressLine1 = selectedView.AddressLine1,
                    AddressLine2 = selectedView.AddressLine2,
                    City = selectedView.City,
                    Country = selectedView.Country,
                    PostalCode = selectedView.PostalCode,
                    Phone = selectedView.Phone,
                    CreateDate = selectedView.CreateDate,
                    CreatedBy = selectedView.CreatedBy,
                    LastUpdate = selectedView.LastUpdate,
                    LastUpdateBy = selectedView.LastUpdateBy
                };

                _parentContainer.Content = new AddCustomerView(_parentContainer, customer);
            }
            else
            {
                MessageBox.Show(Lang.NoCustomerSelected, Lang.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerGrid.SelectedItem is Customer selectedCustomer)
            {
                int customerId = selectedCustomer.CustomerId;

                var confirmDelete = MessageBox.Show(Lang.ConfirmDelete, Lang.ConfirmDeleteTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (confirmDelete == MessageBoxResult.Yes)
                {
                    var customerR = new CustomerRepository();

                    try
                    {
                        customerR.DeleteCustomer(customerId);
                        CustomerGrid.ItemsSource = customerR.GetAllCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format(Lang.DeleteFailed, ex.Message), Lang.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(Lang.NoCustomerSelected, Lang.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
