using SchedulingSystemWPF.DatabaseAccess;
using SchedulingSystemWPF.Models;
using SchedulingSystemWPF.Resources;
using SchedulingSystemWPF.ViewModels;
using System;
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
            if (AppointmentGrid.SelectedItem is AppointmentsViewModel selectedViewModel)
            {
                var appointment = new Appointment
                {
                    AppointmentId = selectedViewModel.AppointmentId,
                    CustomerId = selectedViewModel.CustomerId,
                    UserId = selectedViewModel.UserId,
                    Title = selectedViewModel.Title,
                    Description = selectedViewModel.Description,
                    Location = selectedViewModel.Location,
                    Contact = selectedViewModel.Contact,
                    Type = selectedViewModel.Type,
                    Url = selectedViewModel.Url,
                    Start = selectedViewModel.StartLocal,
                    End = selectedViewModel.EndLocal,
                    CreateDate = selectedViewModel.CreateDate,
                    CreatedBy = selectedViewModel.CreatedBy,
                    LastUpdate = selectedViewModel.LastUpdate,
                    LastUpdateBy = selectedViewModel.LastUpdateBy
                };

                _parentContainer.Content = new AddAppointmentView(_parentContainer, appointment);
            }
            else
            {
                MessageBox.Show(Lang.NoAppointmentSelected, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AppointmentGrid.SelectedItem is AppointmentsViewModel selectedAppointment)
            {
                int appointmentId = selectedAppointment.AppointmentId;

                var confirmDelete = MessageBox.Show(Lang.ConfirmDelete, Lang.ConfirmDeleteTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (confirmDelete == MessageBoxResult.Yes)
                {
                    var appointmentR = new AppointmentRepository();

                    try
                    {
                        appointmentR.DeleteAppointment(appointmentId);

                        // Refresh Grid
                        AppointmentGrid.ItemsSource = appointmentR.GetAllAppointments();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format(Lang.DeleteFailed, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error));
                    }
                }
            }
            else
            {
                MessageBox.Show(Lang.NoAppointmentSelected, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
