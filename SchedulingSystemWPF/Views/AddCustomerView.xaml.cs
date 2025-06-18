using SchedulingSystemWPF.DatabaseAccess;
using SchedulingSystemWPF.Services;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SchedulingSystemWPF.Views
{
    /// <summary>
    /// Interaction logic for AddCustomerView.xaml
    /// </summary>
    public partial class AddCustomerView : UserControl
    {
        private readonly ContentControl _parentContainer;
        public AddCustomerView(ContentControl ParentContainer)
        {
            _parentContainer = ParentContainer;
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _parentContainer.Content = new DashboardOptions(_parentContainer);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
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
                MessageBox.Show("All fields must be filled.");
                return;
            }

            // Validation for string type in name
            if (int.TryParse(nameInput, out _))
            {
                MessageBox.Show("Customer's name cannot be a number.");
                return;
            }

            // Validation for phone format pattern (7-digits ignoring non-digits)
            string phonePattern = @"^\D*(\d\D*){7}$";
            if (!Regex.IsMatch(phoneInput, phonePattern))
            {
                MessageBox.Show("Please enter a valid 7-digits phone number.");
                return;
            }

            // Validation for postal code (should be all digits and <= 5 in length)
            if (!postalCodeInput.All(char.IsDigit) ||
                postalCodeInput.Length > 5)
            {
                MessageBox.Show("Please enter a valid postal code.");
                return;
            }

            var createdBy = SessionManager.Username;

            var countryR = new CountryRepository();
            var cityR = new CityRepository();
            var addressR = new AddressRepository();
            var customerR = new CustomerRepository();

            // Get or insert country
            int countryId = countryR.GetOrInsertCountry(countryInput, createdBy);

            // Get or insert city
            int cityId = cityR.GetOrInsertCity(cityInput, countryId, createdBy);

            // Get or insert address
            int addressId = addressR.GetOrInsertAddress(address1Input, address2Input, postalCodeInput, phoneInput, cityId, createdBy);

            // Add Customer
            customerR.AddCustomer(nameInput, addressId, createdBy);

            MessageBox.Show("Customer has been added!");
            _parentContainer.Content = new CustomersView(_parentContainer);

        }
    }
}
