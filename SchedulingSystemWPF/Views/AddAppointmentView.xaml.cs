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
    /// Interaction logic for AddAppointmentView.xaml
    /// </summary>
    public partial class AddAppointmentView : UserControl
    {
        private readonly ContentControl _parentContainer;
        private readonly Appointment _appointmentToEdit;
        private readonly bool _isEditMode;

        // Constructor 
        public AddAppointmentView(ContentControl parentContainer, Appointment appointmentToEdit = null)
        {
            InitializeComponent();
            _parentContainer = parentContainer;
            _appointmentToEdit = appointmentToEdit;
            _isEditMode = _appointmentToEdit != null;

            LoadCustomers();

            if (_isEditMode)
            {
                AddButton.Content = Lang.UpdateButton;
                PopulateForm();

            }
        }

        private void LoadCustomers()
        {
            var customerR = new CustomerRepository();
            var customers = customerR.GetAllCustomers();
            CustomerBox.ItemsSource = customers;
            CustomerBox.DisplayMemberPath = "CustomerName";
            CustomerBox.SelectedValuePath = "CustomerId";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _parentContainer.Content = new AppointmentsView(_parentContainer);
        }

        private void PopulateForm()
        {
            if (_appointmentToEdit == null) return;
            TitleBox.Text = _appointmentToEdit.Title;
            DescriptionBox.Text = _appointmentToEdit.Description;
            LocationBox.Text = _appointmentToEdit.Location;
            ContactBox.Text = _appointmentToEdit.Contact?.Replace("-", "");
            TypeBox.Text = _appointmentToEdit.Type;
            UrlBox.Text = _appointmentToEdit.Url;
            StartTimeBox.Text = _appointmentToEdit.Start.ToString("HH:mm");
            EndTimeBox.Text = _appointmentToEdit.End.ToString("HH:mm");
            CustomerBox.SelectedValue = _appointmentToEdit.CustomerId;
        }

        private void ContactBox_Preview(object sender, TextCompositionEventArgs e)
        {
            // Only digits (0-9)
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Set culture for localization
            CultureInfo culture = CultureInfo.CurrentUICulture;

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
                MessageBox.Show(Lang.AllFieldsRequired, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int customerId = (int)CustomerBox.SelectedValue;

            // Validation for phone format pattern (7-digits)
            string phonePattern = @"^\d{7}$";
            if (!Regex.IsMatch(contactInput, phonePattern))
            {
                MessageBox.Show(Lang.InvalidPhone, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Format phone number as 222 - 3434 for database storage
            string formattedContact = $"{contactInput.Substring(0, 3)}-{contactInput.Substring(3, 4)}";

            // Validate time input, restricted to valid hours 0-23 format
            if (!Regex.IsMatch(startTimeInput, @"^([0-1][0-9]|2[0-3]):[0-5][0-9]$") ||
                !Regex.IsMatch(endTimeInput, @"^([0-1][0-9]|2[0-3]):[0-5][0-9]$"))
            {
                MessageBox.Show(Lang.InvalidTimeFormat, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Convert time strings to DateTime
            if (!TimeSpan.TryParse(startTimeInput, out TimeSpan startTime) ||
                !TimeSpan.TryParse(endTimeInput, out TimeSpan endTime))
            {
                MessageBox.Show(Lang.InvalidTimeFormat, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Use EST time zone
            var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime today = DateTime.Today;
            DateTime startDateTime = today.Add(startTime);
            DateTime endDateTime = today.Add(endTime);

            // Business hours convertion (EST)
            var startBusiness = TimeZoneInfo.ConvertTime(startDateTime, estTimeZone);
            var endBusiness = TimeZoneInfo.ConvertTime(endDateTime, estTimeZone);

            // Business hours validation 9AM - 5PM & Monday - Friday
            if (startBusiness.Hour < 9 || startBusiness.Hour >= 17 ||
                endBusiness.Hour < 9 || endBusiness.Hour > 17 ||
                startBusiness.DayOfWeek == DayOfWeek.Saturday || startBusiness.DayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show(Lang.BusinessHoursError, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            // Validate that End time is after Start time
            if (endDateTime <= startDateTime)
            {
                MessageBox.Show(Lang.EndAfterStart, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate URL
            string urlPattern = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";
            if (!Regex.IsMatch(urlInput, urlPattern))
            {
                MessageBox.Show(Lang.InvalidUrl, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Get current username
            var username = SessionManager.Username;
            // Get current userId
            var userId = SessionManager.UserId;

            if (string.IsNullOrEmpty(username) || userId == 0)
            {
                MessageBox.Show(Lang.SessionNotFound, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var appointmentR = new AppointmentRepository();

            // Check for overlapping appointments
            try
            {
                var startUtc = startDateTime.ToUniversalTime();
                var endUtc = endDateTime.ToUniversalTime();
                var overlappingAppointments = appointmentR.GetAppointmentsWithinTimeRange(userId, startUtc, endUtc);

                if (_isEditMode)
                {
                    overlappingAppointments = overlappingAppointments
                        .Where(a => a.AppointmentId != _appointmentToEdit.AppointmentId)
                        .ToList();
                }

                if (overlappingAppointments.Any())
                {
                    MessageBox.Show(Lang.OverlapError, Lang.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Lang.SaveFailed, ex.Message), Lang.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {

                if (_isEditMode)
                {
                    // Update appointment
                    _appointmentToEdit.Title = titleInput;
                    _appointmentToEdit.Description = descriptionInput;
                    _appointmentToEdit.Location = locationInput;
                    _appointmentToEdit.Contact = formattedContact;
                    _appointmentToEdit.Type = typeInput;
                    _appointmentToEdit.Url = urlInput;
                    _appointmentToEdit.Start = startDateTime;
                    _appointmentToEdit.End = endDateTime;
                    _appointmentToEdit.CustomerId = customerId;
                    _appointmentToEdit.LastUpdate = DateTime.UtcNow;
                    _appointmentToEdit.LastUpdateBy = username;



                    appointmentR.EditAppointment(_appointmentToEdit, username);
                    MessageBox.Show(Lang.AppointmentUpdated, Lang.SuccessTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Appointment Object
                    var appointment = new Appointment
                    {
                        CustomerId = customerId,
                        UserId = userId,
                        Title = titleInput,
                        Description = descriptionInput,
                        Location = locationInput,
                        Contact = formattedContact,
                        Type = typeInput,
                        Url = urlInput,
                        Start = startDateTime,
                        End = endDateTime
                    };

                    // Add Appointment
                    appointmentR.AddAppointment(appointment, username);

                    MessageBox.Show(Lang.AppointmentAdded, Lang.SuccessTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                _parentContainer.Content = new AppointmentsView(_parentContainer); ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Lang.SaveFailed, ex.Message), Lang.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
