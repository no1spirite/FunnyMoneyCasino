﻿#pragma checksum "C:\Dev\Personal\FunnyMoneyCasino\BlackJackSL\Views\ChatView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0CD1E51781334E408147C6E91541E4D3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace BlackJackSL.Views {
    
    
    public partial class ChatView : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Style listboxstyle;
        
        internal System.Windows.DataTemplate ChatTemplate;
        
        internal System.Windows.Controls.TextBox textMessage;
        
        internal System.Windows.Controls.TextBlock InvisibleMsg;
        
        internal System.Windows.Controls.Button buttonSend;
        
        internal System.Windows.Controls.ListBox textLines;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/BlackJackSL;component/Views/ChatView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.listboxstyle = ((System.Windows.Style)(this.FindName("listboxstyle")));
            this.ChatTemplate = ((System.Windows.DataTemplate)(this.FindName("ChatTemplate")));
            this.textMessage = ((System.Windows.Controls.TextBox)(this.FindName("textMessage")));
            this.InvisibleMsg = ((System.Windows.Controls.TextBlock)(this.FindName("InvisibleMsg")));
            this.buttonSend = ((System.Windows.Controls.Button)(this.FindName("buttonSend")));
            this.textLines = ((System.Windows.Controls.ListBox)(this.FindName("textLines")));
        }
    }
}
