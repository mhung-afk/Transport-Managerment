using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for DayActWindow.xaml
    /// </summary>
    public partial class DayActWindow : Window
    {
        public DayActWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListDayAct();
            GetListDay();
            GetListStop();
            GetListRoute();
            CheckStaff();
        }

        public HD_ve_ngay selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListDayAct()
        {
            var list = DayActDAO.Instance.GetListDayAct();
            lstDayAct.ItemsSource = list.OrderBy(i => i.Ma_ve).ThenBy(i => i.STT).ToList();
        }

        void GetListDay()
        {
            cbID.ItemsSource = TicketDAO.Instance.GetListDay();
            cbID.DisplayMemberPath = "Ma_ve";
        }

        void GetListStop()
        {
            cbIDstop1.ItemsSource = StopDAO.Instance.GetListStop();
            cbIDstop1.DisplayMemberPath = "Ma_ga_tram";
            cbIDstop2.ItemsSource = StopDAO.Instance.GetListStop();
            cbIDstop2.DisplayMemberPath = "Ma_ga_tram";
        }

        void GetListRoute()
        {
            cbIDroute.ItemsSource = RouteDAO.Instance.GetListRoute();
            cbIDroute.DisplayMemberPath = "Ma_tuyen";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbID.SelectedIndex == -1 || cbIDstop1.SelectedIndex == -1 || cbIDstop2.SelectedIndex == -1 || cbIDroute.SelectedIndex == -1) return;
            DayActDAO.Instance.AddDayAct(cbID.Text, cbIDroute.Text, cbIDstop1.Text, cbIDstop2.Text, tpkCome.SelectedTime, tpkLeave.SelectedTime);
            GetListDayAct();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbIDstop1.SelectedIndex == -1 || cbIDstop2.SelectedIndex == -1) return;
            DayActDAO.Instance.UpdateDayAct(selectedItem, cbIDroute.Text, cbIDstop1.Text, cbIDstop2.Text, tpkCome.SelectedTime, tpkLeave.SelectedTime);
            GetListDayAct();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DayActDAO.Instance.DeleteDayAct(selectedItem);
            GetListDayAct();
        }

        private void lstDayAct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstDayAct.SelectedItem as HD_ve_ngay;
            if (selectedItem != null)
            {
                cbID.SelectedItem = TicketDAO.Instance.GetTicket(selectedItem.Ma_ve);
                txbSTT.Text = selectedItem.STT.ToString();
                cbIDroute.SelectedItem = RouteDAO.Instance.GetRoute(selectedItem.Ma_tuyen);
                cbIDstop1.SelectedItem = StopDAO.Instance.GetStop(selectedItem.Ma_ga_tram_len);
                cbIDstop2.SelectedItem = StopDAO.Instance.GetStop(selectedItem.Ma_ga_tram_xuong);

                tpkCome.SelectedTime = new DateTime() + selectedItem.Gio_len;
                tpkLeave.SelectedTime = new DateTime() + selectedItem.Gio_xuong;

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
