using System;
using System.Collections.Generic;
using System.Windows;
using TransportManagerment.DataAccess;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for PriceTableWindow.xaml
    /// </summary>
    public partial class PriceTableWindow : Window
    {
        public PriceTableWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetPrice();
            CheckStaff();
        }

        public bool isStaff { get; set; }
        public int? busprice { get; set; }
        public int? buspriceNor { get; set; }
        public int? buspriceWeekend { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetPrice()
        {
            List<int?> t = PriceTableDAO.Instance.GetPrice();
            busprice = t[0];
            buspriceNor = t[1];
            buspriceWeekend = t[2];
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PriceTableDAO.Instance.UpdatePrice(Convert.ToInt32(txb0.Text), Convert.ToInt32(txb1.Text), Convert.ToInt32(txb2.Text));
                MessageBox.Show("Đã cập nhật thành công");
                GetPrice();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi nhập dữ liệu");
                return;
            }

        }
    }
}
