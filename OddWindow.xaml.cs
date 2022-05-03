using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for OddWindow.xaml
    /// </summary>
    public partial class OddWindow : Window
    {
        public OddWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListOdd();
            GetListRoute();
            GetListStop();
            CheckStaff();
        }

        public Ve_le selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListOdd()
        {
            var list = OddDAO.Instance.GetListOdd();
            lstOdd.ItemsSource = list.OrderBy(i => i.Ngay_su_dung).ToList();
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
            OddDAO.Instance.UpdateOdd(selectedItem, cbIDroute.Text, dtpkDate.SelectedDate, cbIDstop1.Text, cbIDstop2.Text, tpkCome.SelectedTime, tpkLeave.SelectedTime);
            GetListOdd();
        }

        //private void btnDelete_Click(object sender, RoutedEventArgs e)
        //{
        //    OddDAO.Instance.DeleteOdd(selectedItem);
        //    GetListOdd();
        //}

        private void lstOdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstOdd.SelectedItem as Ve_le;
            if (selectedItem != null)
            {
                txbID.Text = selectedItem.Ma_ve;
                cbIDroute.SelectedItem = RouteDAO.Instance.GetRoute(selectedItem.Ma_tuyen);
                dtpkDate.SelectedDate = selectedItem.Ngay_su_dung;
                cbIDstop1.SelectedItem = StopDAO.Instance.GetStop(selectedItem.Ma_ga_tram_len);
                cbIDstop2.SelectedItem = StopDAO.Instance.GetStop(selectedItem.Ma_ga_tram_xuong);

                tpkCome.SelectedTime = new DateTime() + selectedItem.Gio_len;
                tpkLeave.SelectedTime = new DateTime() + selectedItem.Gio_xuong;

                btnUpdate.IsEnabled = true && isStaff;
                //btnDelete.IsEnabled = true && isStaff;
            }
            else
            {
                btnUpdate.IsEnabled = false;
                //btnDelete.IsEnabled = false;
            }
        }
    }
}
