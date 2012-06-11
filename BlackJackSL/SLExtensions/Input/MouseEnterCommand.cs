using System.Windows;

namespace BlackJackSL.SLExtensions.Input
{
    public class MouseEnterCommand : Command
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEnterCommand"/> class.
        /// </summary>
        /// <param name="commandName">The command name used for retreiving command in Xaml.</param>
        public MouseEnterCommand(string commandName)
            : base(commandName)
        {
        }

        #endregion Constructors

        #region Methods

        public override CommandSubscription CreateCommandSubscription(UIElement element, string commandName)
        {
            return  new MouseEnterCommandSubscription(element, commandName);
        }

        #endregion Methods
    }
}