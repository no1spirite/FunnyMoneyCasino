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
using BlackJackSL.Views;

namespace BlackJackSL.Controls
{
    public class HandItemsControl : ItemsControl
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            FrameworkElement contentitem = element as FrameworkElement;
            Binding leftBinding = new Binding("HandXPos");
            Binding zBinding = new Binding("HandZPos");
            contentitem.SetBinding(Canvas.LeftProperty, leftBinding);
            contentitem.SetBinding(Canvas.ZIndexProperty, zBinding);
            base.PrepareContainerForItemOverride(element, item);
        }
    }
}
