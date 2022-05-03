using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for DayWindow.xaml
    /// </summary>
    public partial class DayWindow : Window
    {
        public DayWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListDay();
            CheckStaff();
        }

        public Ve_1_ngay selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListDay()
        {
            var list = DayDAO.Instance.GetListDay();
            lstDay.ItemsSource = list.OrderBy(i => i.Ngay_su_dung).ToList();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            DayDAO.Instance.UpdateDay(selectedItem, dtpkDate.SelectedDate);
            GetListDay();
        }

        private void lstDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstDay.SelectedItem as Ve_1_ngay;
            if (selectedItem != null)
            {
                txbID.Text = selectedItem.Ma_ve;
                dtpkDate.SelectedDate = selectedItem.Ngay_su_dung;

                btnUpdate.IsEnabled = true && isStaff;
            }
            else
            {
                btnUpdate.IsEnabled = false;
            }
        }
    }
}
