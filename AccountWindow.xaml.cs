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
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        //public string username { get; set; }
        //public string displayName { get; set; }
        //public string password { get; set; }
        //public byte? proxy { get; set; }

        public List<int> listProxy { get; set; }
        public Account selectedItem { get; set; }
        public bool isAdmin { get; set; }

        public AccountWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GetListAccount();
            GetListProxy();
            CheckProxy();
        }

        void CheckProxy()
        {
            isAdmin = DataProvider.Instance.CheckProxy();
        }

        void GetListProxy()
        {
            listProxy = AccountDAO.Instance.GetListProxy();
        }

        void GetListAccount()
        {
            lstAccount.ItemsSource = AccountDAO.Instance.GetListAccount();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstAccount.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("proxy", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("username", ListSortDirection.Ascending));
        }

        private void lstAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = lstAccount.SelectedItem as Account;
            if (selectedItem != null)
            {
                txbUsername.Text = selectedItem.username;
                txbDisplayName.Text = selectedItem.displayName;
                cbProxy.SelectedItem = (int)selectedItem.proxy;
                btnDelete.IsEnabled = true && isAdmin;
                btnUpdate.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
                btnUpdate.IsEnabled = false;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbProxy.SelectedIndex == -1) return;
            AccountDAO.Instance.AddNewAcc(txbUsername.Text, txbDisplayName.Text, (byte?)Convert.ToInt32((cbProxy.SelectedItem.ToString())));
            GetListAccount();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbProxy.SelectedIndex == -1) return;
            AccountDAO.Instance.UpdateAcc(selectedItem, txbDisplayName.Text, (byte?)Convert.ToInt32((cbProxy.SelectedItem.ToString())), isAdmin);
            GetListAccount();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            AccountDAO.Instance.DeleteAcc(selectedItem);
            GetListAccount();
        }

        private void btnChangePass_Click(object sender, RoutedEventArgs e)
        {
            ChangePassWindow changePassWindow = new ChangePassWindow();
            changePassWindow.ShowDialog();
            GetListAccount();
        }
    }
}
