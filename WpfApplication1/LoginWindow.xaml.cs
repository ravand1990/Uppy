using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Uppy.Properties;

namespace Uppy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public static CookieContainer cookies = new CookieContainer();
        private String username;
        private String password;
        public Login()
        {
            InitializeComponent();
           
            setupValues();
        }

        public void setupValues()
        {
            if (Settings.Default.login_username != "" && Settings.Default.login_password != "")
            {


                textBox_username.Text = Settings.Default.login_username;
                passwordBox_password.Password = Settings.Default.login_password;
                username = Settings.Default.login_username;
                password = Settings.Default.login_password;
            }
        }


        private void button_login_Click(object sender, RoutedEventArgs e)
        {
            string response =
                new WebClient().DownloadString("http://ravand.org/logintest/login.php?username=" +
                                               textBox_username.Text +
                                               "&password=" + passwordBox_password.Password);
            if (response.Contains("Login successful"))
            {
                username = textBox_username.Text;
                password = passwordBox_password.Password;
                Settings.Default.login_username = username;
                Settings.Default.login_password = password;

                Settings.Default.Save();
                MemberWindow mw = new MemberWindow(this.username,this.password);
                mw.Show();
                Hide();
            }
            if (response.Contains("Wrong password"))
            {
                MessageBox.Show("Incorrect password!");
            }
            if (response.Contains("doesn't exist"))
            {
                MessageBox.Show("Account does not exist!");
            }
        }

        private void button_register_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://ravand.org/logintest/register.php");
        }
    }
}
