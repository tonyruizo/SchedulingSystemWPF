using SchedulingSystemWPF.DatabaseAccess;
using SchedulingSystemWPF.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SchedulingSystemWPF.Views
{
    /// <summary>
    /// Interaction logic for AppointmentsView.xaml
    /// </summary>
    public partial class AppointmentsView : UserControl
    {
        private readonly AppointmentRepository _appointmentRepo = new AppointmentRepository();
        private readonly ContentControl _parentContainer;
        public AppointmentsView(ContentControl parentContainer)
        {
            InitializeComponent();
            _parentContainer = parentContainer;
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            List<AppointmentsViewModel> appointments = _appointmentRepo.GetAllAppointments();
            AppointmentGrid.ItemsSource = appointments;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _parentContainer.Content = new DashboardOptions(_parentContainer);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _parentContainer.Content = new AddAppointmentView(_parentContainer);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit appointment");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AppointmentGrid.SelectedItem is AppointmentsViewModel selectedAppointment)
            {
                int appointmentId = selectedAppointment.AppointmentId;

                var confirmDelete = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (confirmDelete == MessageBoxResult.Yes)
                {
                    var appointmentR = new AppointmentRepository();

                    appointmentR.DeleteAppointment(appointmentId);

                    // Refresh Grid
                    AppointmentGrid.ItemsSource = appointmentR.GetAllAppointments();
                }
            }
            else
            {
                MessageBox.Show("Please select an appointment to delete.");
            }
        }


    }
}
