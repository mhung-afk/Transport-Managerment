using System.Windows;
using TransportManagerment.DataAccess;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for ChangePassWindow.xaml
    /// </summary>
    public partial class ChangePassWindow : Window
    {
        public ChangePassWindow()
        {
            InitializeComponent();
            txbUserName.Text = DataProvider.Instance.curUser.username;
        }

        private void pwbnew_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(pwbnew.Password))
            {
                pwbre.IsEnabled = true;
            }
            else
            {
                pwbre.Clear();
            }
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            AccountDAO.Instance.CheckNewPass(pwb.Password, pwbnew.Password, pwbre.Password);
            this.Close();
        }
    }
}
