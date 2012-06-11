using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BlackJackSL.BaseClasses;
using BlackJackSL.ChatService;
using BlackJackSL.Code;
using BlackJackSL.Model;
using BlackJackSL.SLExtensions.Input;

namespace BlackJackSL.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private AsyncObservableCollection<Chat> _textItems;
        public AsyncObservableCollection<Chat> TextItems
        {
            get { return _textItems; }
            set
            {
                _textItems = value;
                OnPropertyChanged("TextItems");
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public ChatViewModel()
        {
            TextItems = new AsyncObservableCollection<Chat>();
            Message = "";
            Commands.SendMessageCommand.CanExecute += (sender, e) => e.CanExecute = true;
            Commands.SendMessageCommand.Executed += SendMessageCommand_OnExecuted;
        }

        private void SendMessageCommand_OnExecuted(object sender, ExecutedEventArgs e)
        {
            ServiceLocator.Get<ClientComms>().Send();   
        }
    }

}
