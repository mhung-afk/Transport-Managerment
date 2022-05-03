using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for StationStationStaff.xaml
    /// </summary>
    public partial class StationStaffWindow : Window
    {
        public StationStaffWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListStationStaff();
            GetListStaff();
            GetListStop();
            CheckStationStaff();
        }

        public Ga_Tram_Lam_Viec selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStationStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListStationStaff()
        {
            lstStationStaff.ItemsSource = StationStaffDAO.Instance.GetListStationStaff();
        }

        void GetListStaff()
        {
            cbID.ItemsSource = StaffDAO.Instance.GetListStaff();
            cbID.DisplayMemberPath = "Ma_nhan_vien";
        }

        void GetListStop()
        {
            cbStop.ItemsSource = StopDAO.Instance.GetListStop();
            cbStop.DisplayMemberPath = "Ma_ga_tram";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbID.SelectedIndex == -1 || cbStop.SelectedIndex == -1) return;
            StationStaffDAO.Instance.AddNewStationStaff(cbID.Text, cbStop.Text);
            GetListStationStaff();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbStop.SelectedIndex == -1) return;
            StationStaffDAO.Instance.UpdateStationStaff(selectedItem, cbStop.Text);
            GetListStationStaff();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            StationStaffDAO.Instance.DeleteStationStaff(selectedItem);
            GetListStationStaff();
        }

        private void lstStationStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstStationStaff.SelectedItem as Ga_Tram_Lam_Viec;
            if (selectedItem != null)
            {
                cbID.SelectedItem = StaffDAO.Instance.GetStaff(selectedItem.Ma_nhan_vien);
                cbStop.SelectedItem = StopDAO.Instance.GetStop(selectedItem.Ma_ga_tram);

                btnUpdate.IsEnabled = true && isStaff;
                btnDelete.IsEnabled = true && isStaff;
            }
            else
            {
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }
    }
}
