using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for CrossWindow.xaml
    /// </summary>
    public partial class CrossWindow : Window
    {
        public CrossWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListCross();
            CheckStaff();
        }

        public Giao_lo selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListCross()
        {
            var list = CrossDAO.Instance.GetListCross();
            lstCross.ItemsSource = list.OrderBy(i => i.Ma_giao_lo.Length).ThenBy(i => i.Ma_giao_lo).ToList();
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(lstCross.ItemsSource);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string[] pos = null;
            //try
            //{
            //    pos = txbPos.Text.Split(':');
            //    CrossDAO.Instance.AddNewCross(Convert.ToDouble(pos[0]), Convert.ToDouble(pos[1]));
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Toạ độ phải có định dạng: \"x:y\"");
            //    return;
            //}
            pos = txbPos.Text.Split(':');
            CrossDAO.Instance.AddNewCross(Convert.ToDouble(pos[0]), Convert.ToDouble(pos[1]));
            GetListCross();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string[] pos = null;
            //try
            //{
            //    pos = txbPos.Text.Split(':');
            //    CrossDAO.Instance.UpdateCross(selectedItem, Convert.ToDouble(pos[0]), Convert.ToDouble(pos[1]));
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Toạ độ phải có định dạng: \"x:y\"");
            //    return;
            //}
            pos = txbPos.Text.Split(':');
            CrossDAO.Instance.UpdateCross(selectedItem, Convert.ToDouble(pos[0]), Convert.ToDouble(pos[1]));
            GetListCross();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            CrossDAO.Instance.DeleteCross(selectedItem);
            GetListCross();
        }

        private void lstCross_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstCross.SelectedItem as Giao_lo;
            if (selectedItem != null)
            {
                txbIDcross.Text = selectedItem.Ma_giao_lo;
                txbPos.Text = string.Format("{0}:{1}", selectedItem.@long, selectedItem.lat);

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
