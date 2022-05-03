using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TransportManagerment.DataAccess;
using TransportManagerment.Model;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for TrainWindow.xaml
    /// </summary>
    public partial class TrainWindow : Window
    {
        public TrainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListTrain();
            CheckStaff();
        }

        public Tuyen_tau_dien selectedItem { get; set; }
        public bool isStaff { get; set; }

        void CheckStaff()
        {
            isStaff = DataProvider.Instance.CheckStaff();
        }

        void GetListTrain()
        {
            lstTrain.ItemsSource = TrainDAO.Instance.GetListTrain();
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(lstTrain.ItemsSource);

            view.SortDescriptions.Add(new SortDescription("Ma_tuyen_tau", ListSortDirection.Ascending));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            TrainDAO.Instance.AddNewTrain(txbTrainCode.Text, txbName.Text, Convert.ToInt32(txbPrice.Text), txbIDtrain.Text);
            GetListTrain();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            TrainDAO.Instance.UpdateTrain(selectedItem, txbName.Text, Convert.ToInt32(txbPrice.Text), txbIDtrain.Text);
            GetListTrain();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            TrainDAO.Instance.DeleteTrain(selectedItem);
            GetListTrain();
        }

        private void lstTrain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstTrain.SelectedItem as Tuyen_tau_dien;
            if (selectedItem != null)
            {
                txbIDtrain.Text = selectedItem.Ma_tuyen_tau_dien;
                txbName.Text = selectedItem.Ten_tuyen_tau;
                txbPrice.Text = selectedItem.Don_gia.ToString();
                txbTrainCode.Text = selectedItem.Ma_tuyen_tau;

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
