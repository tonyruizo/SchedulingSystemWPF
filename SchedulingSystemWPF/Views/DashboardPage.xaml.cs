using System.Windows.Controls;

namespace SchedulingSystemWPF.Views
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public DashboardPage(string username)
        {
            InitializeComponent();
            DataContext = new ViewModels.DashboardViewModel(username);
        }
    }
}
