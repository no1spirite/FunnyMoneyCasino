using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace BlackJackSL.Behaviours
{
    public enum TargetMethodExecuteMode
    {
        DataContext,
        AssociatedObject
    }

    public class UnloadedBehavior : Behavior<Control>
    {
        #region MethodExecuteMode

        public static readonly DependencyProperty MethodExecuteModeProperty =
            DependencyProperty.Register("MethodExecuteMode", typeof(TargetMethodExecuteMode), typeof(UnloadedBehavior),
                                        new PropertyMetadata(TargetMethodExecuteMode.AssociatedObject, OnMethodExecuteModeChanged));

        public TargetMethodExecuteMode MethodExecuteMode
        {
            get { return (TargetMethodExecuteMode)GetValue(MethodExecuteModeProperty); }
            set { SetValue(MethodExecuteModeProperty, value); }
        }

        private static void OnMethodExecuteModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((UnloadedBehavior)d).OnMethodExecuteModeChanged(e);
        }

        protected virtual void OnMethodExecuteModeChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region TargetMethod

        public static readonly DependencyProperty TargetMethodProperty =
            DependencyProperty.Register("TargetMethod", typeof(string), typeof(UnloadedBehavior),
                                        new PropertyMetadata("", OnTargetMethodChanged));

        public string TargetMethod
        {
            get { return (string)GetValue(TargetMethodProperty); }
            set { SetValue(TargetMethodProperty, value); }
        }

        private static void OnTargetMethodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((UnloadedBehavior)d).OnTargetMethodChanged(e);
        }

        protected virtual void OnTargetMethodChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
        }

        private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
        {
            if (!InTree())
                ExecuteTargetMethod();
        }

        protected bool InTree()
        {
            FrameworkElement element = AssociatedObject;

            var rootElement = Application.Current.RootVisual as FrameworkElement;

            while (element != null)
            {
                if (element == rootElement)
                    return true;

                element = VisualTreeHelper.GetParent(element) as FrameworkElement;
            }

            return false;
        }

        private void ExecuteTargetMethod()
        {
            if (string.IsNullOrEmpty(TargetMethod))
                return;

            Type contextType;
            object targetMethodObject;

            switch (MethodExecuteMode)
            {
                case TargetMethodExecuteMode.DataContext:
                    targetMethodObject = AssociatedObject.DataContext;
                    if (targetMethodObject == null)
                        return;

                    contextType = AssociatedObject.DataContext.GetType();
                    break;
                case TargetMethodExecuteMode.AssociatedObject:
                    targetMethodObject = AssociatedObject;
                    contextType = AssociatedObject.GetType();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (targetMethodObject == null)
                return;

            MethodInfo method = contextType.GetMethod(TargetMethod);

            if (method == null)
                return;

            method.Invoke(targetMethodObject, null);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
            base.OnDetaching();
        }
    }
}