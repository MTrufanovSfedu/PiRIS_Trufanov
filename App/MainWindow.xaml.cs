using Grpc.Net.Client;
using GrpcClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Work3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private GrpcChannel channel;
        private GrpcClient.Work3.Work3Client client;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }
        public enum PositionType
        {
            Production = 0,
            Service = 1,
            Bonus = 2
        }
        public class PositionObject
        {
            public int PositionID { get; set; }
            public string PositionName { get; set; }
            public PositionType PositionType { get; set; }
            public int PositionValue { get; set; }
            public double PositionPrice { get; set; }
            public double PriceCurrency { get; set; }
        }

        private void UpdateDb()
        {
            dgMain.Items.Clear();
            try
            {
                SendAllItemsRequest request = new SendAllItemsRequest();
                request.Request = 0;
                SendAllItemsReply reply = client.SendAllItems(request);
                if (reply != null & reply.Reply == 1)
                {
                    MessageBox.Show("Не удалось обновить базу данных", "Ошибка");
                }
                if (reply != null & reply.Reply == 0)
                {
                    for (int i = 0; i < reply.Item.Count; i++)
                    {
                        PositionObject item = new PositionObject();
                        item.PositionID = ((int)reply.Item[i].Id);
                        item.PositionName = reply.Item[i].PositionName;
                        item.PositionType = (Work3.MainWindow.PositionType)reply.Item[i].PositionType;
                        item.PositionValue = ((int)reply.Item[i].PositionValue);
                        item.PositionPrice = reply.Item[i].PositionPrice;
                        item.PriceCurrency = reply.Item[i].PriceCurrency;
                        dgMain.Items.Add(item);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Потеряно соединение с " + App.ServerHost, "Ошибка");
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            cbPosType.ItemsSource = Enum.GetValues(typeof(PositionType));
            cbPosType.SelectedIndex = 0;
            lUser.Content = "Текущий пользователь: " + App.User;
            channel = GrpcChannel.ForAddress(App.ServerHost);
            client = new GrpcClient.Work3.Work3Client(channel);
            UpdateDb();
        }

        private void bQuotes_Click(object sender, RoutedEventArgs e)
        {
            lbQuotes.Items.Clear();
            lbQuotes.Items.Add("75.47 USD");
            lbQuotes.Items.Add("80.24 EUR");
            lbQuotes.Items.Add("10.88 CNY");
            lbQuotes.SelectedIndex = 0;
        }

        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            PositionObject item = new PositionObject();
            item.PositionID = dgMain.Items.Count + 1;
            item.PositionName = tbPosName.Text;
            item.PositionType =
            (PositionType)cbPosType.SelectedItem;
            item.PositionValue = int.Parse(tbPosValue.Text);
            item.PositionPrice = double.Parse(tbPosPrice.Text);
            string[] strings = lbQuotes.SelectedItem.ToString().Split(' ');
            item.PriceCurrency = Math.Round(double.Parse(tbPosPrice.Text) / double.Parse(strings[0]), 2);
            try
            {
                GetItemRequest request = new GetItemRequest();
                request.Item = new Item();
                request.Item.Id = item.PositionID;
                request.Item.PositionName = item.PositionName;
                request.Item.PositionType = (GrpcClient.PositionType)item.PositionType;
                request.Item.PositionValue = item.PositionValue;
                request.Item.PositionPrice = item.PositionPrice;
                request.Item.PriceCurrency = item.PriceCurrency;
                GetItemReply reply = client.GetItem(request);
                if (reply != null & reply.Reply == 1)
                {
                    MessageBox.Show("Не удалось добавить элемент", "Ошибка");
                }
                if (reply != null & reply.Reply == 2)
                {
                    MessageBox.Show("Не удалось изменить элемент", "Ошибка");
                }
            }
            catch
            {
                MessageBox.Show("Потеряно соединение с " + App.ServerHost, "Ошибка");
            }
            dgMain.Items.Add(item);
        }

        private void sMarkup_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lMarkup == null) return;
            lMarkup.Content = Math.Round(sMarkup.Value, 2).ToString() + "%";
        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            bUpdate.IsEnabled = false;
            UpdateDb();
            bUpdate.IsEnabled = true;
        }
    }
}