namespace Core.Behaviors
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System;
    using Microsoft.Phone.Controls;
    using System.Windows.Controls;

    /// <summary>
    /// Invokes command by tap on assosiated object.
    /// </summary>
    public class TapListenerBehavior : Behavior<UIElement>
    {
        #region Command

        /// <summary>
        /// Command property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(TapListenerBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Command parameter property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(TapListenerBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets command property.
        /// </summary>
        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }

            set
            {
                this.SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets command parameter.
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return (object)this.GetValue(CommandParameterProperty);
            }

            set
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        #endregion Command

        #region NavigationUri

        public static readonly DependencyProperty NavigationUriProperty = DependencyProperty.Register(
            "NavigationUri",
            typeof(Uri),
            typeof(TapListenerBehavior),
            new PropertyMetadata(null));

        public Uri NavigationUri
        {
            get { return (Uri)GetValue(NavigationUriProperty); }
            set { SetValue(NavigationUriProperty, value); }
        }

        #endregion NavigationUri

        /// <summary>
        /// Raises by attaching assosiated object.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            if (this.AssociatedObject as Control != null)
                this.Command.CanExecuteChanged += OnCanExecuteChanged;
            this.AssociatedObject.Tap += this.OnTap;
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            ((Control)this.AssociatedObject).IsEnabled = this.Command.CanExecute(CommandParameter);
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Tap -= this.OnTap;
            base.OnDetaching();
        }

        /// <summary>
        /// Raises by tap on assosiated object.
        /// </summary>
        /// <param name="sender">Assosiated object.</param>
        /// <param name="e">Event parameters.</param>
        private void OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.Command != null && this.Command.CanExecute(CommandParameter))
            {
                this.Command.Execute(this.CommandParameter);
            }
            else if (NavigationUri != null)
            {
                var frame = Application.Current.RootVisual as PhoneApplicationFrame;
                frame.Navigate(NavigationUri);
            }
        }
    }
}
