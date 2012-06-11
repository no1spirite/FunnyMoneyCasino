using System.Windows;

namespace BlackJackSL.SLExtensions.Input
{
    public class MouseLeftButtonUpCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseLeftButtonUpCommand"/> class.
        /// </summary>
        /// <param name="commandName">The command name used for retreiving command in Xaml.</param>
        public MouseLeftButtonUpCommand(string commandName) 
            : base(commandName)
        {
        }

        public override CommandSubscription CreateCommandSubscription(UIElement element, string commandName)
        {
            return new MouseLeftButtonUpCommandSubscription(element, commandName);
        }
    }
}