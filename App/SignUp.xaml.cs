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

        private void text_changed()
        {
            if ((textbox_username.Text.Length > 0)  &&
                (textbox_password1.Text.Length > 0) &&
                (textbox_password2.Text.Length > 0)) {
                label_incorrect_password.Content = "";
                button_create_account.IsEnabled = true;
                return;
            }
            label_incorrect_password.Content = "";
            button_create_account.IsEnabled = false;
        }
        private void textbox_username_TextChanged(object sender, TextChangedEventArgs e)
        {
            text_changed();
        }

        private void textbox_password1_TextChanged(object sender, TextChangedEventArgs e)
        {
            text_changed();
        }

        private void textbox_password2_TextChanged(object sender, TextChangedEventArgs e)
        {
            text_changed();
        }

        private void button_create_account_Click(object sender, RoutedEventArgs e)
        {
            if (textbox_password1.Text != textbox_password2.Text) {
                label_incorrect_password.Content = "Пароли не совпадают";
                return;
            }
            MessageBox.Show("Пользователь создан");
            Login Login = new Login();
            Login.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}