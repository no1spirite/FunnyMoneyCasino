﻿#pragma checksum "C:\Dev\Personal\FunnyMoneyCasino\BlackJackSL\Views\LoginView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "09B8D3BFE342FD810883EBFD775CBDCD"
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
    
    
    public partial class LoginView : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Canvas LayoutRoot;
        
        internal System.Windows.Controls.TextBox Nickname;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.TextBlock InvisibleNick;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/BlackJackSL;component/Views/LoginView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Canvas)(this.FindName("LayoutRoot")));
            this.Nickname = ((System.Windows.Controls.TextBox)(this.FindName("Nickname")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.InvisibleNick = ((System.Windows.Controls.TextBlock)(this.FindName("InvisibleNick")));
        }
    }
}

