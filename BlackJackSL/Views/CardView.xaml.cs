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
using BlackJackSL.ViewModels;

namespace BlackJackSL.Views
{
    public partial class CardView : UserControl
    {
        public static readonly DependencyProperty IsCardVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(int), typeof(CardView),
                                        new PropertyMetadata(0, IsCardVisibleCallback));

        public CardView()
        {
            InitializeComponent();
            SetBinding(IsCardVisibleProperty, new Binding("IsVisible"));
            CardFront.LayoutUpdated += new EventHandler(CardChanged);
            CardBack.LayoutUpdated += new EventHandler(CardChanged);
        }

        //public int IsVisible
        //{
        //    get { return (int)GetValue(IsCardVisibleProperty); }
        //    set { SetValue(IsCardVisibleProperty, value); }
        //}

        private static void IsCardVisibleCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CardView;
            if (control != null)
            {
                //if(e.NewValue != e.OldValue)
                    control.SetIsVisible((int)e.NewValue);
            }
        }

        private void SetIsVisible(int isVisible)
        {

            if (isVisible == 1)
                RotateCard.Begin();
            else if (isVisible == 2)
            {
                CardFadeIn.Begin();
            }
            else if (isVisible == 3)
            {
                CardMove.Begin();
            }
            else if (isVisible == 4)
            {
                DelayedRotateCard.Begin();
            }
        }

        private void CardChanged(object sender, EventArgs e)
        {
            SliderZValue.Text = "Z Rotation: " + String.Format("{0:0.00}", SliderZ.Value);
            SliderXValue.Text = "X Rotation: " + String.Format("{0:0.00}", SliderX.Value);
            SliderYValue.Text = "Y Rotation: " + String.Format("{0:0.00}", SliderY.Value);

            if (SliderX.Value > 90 && SliderX.Value < 270 && (SliderY.Value < 90 || SliderY.Value > 270))
            {
                CardBack.Visibility = Visibility.Collapsed;
            }

            else if (SliderX.Value > 90 && SliderX.Value < 270 && (SliderY.Value > 90 && SliderY.Value < 270))
            {
                CardBack.Visibility = Visibility.Visible;
            }

            else if ((SliderX.Value < 90 || SliderX.Value > 270) && (SliderY.Value < 90 || SliderY.Value > 270))
            {
                CardBack.Visibility = Visibility.Visible;
            }

            else if ((SliderX.Value < 90 || SliderX.Value > 270) && (SliderY.Value > 90 || SliderY.Value < 270))
            {
                CardBack.Visibility = Visibility.Collapsed;
            }
        }

    }
}
