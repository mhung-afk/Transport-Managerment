using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for TicketWindow.xaml
    /// </summary>
    public partial class TicketWindow : Window
    {
        public TicketWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListTicket();
            GetListType();
            CheckStaff();
        }

        public Ve selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListTicket()
        {
            var list = TicketDAO.Instance.GetListTicket();
            lstTicket.ItemsSource = list.OrderBy(i => i.Loai_ve).ThenByDescending(i => i.Ngay_gio_mua).ToList();
        }

        void GetListType()
        {
            cbType.ItemsSource = new List<string>() { "Vé lẻ", "Vé tháng", "Vé ngày" };
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbType.SelectedIndex == -1) return;
            TicketDAO.Instance.AddNewTicket(txbID.Text, cbType.Text, dtpkPurchaseDay.SelectedDate, txbIDcus.Text);
            GetListTicket();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbType.SelectedIndex == -1) return;
            TicketDAO.Instance.UpdateTicket(selectedItem, cbType.Text, dtpkPurchaseDay.SelectedDate, txbIDcus.Text);
            GetListTicket();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            TicketDAO.Instance.DeleteTicket(selectedItem);
            GetListTicket();
        }

        private void lstTicket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstTicket.SelectedItem as Ve;
            if (selectedItem != null)
            {
                txbID.Text = selectedItem.Ma_ve;
                if (selectedItem.Loai_ve == 0) cbType.SelectedIndex = 0;
                if (selectedItem.Loai_ve == 1) cbType.SelectedIndex = 1;
                if (selectedItem.Loai_ve == 2) cbType.SelectedIndex = 2;
                txbPrice.Text = selectedItem.Gia_ve.ToString();
                txbIDcus.Text = selectedItem.Ma_khach_hang;
                dtpkPurchaseDay.SelectedDate = selectedItem.Ngay_gio_mua;

                btnUpdate.IsEnabled = true && isStaff;
                btnDelete.IsEnabled = true && isStaff;
            }
            else
            {
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void btnOddWindow_Click(object sender, RoutedEventArgs e)
        {
            OddWindow oddWindow = new OddWindow();
            oddWindow.ShowDialog();
            DataProvider.Instance.ResetConnection();
            GetListTicket();
        }

        private void btnDayWindow_Click(object sender, RoutedEventArgs e)
        {
            DayWindow dayWindow = new DayWindow();
            dayWindow.ShowDialog();
            DataProvider.Instance.ResetConnection();
            GetListTicket();
        }

        private void btnMonthWindow_Click(object sender, RoutedEventArgs e)
        {
            MonthWindow monthWindow = new MonthWindow();
            monthWindow.ShowDialog();
            DataProvider.Instance.ResetConnection();
            GetListTicket();
        }

        private void btnDayActWindow_Click(object sender, RoutedEventArgs e)
        {
            DayActWindow dayActWindow = new DayActWindow();
            dayActWindow.ShowDialog();
        }

        private void btnMonthActWindow_Click(object sender, RoutedEventArgs e)
        {
            MonthActWindow monthActWindow = new MonthActWindow();
            monthActWindow.ShowDialog();
        }
    }
}
