using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for StopWindow.xaml
    /// </summary>
    public partial class StopWindow : Window
    {
        public StopWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListStop();
            GetListCross();
            GetListTypeTransport();
            CheckStaff();
        }

        public Ga_Tram selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListCross()
        {
            cbCross1.ItemsSource = CrossDAO.Instance.GetListCross();
            cbCross1.DisplayMemberPath = "Ma_giao_lo";
            cbCross2.ItemsSource = CrossDAO.Instance.GetListCross();
            cbCross2.DisplayMemberPath = "Ma_giao_lo";
        }

        void GetListStop()
        {
            lstStop.ItemsSource = StopDAO.Instance.GetListStop();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstStop.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Ma_con_duong", ListSortDirection.Ascending));
        }

        void GetListTypeTransport()
        {
            cbType.ItemsSource = new List<string>() { "Trạm xe buýt", "Ga tàu điện" };
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbCross1.SelectedIndex == -1 || cbCross2.SelectedIndex == -1 || cbType.SelectedIndex == -1) return;
            string cross1 = (cbCross1.SelectedItem as Giao_lo).Ma_giao_lo;
            string cross2 = (cbCross2.SelectedItem as Giao_lo).Ma_giao_lo;
            byte type = Convert.ToByte(cbType.SelectedIndex);

            StopDAO.Instance.AddNewStop(txbID.Text, txbName.Text, txbAddr.Text, type, cross1, cross2);
            GetListStop();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbCross1.SelectedIndex == -1 || cbCross2.SelectedIndex == -1 || cbType.SelectedIndex == -1) return;
            string cross1 = (cbCross1.SelectedItem as Giao_lo).Ma_giao_lo;
            string cross2 = (cbCross2.SelectedItem as Giao_lo).Ma_giao_lo;

            StopDAO.Instance.UpdateStop(selectedItem, txbName.Text, txbAddr.Text, cross1, cross2);
            GetListStop();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            StopDAO.Instance.DeleteStop(selectedItem);
            GetListStop();
        }

        private void lstStop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstStop.SelectedItem as Ga_Tram;
            if (selectedItem != null)
            {
                cbCross1.SelectedItem = CrossDAO.Instance.GetCross(selectedItem.ma_giao_lo_1);
                cbCross2.SelectedItem = CrossDAO.Instance.GetCross(selectedItem.ma_giao_lo_2);
                txbID.Text = selectedItem.Ma_ga_tram;
                txbName.Text = selectedItem.ten;
                txbAddr.Text = selectedItem.dia_chi;
                if (selectedItem.Ga_Tram1 == 0) cbType.SelectedIndex = 0;
                if (selectedItem.Ga_Tram1 == 1) cbType.SelectedIndex = 1;

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
