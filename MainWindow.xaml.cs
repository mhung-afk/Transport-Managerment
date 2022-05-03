using System.Windows;
using System.Windows.Controls;
using TransportManagerment.DataAccess;

namespace TransportManagerment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            AccountWindow accountWindow = new AccountWindow();
            accountWindow.ShowDialog();
        }

        private void btnRoad_Click(object sender, RoutedEventArgs e)
        {
            RoadWindow roadWindow = new RoadWindow();
            roadWindow.ShowDialog();
        }

        private void btnDistance_Click(object sender, RoutedEventArgs e)
        {
            DistanceWindow distanceWindow = new DistanceWindow();
            distanceWindow.ShowDialog();
        }

        private void btnCross_Click(object sender, RoutedEventArgs e)
        {
            CrossWindow crossWindow = new CrossWindow();
            crossWindow.ShowDialog();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            StopWindow stopWindow = new StopWindow();
            stopWindow.ShowDialog();
        }

        private void btnBus_Click(object sender, RoutedEventArgs e)
        {
            BusWindow busWindow = new BusWindow();
            busWindow.ShowDialog();
        }

        private void btnTrain_Click(object sender, RoutedEventArgs e)
        {
            TrainWindow trainWindow = new TrainWindow();
            trainWindow.ShowDialog();
        }

        private void btnStaff_Click(object sender, RoutedEventArgs e)
        {
            StaffWindow staffWindow = new StaffWindow();
            staffWindow.ShowDialog();
        }

        private void btnConsumer_Click(object sender, RoutedEventArgs e)
        {
            ConsumerWindow consumerWindow = new ConsumerWindow();
            consumerWindow.ShowDialog();
        }

        private void btnTicket_Click(object sender, RoutedEventArgs e)
        {
            TicketWindow ticketWindow = new TicketWindow();
            ticketWindow.ShowDialog();
        }

        private void btnTrip_Click(object sender, RoutedEventArgs e)
        {
            TripWindow tripWindow = new TripWindow();
            tripWindow.ShowDialog();
        }

        private void btnPriceTable_Click(object sender, RoutedEventArgs e)
        {
            PriceTableWindow priceTableWindow = new PriceTableWindow();
            priceTableWindow.ShowDialog();
        }

        private void GetRouteByIdRoute(string ID)
        {
            lst1.ItemsSource = DataProvider.Instance.GetRouteByIdRoute(ID);
        }

        private void txbIDroute_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetRouteByIdRoute(txbIDroute.Text);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txbRoute2.Text) || dtpkFrom.SelectedDate == null || dtpkTo.SelectedDate == null) return;
            lst2.ItemsSource = DataProvider.Instance.StatisticTurnByDay(txbRoute2.Text, dtpkFrom.SelectedDate, dtpkTo.SelectedDate);
        }
    }
}
