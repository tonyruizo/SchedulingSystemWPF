using SchedulingSystemWPF.DatabaseAccess;
using SchedulingSystemWPF.Resources;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SchedulingSystemWPF.Views
{
    /// <summary>
    /// Interaction logic for ReportsView.xaml
    /// </summary>
    public partial class ReportsView : UserControl
    {
        private readonly ContentControl _parentContainer;
        private readonly AppointmentRepository _appointmentRepo = new AppointmentRepository();
        public ReportsView(ContentControl parentContainer)
        {
            InitializeComponent();
            _parentContainer = parentContainer;

            // Backend Localization due to x:Static binding XAML issues 
            BackButton.Content = Lang.Back;
            ReportsTitle.Text = Lang.ReportsTitle;

            AppointmentsTypesByMonth.Text = Lang.AppointmentTypesByMonth;
            Month.Header = Lang.Month;
            AppointmentType.Header = Lang.AppointmentType;
            Count.Header = Lang.Count;

            UserSchedule.Text = Lang.UserSchedule;
            UserName.Header = Lang.UserName;
            CustomerName.Header = Lang.CustomerName;
            AppointmentTitle.Header = Lang.Title;
            AppointmentStart.Header = Lang.StartTime;
            AppointmentEnd.Header = Lang.EndTime;

            CustomerAppointmentCount.Text = Lang.CustomerAppointmentCount;
            AppointmentCount.Header = Lang.AppointmentCount;

            LoadReports();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _parentContainer.Content = new DashboardOptions(_parentContainer);
        }

        private void LoadReports()
        {
            CultureInfo culture = CultureInfo.CurrentUICulture;
            var localTimeZone = TimeZoneInfo.Local;

            try
            {
                var appointments = _appointmentRepo.GetAllAppointments();

                // Report 1: Appointment Types by Month
                var appointmentTypesByMonth = appointments
                    .GroupBy(a => new
                    {
                        Month = TimeZoneInfo.ConvertTimeFromUtc(a.Start, localTimeZone).ToString("MMMM yyyy", culture),
                        Type = a.Type
                    }, a => a)
                    .Select(g => new
                    {
                        Month = g.Key.Month,
                        Type = g.Key.Type,
                        Count = g.Count()
                    })
                    .OrderBy(r => r.Month)
                    .ThenBy(r => r.Type)
                    .ToList();
                AppointmentTypesGrid.ItemsSource = appointmentTypesByMonth;

                // Report 2: User Schedule
                var userSchedule = appointments
                    .Select(a => new
                    {
                        UserName = a.UserName,
                        CustomerName = a.CustomerName,
                        Title = a.Title,
                        StartTime = TimeZoneInfo.ConvertTimeFromUtc(a.Start, localTimeZone).ToString(culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.ShortTimePattern),
                        EndTime = TimeZoneInfo.ConvertTimeFromUtc(a.End, localTimeZone).ToString(culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.ShortTimePattern)
                    })
                    .OrderBy(a => a.UserName)
                    .ThenBy(a => a.StartTime)
                    .ToList();
                UserScheduleGrid.ItemsSource = userSchedule;

                // Report 3: Customer Appointment Count
                var customerAppointmentCount = appointments
                    .GroupBy(a => a.CustomerName, a => a)
                    .Select(g => new
                    {
                        CustomerName = g.Key,
                        AppointmentCount = g.Count()
                    })
                    .OrderBy(r => r.CustomerName)
                    .ToList();
                AppointmentCountGrid.ItemsSource = customerAppointmentCount;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Lang.ErrorLoadingReports, ex.Message), Lang.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
