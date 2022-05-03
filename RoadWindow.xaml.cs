using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for RoadWindow.xaml
    /// </summary>
    public partial class RoadWindow : Window
    {
        public RoadWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListRoad();
            CheckStaff();
        }

        public Con_duong selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListRoad()
        {
            //lstRoad.ItemsSource = RoadDAO.Instance.GetListRoad();
            //ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(lstRoad.ItemsSource);

            //view.SortDescriptions.Add(new SortDescription("Ma_con_duong", ListSortDirection.Ascending));
            var list = RoadDAO.Instance.GetListRoad();
            lstRoad.ItemsSource = list.OrderBy(i => i.Ma_con_duong.Length).ThenBy(i => i.Ma_con_duong).ToList();
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(lstRoad.ItemsSource);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    RoadDAO.Instance.AddNewRoad(txbNameRoad.Text);
            //    GetListRoad();
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Lỗi");
            //}
            RoadDAO.Instance.AddNewRoad(txbNameRoad.Text);
            GetListRoad();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    RoadDAO.Instance.UpdateRoad(selectedItem, txbNameRoad.Text);
            //    GetListRoad();
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Lỗi");
            //}
            RoadDAO.Instance.UpdateRoad(selectedItem, txbNameRoad.Text);
            GetListRoad();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    RoadDAO.Instance.DeleteRoad(selectedItem);
            //    GetListRoad();
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Lỗi");
            //}
            RoadDAO.Instance.DeleteRoad(selectedItem);
            GetListRoad();
        }

        private void lstRoad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstRoad.SelectedItem as Con_duong;
            if (selectedItem != null)
            {
                txbIDroad.Text = selectedItem.Ma_con_duong;
                txbNameRoad.Text = selectedItem.Ten_duong;

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
