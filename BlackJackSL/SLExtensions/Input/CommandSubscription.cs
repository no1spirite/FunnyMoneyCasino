// <copyright file="CommandSubscription.cs" company="Ucaya">
// Distributed under Microsoft Public License (Ms-PL)
// </copyright>
// <author>Thierry Bouquain</author>
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BlackJackSL.SLExtensions.Input
{
    /// <summary>
    /// Handles command subscription of a UIElement
    /// </summary>
    public class CommandSubscription
    {
        #region Fields

        /// <summary>
        /// CommandSubscription depedency property.
        /// </summary>
        public static readonly DependencyProperty CommandSubscriptionProperty = 
            DependencyProperty.RegisterAttached(
                "CommandSubscription",
                typeof(Dictionary<string, CommandSubscription>),
                typeof(CommandSubscription),
                null);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandSubscription"/> class.
        /// </summary>
        /// <param name="element">The element attached to the command.</param>
        /// <param name="commandName">Name of the command.</param>
        public CommandSubscription(UIElement element, string commandName)
        {
            this.Element = element;
            this.CommandName = commandName;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the Command name
        /// </summary>
        /// <value>The name of the command.</value>
        public string CommandName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the element attached to the command
        /// </summary>
        /// <value>The element.</value>
        public UIElement Element
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Registers the command.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <param name="element">The element.</param>
        internal static void RegisterCommand(string commandName, UIElement element)
        {
            Command cmd = CommandService.FindCommand(commandName);
            if (cmd != null)
            {
                CommandSubscription subscription = cmd.CreateCommandSubscription(element, commandName);

                Dictionary<string, CommandSubscription> elementSubscriptions = element.GetValue(CommandSubscriptionProperty) as Dictionary<string, CommandSubscription>;
                if (elementSubscriptions == null)
                {
                    elementSubscriptions = new Dictionary<string, CommandSubscription>();
                    element.SetValue(CommandSubscriptionProperty, elementSubscriptions);
                }

                subscription.HookEvents();
                elementSubscriptions[commandName] = subscription;
            }
        }

        internal static void UnregisterAllSubscriptions(UIElement element)
        {
            Dictionary<string, CommandSubscription> elementSubscriptions = element.GetValue(CommandSubscriptionProperty) as Dictionary<string, CommandSubscription>;
            if (elementSubscriptions == null)
                return;

            foreach (var item in new List<CommandSubscription>(elementSubscriptions.Values))
            {
                item.Unregister();
            }
        }

        /// <summary>
        /// Unregister a command from an element
        /// </summary>
        /// <param name="commandName">The command name to remove</param>
        /// <param name="element">The element to be detached</param>
        internal static void UnregisterSubscription(string commandName, UIElement element)
        {
            Dictionary<string, CommandSubscription> elementSubscriptions = element.GetValue(CommandSubscriptionProperty) as Dictionary<string, CommandSubscription>;
            if (elementSubscriptions == null)
            {
                return;
            }

            CommandSubscription currentSubscription;
            if (!elementSubscriptions.TryGetValue(commandName, out currentSubscription))
            {
                return;
            }

            currentSubscription.Unregister();
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="sender">The command sender</param>
        protected virtual void ExecuteCommand(object sender)
        {
            string commandName = CommandService.GetCommand(this.Element);
            Command cmd = CommandService.FindCommand(commandName);
            object parameter = CommandService.GetCommandParameter(this.Element);
            parameter = PreProcessParameter(parameter);
            cmd.Execute(parameter, sender);
        }

        protected virtual void HookEvents()
        {
            UIElement element = Element;

            if (element is ButtonBase)
            {
                ((ButtonBase)element).Click += CommandService_Click;
            }
            else if (element is TextBox)
            {
                ((TextBox)element).KeyDown += CommandService_KeyDown;
            }
            else
            {
                element.MouseLeftButtonUp += CommandService_Click;
            }
        }

        protected virtual object PreProcessParameter(object parameter)
        {
            return parameter;
        }

        protected virtual void UnhookEvents()
        {
            UIElement element = this.Element;

            if (element is ButtonBase)
            {
                ((ButtonBase)element).Click -= this.CommandService_Click;
            }
            else if (element is TextBox)
            {
                ((TextBox)element).KeyDown -= CommandService_KeyDown;
            }
            else
            {
                element.MouseLeftButtonUp -= this.CommandService_Click;
            }
        }

        /// <summary>
        /// Unregister the current CommandSubscription
        /// </summary>
        protected virtual void Unregister()
        {
            UnhookEvents();

            Dictionary<string, CommandSubscription> elementSubscriptions = this.Element.GetValue(CommandSubscriptionProperty) as Dictionary<string, CommandSubscription>;
            if (elementSubscriptions == null)
            {
                return;
            }

            elementSubscriptions.Remove(this.CommandName);
        }

        /// <summary>
        /// Handles the Click event of the CommandService control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void CommandService_Click(object sender, EventArgs e)
        {
            ExecuteCommand(sender);
        }

        /// <summary>
        /// Handles the KeyDown event of the CommandService control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void CommandService_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ExecuteCommand(sender);
            }
        }

        #endregion Methods
    }
}