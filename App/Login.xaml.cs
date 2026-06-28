using Grpc.Net.Client;
using GrpcClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.InteropServices.Swift;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Work3
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private void OnTextChange()
        {
            if (Text_UserName.Text.Length > 0 && Text_Password.Text.Length > 0)
            {
                button_sign_in.IsEnabled = true;
            }
            else
            {
                button_sign_in.IsEnabled = false;
            }
        }
        public Login()
        {
            InitializeComponent();
        }

        public Login(string UserName)
        {
            InitializeComponent();
            Text_UserName.Text = UserName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            button_sign_up.IsEnabled = false;
            SignUp SignUp = new SignUp(Text_UserName.Text);
            SignUp.Visibility = Visibility.Visible;
            this.Close();
        }

        private void button_sign_in_Click(object sender, RoutedEventArgs e)
        {
            button_sign_in.IsEnabled = false;
            using var channel = GrpcChannel.ForAddress(App.ServerHost);
            var client = new GrpcClient.Work3.Work3Client(channel);
            try
            {
                AuthUserRequest request = new AuthUserRequest();
                request.User = new User();
                request.User.UserName = Text_UserName.Text;
                request.User.Password = Text_Password.Text;
                AuthUserReply reply = client.AuthUser(request);
                if (reply != null & reply.Reply == 1)
                {
                    MessageBox.Show("Пользователь не существует", "Ошибка");
                }
                if (reply != null & reply.Reply == 2)
                {
                    MessageBox.Show("Неверный пароль", "Ошибка");
                }
                if (reply != null & reply.Reply == 0)
                {
                    App.User = Text_UserName.Text;
                    MainWindow MainWindow = new MainWindow();
                    MainWindow.Visibility = Visibility.Visible;
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Потеряно соединение с " + App.ServerHost, "Ошибка");
            }
            button_sign_in.IsEnabled = true;
        }

        private void Text_UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChange();
        }

        private void Text_Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChange();
        }
    }
}
