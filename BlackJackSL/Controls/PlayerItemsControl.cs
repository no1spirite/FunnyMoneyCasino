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
    public class PlayerItemsControl : ItemsControl
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            FrameworkElement contentitem = element as FrameworkElement;
            Binding leftBinding = new Binding("PlayerXPos");
            Binding topBinding = new Binding("PlayerYPos");
            contentitem.SetBinding(Canvas.LeftProperty, leftBinding);
            contentitem.SetBinding(Canvas.TopProperty, topBinding);
            base.PrepareContainerForItemOverride(element, item);
        }

    }
}