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
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : UserControl
    {
        private readonly ContentControl _parentContainer;
        private readonly AppointmentRepository _appointmentRepo = new AppointmentRepository();
        private enum ViewMode { AllAppointments, Month, Week }
        private ViewMode _currentViewMode = ViewMode.AllAppointments;
        private DateTime _currentDate = DateTime.Today;
        public CalendarView(ContentControl parentContainer)
        {
            InitializeComponent();
            _parentContainer = parentContainer;

            // Localization
            CalendarTitle.Text = Lang.CalendarTitle;
            AllAppointmentsViewRadio.Content = Lang.AllAppointments;
            MonthViewRadio.Content = Lang.MonthView;
            WeekViewRadio.Content = Lang.WeekView;
            UserName.Header = Lang.UserName;
            CustomerName.Header = Lang.CustomerName;
            Title.Header = Lang.Title;
            StartTime.Header = Lang.StartTime;
            EndTime.Header = Lang.EndTime;
            Back.Content = Lang.Back;

            AllAppointmentsViewRadio.IsChecked = true;
            UpdatePeriodDisplay();
            LoadAppointments();
        }

        private void UpdatePeriodDisplay()
        {
            CultureInfo culture = CultureInfo.CurrentUICulture;
            if (_currentViewMode == ViewMode.AllAppointments)
            {
                CurrentPeriod.Text = Lang.AllAppointments;
            }
            else if (_currentViewMode == ViewMode.Month)
            {
                CurrentPeriod.Text = _currentDate.ToString("MMMM yyyy", culture);
            }
            else
            {
                DateTime startOfWeek = _currentDate.AddDays(-(int)_currentDate.DayOfWeek);
                DateTime endOfWeek = startOfWeek.AddDays(6);
                CurrentPeriod.Text = $"{startOfWeek.ToString("d MMM", culture)} - {endOfWeek.ToString("d MMM yyyy", culture)}";
            }
        }

        private void LoadAppointments()
        {
            try
            {
                var appointments = _appointmentRepo.GetAllAppointments();
                var localTimeZone = TimeZoneInfo.Local;
                CultureInfo culture = CultureInfo.CurrentUICulture;

                // Filter appointments by month or week using lambda
                var filteredAppointments = appointments
                    .Where(a => _currentViewMode == ViewMode.AllAppointments ||
                                (_currentViewMode == ViewMode.Month &&
                                 a.StartLocal.Year == _currentDate.Year &&
                                 a.StartLocal.Month == _currentDate.Month) ||
                                (_currentViewMode == ViewMode.Week &&
                                 a.StartLocal.Date >= _currentDate.AddDays(-(int)_currentDate.DayOfWeek) &&
                                 a.StartLocal.Date <= _currentDate.AddDays(6 - (int)_currentDate.DayOfWeek)))
                    .Select(a => new
                    {
                        UserName = a.UserName,
                        CustomerName = a.CustomerName,
                        Title = a.Title,
                        StartTime = a.StartLocal.ToString(culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.ShortTimePattern),
                        EndTime = a.EndLocal.ToString(culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.ShortTimePattern)
                    })
                    .OrderBy(a => a.StartTime)
                    .ToList();

                AppointmentsGrid.ItemsSource = filteredAppointments;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Lang.ErrorLoadingCalendar, ex.Message), Lang.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (AllAppointmentsViewRadio.IsChecked == true)
            {
                _currentViewMode = ViewMode.AllAppointments;
            }
            else if (MonthViewRadio.IsChecked == true)
            {
                _currentViewMode = ViewMode.Month;
            }
            else if (WeekViewRadio.IsChecked == true)
            {
                _currentViewMode = ViewMode.Week;
            }
            UpdatePeriodDisplay();
            LoadAppointments();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _parentContainer.Content = new DashboardOptions(_parentContainer);
        }
    }
}
