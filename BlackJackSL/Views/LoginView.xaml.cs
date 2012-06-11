using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BlackJackSL.Code;
using BlackJackSL.ViewModels;

namespace BlackJackSL.Views
{
    public partial class LoginView : ChildWindow
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            InvisibleNick.Text = Nickname.Text;
            if(Nickname.Text.Length != 0)
                this.DialogResult = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Browser.HtmlPage.Plugin.Focus();
            this.Nickname.Focus();
        }

        private void Nickname_KeyDown(object sender, KeyEventArgs e)
        {
            InvisibleNick.Text = Nickname.Text;
        }
    }
}
