using System.Windows;
using TransportManagerment.DataAccess;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Login(txbUserName.Text, pwb.Password))
            {
                this.Hide();
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác.");
            }
        }

        bool Login(string userName, string password)
        {
            return AccountDAO.Instance.Login(userName, password);
        }
    }
}
