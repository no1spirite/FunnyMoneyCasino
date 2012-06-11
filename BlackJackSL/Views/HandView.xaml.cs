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

namespace BlackJackSL.Views
{
    public partial class HandView : UserControl
    {
        public static readonly DependencyProperty HandActiveProperty =
           DependencyProperty.Register("ActiveHand", typeof(bool), typeof(HandView),
                                       new PropertyMetadata(false, IsActiveHandCallback));

        public static readonly DependencyProperty SplitLeftProperty =
           DependencyProperty.Register("IsSplitLeft", typeof(bool), typeof(HandView),
                                       new PropertyMetadata(false, IsSplitLeftCallback));

        public static readonly DependencyProperty SplitRightProperty =
           DependencyProperty.Register("IsSplitRight", typeof(bool), typeof(HandView),
                                       new PropertyMetadata(false, IsSplitRightCallback));

        public HandView()
        {
            InitializeComponent();
            SetBinding(HandActiveProperty, new Binding("ActiveHand"));
            SetBinding(SplitLeftProperty, new Binding("IsSplitLeft"));
            SetBinding(SplitRightProperty, new Binding("IsSplitRight"));
        }

        public bool HandActive
        {
            get { return (bool)GetValue(HandActiveProperty); }
            set { SetValue(HandActiveProperty, value); }
        }

        private static void IsActiveHandCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HandView;    
            if (control != null)
                control.SetActiveHand((bool)e.NewValue);
        }

        private void SetActiveHand(bool handIsActive)
        {
            if (handIsActive)
            {

                HandIsActive.Begin();
            }
            else
            {
                HandIsNotActive.Begin();
            }

        }

        public bool SplitLeft
        {
            get { return (bool)GetValue(SplitLeftProperty); }
            set { SetValue(SplitLeftProperty, value); }
        }

        private static void IsSplitLeftCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HandView;
            if (control != null)
                control.SetSplitLeft((bool)e.NewValue);
        }

        private void SetSplitLeft(bool isSplitLeft)
        {
            if (isSplitLeft)
            {
                MoveHandLeft.Begin();
            }
        }

        public bool SplitRight
        {
            get { return (bool)GetValue(SplitRightProperty); }
            set { SetValue(SplitRightProperty, value); }
        }

        private static void IsSplitRightCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HandView;
            if (control != null)
                control.SetSplitRight((bool)e.NewValue);
        }

        private void SetSplitRight(bool isSplitRight)
        {
            if (isSplitRight)
            {
                //MoveHandUp.Begin();
                MoveHandRight.Begin();
            }
        }
    }
}
