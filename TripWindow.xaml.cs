using System;
using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for TripWindow.xaml
    /// </summary>
    public partial class TripWindow : Window
    {
        public TripWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListTrip();
            GetListRoute();
            GetListStop();
            CheckStaff();
        }

        public Chuyen_tau_xe_ghe_ga_tram selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListTrip()
        {
            lstTrip.ItemsSource = TripDAO.Instance.GetListTrip();
        }

        void GetListRoute()
        {
            cbIDroute.ItemsSource = RouteDAO.Instance.GetListRoute();
            cbIDroute.DisplayMemberPath = "Ma_tuyen";
        }

        void GetListStop()
        {
            cbIDstop.ItemsSource = StopDAO.Instance.GetListStop();
            cbIDstop.DisplayMemberPath = "Ma_ga_tram";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbIDroute.SelectedIndex != -1 && cbIDstop.SelectedIndex != -1 && !string.IsNullOrEmpty(txbSTTstop.Text) && !string.IsNullOrEmpty(txbSTTtrip.Text))
            {
                var t1 = cbIDroute.SelectedItem as Tuyen_tau_xe;
                var t2 = cbIDstop.SelectedItem as Ga_Tram;
                TripDAO.Instance.AddNewTrip(t1.Ma_tuyen, Convert.ToByte(txbSTTtrip.Text),
                                            t2.Ma_ga_tram, Convert.ToByte(txbSTTstop.Text),
                                            tpkCome.SelectedTime, tpkLeave.SelectedTime);
                GetListTrip();
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txbSTTstop.Text) && !string.IsNullOrEmpty(txbSTTtrip.Text))
            {
                var t2 = cbIDstop.SelectedItem as Ga_Tram;
                TripDAO.Instance.UpdateTrip(selectedItem, Convert.ToByte(txbSTTstop.Text),
                                            tpkCome.SelectedTime, tpkLeave.SelectedTime);
                GetListTrip();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            TripDAO.Instance.DeleteTrip(selectedItem);
            GetListTrip();
        }

        private void lstTrip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstTrip.SelectedItem as Chuyen_tau_xe_ghe_ga_tram;
            if (selectedItem != null)
            {
                cbIDroute.SelectedItem = RouteDAO.Instance.GetRoute(selectedItem.Ma_tuyen);
                txbSTTtrip.Text = selectedItem.STT_chuyen.ToString();
                cbIDstop.SelectedItem = StopDAO.Instance.GetStop(selectedItem.Ma_ga_tram);
                txbSTTstop.Text = selectedItem.STT_tram.ToString();

                tpkCome.SelectedTime = new DateTime() + selectedItem.gio_ghe;
                tpkLeave.SelectedTime = new DateTime() + selectedItem.gio_di;

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
