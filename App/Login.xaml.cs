using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            button_sign_up.IsEnabled = false;
            SignUp SignUp = new SignUp();
            SignUp.Visibility = Visibility.Visible;
            this.Close();
        }

        private void button_sign_in_Click(object sender, RoutedEventArgs e)
        {
            button_sign_in.IsEnabled = false;
            MainWindow MainWindow = new MainWindow();
            MainWindow.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
