using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BlackJackSL.Controls
{
    public class CardItemsControl : ItemsControl
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            FrameworkElement contentitem = element as FrameworkElement;
            Binding leftBinding = new Binding("CardXPos");
            Binding topBinding = new Binding("CardYPos");
            Binding zBinding = new Binding("CardZPos");
            Binding isActive = new Binding("ActiveHand");
            //BindingOperations.SetBinding(MyGlow, OuterGlowBitmapEffect.GlowSizeProperty, isActive);
            contentitem.SetBinding(Canvas.LeftProperty, leftBinding);
            contentitem.SetBinding(Canvas.TopProperty, topBinding);
            contentitem.SetBinding(Canvas.ZIndexProperty, zBinding);
            base.PrepareContainerForItemOverride(element, item);
        }

        public void CreateGlow()
        {
            
        }

    }
}