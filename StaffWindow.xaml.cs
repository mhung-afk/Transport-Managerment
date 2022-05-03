using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : Window
    {
        public StaffWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListStaff();
            GetListSex();
            CheckStaff();
        }

        public Nhan_Vien selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListStaff()
        {
            lstStaff.ItemsSource = StaffDAO.Instance.GetListStaff();
        }

        void GetListSex()
        {
            cbSex.ItemsSource = new List<string>() { "Nam", "Nữ" };
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbSex.SelectedIndex == -1) return;
            StaffDAO.Instance.AddNewStaff(txbID.Text, txbName.Text, txbJob.Text, dtpkBirthday.SelectedDate,
                                            txbMail.Text, cbSex.SelectedItem.ToString(), txbTelePhone.Text, txbLocalPhone.Text);
            GetListStaff();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbSex.SelectedIndex == -1) return;
            StaffDAO.Instance.UpdateStaff(selectedItem, txbName.Text, txbJob.Text, dtpkBirthday.SelectedDate,
                                            txbMail.Text, cbSex.SelectedItem.ToString(), txbTelePhone.Text, txbLocalPhone.Text);
            GetListStaff();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            StaffDAO.Instance.DeleteStaff(selectedItem);
            GetListStaff();
        }

        private void lstStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstStaff.SelectedItem as Nhan_Vien;
            if (selectedItem != null)
            {
                txbID.Text = selectedItem.Ma_nhan_vien;
                txbJob.Text = selectedItem.loai_cong_viec;
                txbMail.Text = selectedItem.email;
                txbName.Text = selectedItem.ten;
                txbLocalPhone.Text = selectedItem.dien_thoai_noi_bo;
                txbTelePhone.Text = selectedItem.dien_thoai_di_dong;
                dtpkBirthday.SelectedDate = selectedItem.ngay_sinh;
                if (selectedItem.gioi_tinh == "M") cbSex.SelectedIndex = 0;
                if (selectedItem.gioi_tinh == "F") cbSex.SelectedIndex = 1;

                btnUpdate.IsEnabled = true && isStaff;
                btnDelete.IsEnabled = true && isStaff;
            }
            else
            {
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void btnStationStaff_Click(object sender, RoutedEventArgs e)
        {
            StationStaffWindow window = new StationStaffWindow();
            window.ShowDialog();
        }
    }
}
