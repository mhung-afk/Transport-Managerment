using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for MagCardWindow.xaml
    /// </summary>
    public partial class MagCardWindow : Window
    {
        public MagCardWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListMagCard();
            CheckStaff();
        }

        public The_Tu selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListMagCard()
        {
            lstMagCard.ItemsSource = MagCardDAO.Instance.GetListMagCard();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MagCardDAO.Instance.AddNewMagCard(txbID.Text, dtpkPurchaseDay.SelectedDate, txbIDcus.Text);
            GetListMagCard();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            MagCardDAO.Instance.UpdateMagCard(selectedItem, dtpkPurchaseDay.SelectedDate, txbIDcus.Text);
            GetListMagCard();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MagCardDAO.Instance.DeleteMagCard(selectedItem);
            GetListMagCard();
        }

        private void lstMagCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstMagCard.SelectedItem as The_Tu;
            if (selectedItem != null)
            {
                txbID.Text = selectedItem.Ma_the_tu;
                txbIDcus.Text = selectedItem.Ma_hanh_khach;
                dtpkPurchaseDay.SelectedDate = selectedItem.Ngay_mua;

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
