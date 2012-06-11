using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BlackJackSL.SLExtensions.Input
{
    public class MouseLeftButtonUpCommandSubscription : CommandSubscription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseLeftButtonUpCommandSubscription"/> class.
        /// </summary>
        /// <param name="element">The element attached to the command.</param>
        /// <param name="commandName">Name of the command.</param>
        public MouseLeftButtonUpCommandSubscription(UIElement element, string commandName) 
            : base(element, commandName)
        {
        }

        protected override void HookEvents()
        {
            Element.MouseLeftButtonUp += Element_MouseLeftButtonUp;
        }

        protected override void UnhookEvents()
        {
            Element.MouseLeftButtonUp -= Element_MouseLeftButtonUp;
        }

        void Element_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            ExecuteCommand(sender);
        }
    }
}