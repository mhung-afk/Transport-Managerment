using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for MonthActWindow.xaml
    /// </summary>
    public partial class MonthActWindow : Window
    {
        public MonthActWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListMonthAct();
            GetListMonth();
            CheckStaff();
        }

        public HD_ve_thang selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListMonthAct()
        {
            var list = MonthActDAO.Instance.GetListMonthAct();
            lstMonthAct.ItemsSource = list.OrderBy(i => i.Ngay_su_dung).ToList();
        }

        void GetListMonth()
        {
            cbID.ItemsSource = TicketDAO.Instance.GetListMonth();
            cbID.DisplayMemberPath = "Ma_ve";
        }

        void GetListStop(string ID)
        {
            cbIDstop1.ItemsSource = MonthDAO.Instance.GetStopByIdTicket(ID);
            cbIDstop1.DisplayMemberPath = "Ma_ga_tram";
            cbIDstop2.ItemsSource = MonthDAO.Instance.GetStopByIdTicket(ID);
            cbIDstop2.DisplayMemberPath = "Ma_ga_tram";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbID.SelectedIndex == -1 || dtpkDate.SelectedDate == null || tpkCome.SelectedTime == null
                || cbIDstop1.SelectedIndex == -1 || cbIDstop2.SelectedIndex == -1) return;
            MonthActDAO.Instance.AddMonthAct(cbID.Text, dtpkDate.SelectedDate.Value, cbIDstop1.Text, cbIDstop2.Text, tpkCome.SelectedTime.Value, tpkLeave.SelectedTime);
            GetListMonthAct();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbIDstop1.SelectedIndex == -1 || cbIDstop2.SelectedIndex == -1) return;
            MonthActDAO.Instance.UpdateMonthAct(selectedItem, cbIDstop1.Text, cbIDstop2.Text, tpkLeave.SelectedTime);
            GetListMonthAct();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MonthActDAO.Instance.DeleteMonthAct(selectedItem);
            GetListMonthAct();
        }

        private void lstMonthAct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstMonthAct.SelectedItem as HD_ve_thang;
            if (selectedItem != null)
            {
                cbID.SelectedItem = TicketDAO.Instance.GetTicket(selectedItem.Ma_ve);
                dtpkDate.SelectedDate = selectedItem.Ngay_su_dung;
                cbIDstop1.SelectedItem = StopDAO.Instance.GetStop(selectedItem.Ga_tram_len);
                cbIDstop2.SelectedItem = StopDAO.Instance.GetStop(selectedItem.Ga_tram_xuong);

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

        private void cbIDstop1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbIDstop1.SelectedIndex == 0)
            {
                cbIDstop2.SelectedIndex = 1;
            }
            else if (cbIDstop1.SelectedIndex == 1)
            {
                cbIDstop2.SelectedIndex = 0;
            }
        }

        private void cbIDstop2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbIDstop2.SelectedIndex == 0)
            {
                cbIDstop1.SelectedIndex = 1;
            }
            else if (cbIDstop2.SelectedIndex == 1)
            {
                cbIDstop1.SelectedIndex = 0;
            }
        }

        private void cbID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetListStop((cbID.SelectedItem as Ve).Ma_ve);
        }
    }
}
