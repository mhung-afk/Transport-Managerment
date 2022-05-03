using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for BusWindow.xaml
    /// </summary>
    public partial class BusWindow : Window
    {
        public BusWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListBus();
            CheckStaff();
        }

        public Tuyen_xe_bus selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListBus()
        {
            lstBus.ItemsSource = BusDAO.Instance.GetListBus();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            BusDAO.Instance.AddNewBus(txbIDbus.Text);
            GetListBus();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            BusDAO.Instance.DeleteBus(selectedItem);
            GetListBus();
        }

        private void lstBus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstBus.SelectedItem as Tuyen_xe_bus;
            if (selectedItem != null)
            {
                txbIDbus.Text = selectedItem.Ma_tuyen_xe_bus;

                btnDelete.IsEnabled = true && isStaff;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
        }
    }
}
