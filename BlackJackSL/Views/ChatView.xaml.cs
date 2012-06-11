using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BlackJackSL.ChatService;

namespace BlackJackSL.Views
{
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            InitializeComponent();
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            textMessage.Focus();
            textMessage.Text = "";
        }

        private void textMessage_KeyDown(object sender, KeyEventArgs e)
        {
            InvisibleMsg.Text = textMessage.Text;
        }
    }
}