using SchedulingSystemWPF.DatabaseAccess;
using SchedulingSystemWPF.Models;
using SchedulingSystemWPF.Resources;
using SchedulingSystemWPF.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchedulingSystemWPF.Views
{
    /// <summary>
    /// Interaction logic for AddCustomerView.xaml
    /// </summary>
    public partial class AddCustomerView : UserControl
    {
        private readonly ContentControl _parentContainer;
        private readonly Customer _customerToEdit;
        private readonly bool _isEditMode;

        // Constructor
        public AddCustomerView(ContentControl ParentContainer, Customer customerToEdit = null)
        {
            InitializeComponent();
            _parentContainer = ParentContainer;
            _customerToEdit = customerToEdit;
            _isEditMode = _customerToEdit != null;

            if (_isEditMode)
            {
                AddButton.Content = Lang.UpdateButton;
                PopulateForm();
            }
        }

        private void PopulateForm()
        {
            if (_customerToEdit == null) return;
            CustomerNameBox.Text = _customerToEdit.CustomerName;
            AddressLine1Box.Text = _customerToEdit.AddressLine1;
            AddressLine2Box.Text = _customerToEdit.AddressLine2 ?? string.Empty;
            CityBox.Text = _customerToEdit.City;
            CountryBox.Text = _customerToEdit.Country;
            PostalCodeBox.Text = _customerToEdit.PostalCode;
            PhoneBox.Text = _customerToEdit.Phone?.Replace("-", "");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _parentContainer.Content = new CustomersView(_parentContainer);
        }

        private void PhoneBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Only digits and dashes
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9\-]+$");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Set culture for localization
            CultureInfo culture = CultureInfo.CurrentUICulture;

            // Assign TextBox values
            // If !null "?", excecute Trim() to remove white space from start and end
            string nameInput = CustomerNameBox.Text?.Trim();
            string address1Input = AddressLine1Box.Text?.Trim();
            string address2Input = AddressLine2Box.Text?.Trim();
            string cityInput = CityBox.Text?.Trim();
            string countryInput = CountryBox.Text?.Trim();
            string postalCodeInput = PostalCodeBox.Text?.Trim();
            string phoneInput = PhoneBox.Text?.Trim();

            // Validation if fields are empty
            if (string.IsNullOrEmpty(nameInput) ||
                string.IsNullOrEmpty(address1Input) ||
                string.IsNullOrEmpty(cityInput) ||
                string.IsNullOrEmpty(countryInput) ||
                string.IsNullOrEmpty(postalCodeInput) ||
                string.IsNullOrEmpty(phoneInput))
            {
                MessageBox.Show(Lang.AllFieldsRequired, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validation for string type in name
            if (int.TryParse(nameInput, out _))
            {
                MessageBox.Show(Lang.InvalidName, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validation for phone format pattern (7-digits)
            string phonePattern = @"^\d{7}$";
            if (!Regex.IsMatch(phoneInput, phonePattern))
            {
                MessageBox.Show(Lang.InvalidPhoneFormat, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Format phone number as 222 - 3434 for database storage
            string formattedPhone = $"{phoneInput.Substring(0, 3)}-{phoneInput.Substring(3, 4)}";

            // Validation for postal code (should be all digits and <= 5 in length)
            if (!postalCodeInput.All(char.IsDigit) ||
                postalCodeInput.Length > 5)
            {
                MessageBox.Show(Lang.InvalidPostalCode, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Get current username
            var username = SessionManager.Username;
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show(Lang.SessionNotFound, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var countryR = new CountryRepository();
            var cityR = new CityRepository();
            var addressR = new AddressRepository();
            var customerR = new CustomerRepository();

            try
            {
                // Get or insert country
                int countryId = countryR.GetOrInsertCountry(countryInput, username);

                // Get or insert city
                int cityId = cityR.GetOrInsertCity(cityInput, countryId, username);

                int addressId;

                if (_isEditMode)
                {
                    // Update existing address
                    addressR.UpdateAddress(_customerToEdit.AddressId, address1Input, address2Input, postalCodeInput, formattedPhone, cityId, username);
                    addressId = _customerToEdit.AddressId;

                    // Update customer
                    _customerToEdit.CustomerName = nameInput;
                    _customerToEdit.AddressId = addressId;
                    _customerToEdit.LastUpdate = DateTime.UtcNow;
                    _customerToEdit.LastUpdateBy = username;

                    customerR.EditCustomer(_customerToEdit.CustomerId, nameInput, addressId, username);
                    MessageBox.Show(Lang.CustomerUpdated, Lang.SuccessTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Get or insert address
                    addressId = addressR.GetOrInsertAddress(address1Input, address2Input, postalCodeInput, formattedPhone, cityId, username);

                    // Add Customer
                    customerR.AddCustomer(nameInput, addressId, username);
                    MessageBox.Show(Lang.CustomerAdded, Lang.SuccessTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Navigate only on success
                _parentContainer.Content = new CustomersView(_parentContainer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Lang.SaveCustomerFailed, ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }

        }
    }
}
