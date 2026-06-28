using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Grpc.Net.Client;
using GrpcClient;

namespace Work3
{
    /// <summary>
    /// Interaction logic for Connect.xaml
    /// </summary>
    public partial class Connect : Window
    {
        public Connect()
        {
            InitializeComponent();
        }

        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            Button_Connect.IsEnabled = false;
            Text_HostName.IsEnabled = false;
            using var channel = GrpcChannel.ForAddress("http://" + Text_HostName.Text + ":5122"); 
            var client = new Greeter.GreeterClient(channel);
            try 
            { 
                HelloReply reply = client.SayHello(new HelloRequest { Name = Dns.GetHostName() });
                if (reply != null & reply.Message == "Hello " + Dns.GetHostName())
                {
                    App.ServerHost = "http://" + Text_HostName.Text + ":5122";
                    Login Login = new Login();
                    Login.Visibility = Visibility.Visible;
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Не удалось подключится к " + Text_HostName.Text, "Ошибка");
            }
            Button_Connect.IsEnabled = true;
            Text_HostName.IsEnabled = true;
        }
    }
}
