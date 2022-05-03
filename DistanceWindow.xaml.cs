using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for DistanceWindow.xaml
    /// </summary>
    public partial class DistanceWindow : Window
    {
        public DistanceWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListDistance();
            GetListRoad();
            GetListCross();
            CheckStaff();
        }

        public Doan_duong selectedItem { get; set; }
        public bool isStaff { get; set; }

        void GetListCross()
        {
            cbCross1.ItemsSource = CrossDAO.Instance.GetListCross();
            cbCross1.DisplayMemberPath = "Ma_giao_lo";
            cbCross2.ItemsSource = CrossDAO.Instance.GetListCross();
            cbCross2.DisplayMemberPath = "Ma_giao_lo";
        }

        void GetListRoad()
        {
            cbRoad.ItemsSource = RoadDAO.Instance.GetListRoad();
            cbRoad.DisplayMemberPath = "Ma_con_duong";
        }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListDistance()
        {
            lstDistance.ItemsSource = DistanceDAO.Instance.GetListDistance();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstDistance.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Ma_con_duong", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("STT", ListSortDirection.Ascending));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbCross1.SelectedIndex == -1 || cbCross2.SelectedIndex == -1 || cbRoad.SelectedIndex == -1) return;
            string cross1 = (cbCross1.SelectedItem as Giao_lo).Ma_giao_lo;
            string cross2 = (cbCross2.SelectedItem as Giao_lo).Ma_giao_lo;
            string road = (cbRoad.SelectedItem as Con_duong).Ma_con_duong;
            int STT;
            bool check = int.TryParse(txbSTT.Text, out STT);
            if (!check) STT = 1;
            DistanceDAO.Instance.AddNewDistance(cross1, cross2, road, STT);
            GetListDistance();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int STT;
            if (cbRoad.SelectedIndex == -1) return;
            string road = (cbRoad.SelectedItem as Con_duong).Ma_con_duong;
            bool check = int.TryParse(txbSTT.Text, out STT);
            if (!check) STT = 1;
            DistanceDAO.Instance.UpdateDistance(selectedItem, road, STT);
            GetListDistance();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DistanceDAO.Instance.DeleteDistance(selectedItem);
            GetListDistance();
        }

        private void lstDistance_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstDistance.SelectedItem as Doan_duong;
            if (selectedItem != null)
            {
                cbRoad.SelectedItem = RoadDAO.Instance.GetRoad(selectedItem.Ma_con_duong);
                cbCross1.SelectedItem = CrossDAO.Instance.GetCross(selectedItem.Ma_giao_lo_1);
                cbCross2.SelectedItem = CrossDAO.Instance.GetCross(selectedItem.Ma_giao_lo_2);
                txbSTT.Text = selectedItem.STT.ToString();

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
