using System;
using System.Collections.Generic;
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
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void button_sign_in_Click(object sender, RoutedEventArgs e)
        {
            button_sign_in.IsEnabled = false;
            Login Login = new Login();
            Login.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}