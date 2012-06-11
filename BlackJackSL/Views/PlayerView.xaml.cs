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
using BlackJackSL.Controls;

namespace BlackJackSL.Views
{
    public partial class PlayerView : UserControl
    {
        public static readonly DependencyProperty PlayerActiveProperty =
           DependencyProperty.Register("ActivePlayer", typeof(bool), typeof(PlayerView),
                                       new PropertyMetadata(false, IsActivePlayerCallback));

        public static readonly DependencyProperty InsuranceActiveProperty =
           DependencyProperty.Register("ActiveInsurance", typeof(bool), typeof(PlayerView),
                                       new PropertyMetadata(false, IsActiveInsuranceCallback));

        public PlayerView()
        {
            InitializeComponent();
            SetBinding(PlayerActiveProperty, new Binding("ActivePlayer"));
            SetBinding(InsuranceActiveProperty, new Binding("ActiveInsurance"));
        }

        public bool PlayerActive
        {
            get { return (bool)GetValue(PlayerActiveProperty); }
            set { SetValue(PlayerActiveProperty, value); }
        }

        private static void IsActivePlayerCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PlayerView;
            if (control != null)
                control.SetActivePlayer((bool)e.NewValue);
        }

        private void SetActivePlayer(bool playerIsActive)
        {
            if (playerIsActive)
            {
                PlayerIsActive.Begin();
            }
            else
            {
                PlayerIsNotActive.Begin();
            }

        }

        public bool InsuranceActive
        {
            get { return (bool)GetValue(InsuranceActiveProperty); }
            set { SetValue(InsuranceActiveProperty, value); }
        }

        private static void IsActiveInsuranceCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PlayerView;
            if (control != null)
                control.SetActiveInsurance((bool)e.NewValue);
        }

        private void SetActiveInsurance(bool insuranceIsActive)
        {
            if (insuranceIsActive)
            {
                ShowInsurance.Begin();
                //InsuranceQOn.Begin();
            }
            else
            {
                HideInsurance.Begin();
                //InsuranceQOff.Begin();
            }

        }
    }
}
