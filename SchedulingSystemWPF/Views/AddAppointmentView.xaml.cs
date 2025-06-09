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
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _parentContainer.Content = new DashboardOptions(_parentContainer);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add new");
        }
    }
}
