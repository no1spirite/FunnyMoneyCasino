using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

namespace BlackJackSL.Behaviours
{
    public class TextBoxEnterButtonInvokeBehaviour : TargetedTriggerAction<ButtonBase>
    {
        /// <summary>
        /// Gets or sets the peer.
        /// </summary>
        /// <value>The peer.</value>
        private AutomationPeer _peer { get; set; }

        /// <summary>
        /// Gets or sets the target button
        /// </summary>
        private ButtonBase _targetedButton { get; set; }

        /// <summary>
        /// Called after the TargetedTriggerAction is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            _targetedButton = this.Target;
            if (null == _targetedButton)
            {
                return;
            }

            // set peer
            this._peer = FrameworkElementAutomationPeer.FromElement(_targetedButton);
            if (this._peer == null)
            {
                this._peer = FrameworkElementAutomationPeer.CreatePeerForElement(_targetedButton);
            }
        }

        /// <summary>
        /// Called after targeted Button change.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the new targeted Button.</remarks>
        protected override void OnTargetChanged(ButtonBase oldTarget, ButtonBase newTarget)
        {
            base.OnTargetChanged(oldTarget, newTarget);
            _targetedButton = newTarget;
            if (null == _targetedButton)
            {
                return;
            }

            // set peer
            this._peer = FrameworkElementAutomationPeer.FromElement(_targetedButton);
            if (this._peer == null)
            {
                this._peer = FrameworkElementAutomationPeer.CreatePeerForElement(_targetedButton);
            }
        }

        /// <summary>
        /// Invokes the targeted Button when Enter key is pressed inside TextBox.
        /// </summary>
        /// <param name="parameter">KeyEventArgs with Enter key</param>
        protected override void Invoke(object parameter)
        {
            KeyEventArgs keyEventArgs = parameter as KeyEventArgs;
            if (null != keyEventArgs && keyEventArgs.Key == Key.Enter)
            {
                if (null != _peer && _peer.IsEnabled())
                {
                    IInvokeProvider invokeProvider = _peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProvider.Invoke();
                }
            }
        }
    }
}