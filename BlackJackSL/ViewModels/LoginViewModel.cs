using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BlackJackSL.BaseClasses;
using BlackJackSL.Code;
using BlackJackSL.SLExtensions.Input;
using BlackJackSL.Views;

namespace BlackJackSL.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            LoginText = "";
            Commands.LoginCommand.CanExecute += (sender, e) => e.CanExecute = true;
            Commands.LoginCommand.Executed += LoginCommand_OnExecuted;
        }

        private string _nickname;
        public string Nickname
        {
            get { return _nickname; }
            set
            {
                _nickname = value;
                OnPropertyChanged("Nickname");
            }
        }

        private string _loginText;
        public string LoginText
        {
            get { return _loginText; }
            set
            {
                _loginText = value;
                OnPropertyChanged("LoginText");
            }
        }

        private void LoginCommand_OnExecuted(object sender, ExecutedEventArgs e)
        {
            if(string.IsNullOrEmpty(Nickname))
            {
                LoginText = "Name Cannot Be Empty";
            }
            else
            {
                ServiceLocator.Get<ClientComms>().GameConnect();
                ServiceLocator.Get<ClientComms>().ChatConnect(Nickname);
            }
        }
    }
}
