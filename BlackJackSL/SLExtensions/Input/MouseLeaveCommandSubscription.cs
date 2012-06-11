using System.Windows;
using System.Windows.Input;

namespace BlackJackSL.SLExtensions.Input
{
    public class MouseLeaveCommandSubscription : CommandSubscription
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseLeaveCommandSubscription"/> class.
        /// </summary>
        /// <param name="element">The element attached to the command.</param>
        /// <param name="commandName">Name of the command.</param>
        public MouseLeaveCommandSubscription(UIElement element, string commandName)
            : base(element, commandName)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void HookEvents()
        {
            Element.MouseLeave += Element_MouseLeave;
        }

        protected override void UnhookEvents()
        {
            Element.MouseLeave -= Element_MouseLeave;
        }

        void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            ExecuteCommand(sender);
        }

        #endregion Methods
    }
}