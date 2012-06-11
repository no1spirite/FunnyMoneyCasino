using System.Windows;

namespace BlackJackSL.SLExtensions.Input
{
    public class MouseLeaveCommand : Command
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseLeaveCommand"/> class.
        /// </summary>
        /// <param name="commandName">The command name used for retreiving command in Xaml.</param>
        public MouseLeaveCommand(string commandName)
            : base(commandName)
        {
        }

        #endregion Constructors

        #region Methods

        public override CommandSubscription CreateCommandSubscription(UIElement element, string commandName)
        {
            return  new MouseLeaveCommandSubscription(element, commandName);
        }

        #endregion Methods
    }
}