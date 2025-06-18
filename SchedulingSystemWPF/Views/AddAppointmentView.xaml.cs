using SchedulingSystemWPF.DatabaseAccess;
using SchedulingSystemWPF.Models;
using SchedulingSystemWPF.Services;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SchedulingSystemWPF.Views
{
    /// <summary>
    /// Interaction logic for AddAppointmentView.xaml
    /// </summary>
    public partial class AddAppointmentView : UserControl
    {
        private readonly ContentControl _parentContainer;
        public AddAppointmentView(ContentControl ParentContainer)
        {
            _parentContainer = ParentContainer;
            InitializeComponent();
            var customerR = new CustomerRepository();
            var customers = customerR.GetAllCustomers();
            CustomerBox.ItemsSource = customers;
            CustomerBox.DisplayMemberPath = "CustomerName";
            CustomerBox.SelectedValuePath = "CustomerId";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _parentContainer.Content = new DashboardOptions(_parentContainer);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Assign TextBox values
            string titleInput = TitleBox.Text?.Trim();
            string descriptionInput = DescriptionBox.Text?.Trim();
            string locationInput = LocationBox.Text?.Trim();
            string contactInput = ContactBox.Text?.Trim();
            string typeInput = TypeBox.Text?.Trim();
            string urlInput = UrlBox.Text?.Trim().ToLower();
            string startTimeInput = StartTimeBox.Text?.Trim();
            string endTimeInput = EndTimeBox.Text?.Trim();

            // Validate empty fields
            if (CustomerBox.SelectedValue == null ||
                string.IsNullOrEmpty(titleInput) ||
                string.IsNullOrEmpty(descriptionInput) ||
                string.IsNullOrEmpty(locationInput) ||
                string.IsNullOrEmpty(contactInput) ||
                string.IsNullOrEmpty(typeInput) ||
                string.IsNullOrEmpty(urlInput) ||
                string.IsNullOrEmpty(startTimeInput) ||
                string.IsNullOrEmpty(endTimeInput))
            {
                MessageBox.Show("All fields must be filled.");
                return;
            }

            int customerId = (int)CustomerBox.SelectedValue;

            // Validation for phone format pattern (7-digits ignoring non-digits)
            string phonePattern = @"^\D*(\d\D*){7}$";
            if (!Regex.IsMatch(contactInput, phonePattern))
            {
                MessageBox.Show("Please enter a valid 7-digits phone number.");
                return;
            }

            // Validate time input, restricted to valid hours 0-23 format
            if (!Regex.IsMatch(startTimeInput, @"^([0-1][0-9]|2[0-3]):[0-5][0-9]$") ||
                !Regex.IsMatch(endTimeInput, @"^([0-1][0-9]|2[0-3]):[0-5][0-9]$"))
            {
                MessageBox.Show("Please enter the time in HH:MM format. (e.g, 02:00)");
                return;
            }

            // Convert time strings to DateTime
            if (!TimeSpan.TryParse(startTimeInput, out TimeSpan startTime) ||
                !TimeSpan.TryParse(endTimeInput, out TimeSpan endTime))
            {
                MessageBox.Show("Invalid time format. Please use HH:MM. (e.g, 02:00)");
                return;
            }

            DateTime today = DateTime.Today;
            DateTime startDateTime = today.Add(startTime);
            DateTime endDateTime = today.Add(endTime);

            // Validate that End time is after Start time
            if (endDateTime <= startDateTime)
            {
                MessageBox.Show("End time must be after Start time.");
                return;
            }

            // Validate URL
            string urlPattern = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";
            if (!Regex.IsMatch(urlInput, urlPattern))
            {
                MessageBox.Show("Please enter a valid URL (e.g., https://example.com).");
                return;
            }

            // Get current username
            var createdBy = SessionManager.Username;
            // Get current userId
            var userId = SessionManager.UserId;

            // Appointment Object
            var appointment = new Appointment
            {
                CustomerId = customerId,
                UserId = userId,
                Title = titleInput,
                Description = descriptionInput,
                Location = locationInput,
                Contact = contactInput,
                Type = typeInput,
                Url = urlInput,
                Start = startDateTime,
                End = endDateTime
            };

            // Add Appointment
            var appointmentR = new AppointmentRepository();

            appointmentR.AddAppointment(appointment, createdBy);

            MessageBox.Show("Appointment has been added!");
            _parentContainer.Content = new AppointmentsView(_parentContainer); ;
        }
    }
}
