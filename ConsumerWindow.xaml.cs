using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for ConsumerWindow.xaml
    /// </summary>
    public partial class ConsumerWindow : Window
    {
        public ConsumerWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListConsumer();
            GetListSex();
            CheckStaff();
        }

        public Hanh_Khach selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListConsumer()
        {
            lstConsumer.ItemsSource = ConsumerDAO.Instance.GetListConsumer();
        }

        void GetListSex()
        {
            cbSex.ItemsSource = new List<string>() { "Nam", "Nữ" };
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbSex.SelectedIndex == -1) return;
            ConsumerDAO.Instance.AddNewConsumer(txbID.Text, txbName.Text, txbCM_CC.Text, txbJob.Text, txbPhone.Text,
                                                cbSex.SelectedItem.ToString(), txbMail.Text, dtpkBirthday.SelectedDate);
            GetListConsumer();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbSex.SelectedIndex == -1) return;
            ConsumerDAO.Instance.UpdateConsumer(selectedItem, txbName.Text, txbCM_CC.Text, txbJob.Text, txbPhone.Text,
                                                cbSex.SelectedItem.ToString(), txbMail.Text, dtpkBirthday.SelectedDate);
            GetListConsumer();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ConsumerDAO.Instance.DeleteConsumer(selectedItem);
            GetListConsumer();
        }

        private void lstConsumer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstConsumer.SelectedItem as Hanh_Khach;
            if (selectedItem != null)
            {
                txbID.Text = selectedItem.Ma_hanh_khach;
                txbCM_CC.Text = selectedItem.CMND_CCCD;
                txbJob.Text = selectedItem.nghe_nghiep;
                txbMail.Text = selectedItem.email;
                txbName.Text = selectedItem.ten;
                txbPhone.Text = selectedItem.dien_thoai;
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

        private void btnMagCardWindow_Click(object sender, RoutedEventArgs e)
        {
            MagCardWindow magCardWindow = new MagCardWindow();
            magCardWindow.ShowDialog();
        }
    }
}
