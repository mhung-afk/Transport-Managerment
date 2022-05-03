using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for MonthWindow.xaml
    /// </summary>
    public partial class MonthWindow : Window
    {
        public MonthWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListMonth();
            GetListRoute();
            GetListStop();
            CheckStaff();
        }

        public Ve_thang selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListMonth()
        {
            lstMonth.ItemsSource = MonthDAO.Instance.GetListMonth();
        }

        void GetListRoute()
        {
            cbIDroute.ItemsSource = RouteDAO.Instance.GetListRoute();
            cbIDroute.DisplayMemberPath = "Ma_tuyen";
        }

        void GetListStop()
        {
            cbIDstop1.ItemsSource = StopDAO.Instance.GetListStop();
            cbIDstop1.DisplayMemberPath = "Ma_ga_tram";
            cbIDstop2.ItemsSource = StopDAO.Instance.GetListStop();
            cbIDstop2.DisplayMemberPath = "Ma_ga_tram";
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbIDroute.SelectedIndex == -1 || cbIDstop1.SelectedIndex == -1 || cbIDstop2.SelectedIndex == -1) return;
            MonthDAO.Instance.UpdateMonth(selectedItem, cbIDroute.Text, cbIDstop1.Text, cbIDstop2.Text);
            GetListMonth();
        }

        private void lstMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstMonth.SelectedItem as Ve_thang;
            if (selectedItem != null)
            {
                txbID.Text = selectedItem.Ma_ve;
                cbIDroute.SelectedItem = RouteDAO.Instance.GetRoute(selectedItem.Ma_tuyen);
                cbIDstop1.SelectedItem = StopDAO.Instance.GetStop(selectedItem.Ma_ga_tram_1);
                cbIDstop2.SelectedItem = StopDAO.Instance.GetStop(selectedItem.Ma_ga_tram_2);

                btnUpdate.IsEnabled = true && isStaff;
            }
            else
            {
                btnUpdate.IsEnabled = false;
            }
        }
    }
}
