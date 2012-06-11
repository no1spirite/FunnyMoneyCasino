using System.Windows;
using System.Windows.Input;

namespace BlackJackSL.SLExtensions.Input
{
    public class MouseEnterCommandSubscription : CommandSubscription
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseCommandSubscription"/> class.
        /// </summary>
        /// <param name="element">The element attached to the command.</param>
        /// <param name="commandName">Name of the command.</param>
        public MouseEnterCommandSubscription(UIElement element, string commandName)
            : base(element, commandName)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void HookEvents()
        {
            Element.MouseEnter += new MouseEventHandler(Element_MouseEnter);
        }

        protected override void UnhookEvents()
        {
            Element.MouseEnter -= new MouseEventHandler(Element_MouseEnter);
        }

        void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            ExecuteCommand(sender);
        }

        #endregion Methods
    }
}