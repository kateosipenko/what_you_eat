using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using System.Windows.Input;
using Coding4Fun.Toolkit.Controls;

namespace Controls
{
    /// <summary>
    /// Message box with yes/no buttons.
    /// </summary>
    public class ConfirmationBox : UserPrompt
    {
        #region Fields

        Button yesButton = new Button();
        Button noButton = new Button();

        #endregion Fields

        public event EventHandler<ConfirmationResulEventArgs> DialogCompleted;

        #region FirstButtonText

        public static readonly DependencyProperty FirstButtonTextProperty = DependencyProperty.Register(
            "FirstButtonText",
            typeof(string),
            typeof(ConfirmationBox),
            new PropertyMetadata(null));

        public string FirstButtonText
        {
            get { return (string)this.GetValue(FirstButtonTextProperty); }
            set { this.SetValue(FirstButtonTextProperty, value); }
        }

        #endregion FirstButtonText

        #region SecondButtonText

        public static readonly DependencyProperty SecondButtonTextProperty = DependencyProperty.Register(
           "SecondButtonText",
           typeof(string),
           typeof(ConfirmationBox),
           new PropertyMetadata(null));

        public string SecondButtonText
        {
            get { return (string)this.GetValue(SecondButtonTextProperty); }
            set { this.SetValue(SecondButtonTextProperty, value); }
        }

        #endregion SecondButtonText

        #region FirstButtonCommand

        public static readonly DependencyProperty FirstButtonCommandProperty = DependencyProperty.Register(
           "FirstButtonCommand",
           typeof(ICommand),
           typeof(ConfirmationBox),
           new PropertyMetadata(null));

        public ICommand FirstButtonCommand
        {
            get { return (ICommand)this.GetValue(FirstButtonCommandProperty); }
            set { this.SetValue(FirstButtonCommandProperty, value); }
        }

        #endregion FirstButtonCommand

        #region SecondButtonCommand

        public static readonly DependencyProperty SecondButtonCommandProperty = DependencyProperty.Register(
           "SecondButtonCommand",
           typeof(ICommand),
           typeof(ConfirmationBox),
           new PropertyMetadata(null));

        public ICommand SecondButtonCommand
        {
            get { return (ICommand)this.GetValue(SecondButtonCommandProperty); }
            set { this.SetValue(SecondButtonCommandProperty, value); }
        }

        #endregion SecondButtonCommand

        public ConfirmationBox()
        {
            this.DefaultStyleKey = typeof(ConfirmationBox);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.yesButton = this.GetTemplateChild("yesButton") as Button;
            this.noButton = this.GetTemplateChild("noButton") as Button;
            if (yesButton != null && noButton != null)
            {
                if (!string.IsNullOrEmpty(this.FirstButtonText))
                    this.yesButton.Content = this.FirstButtonText;
                if (!string.IsNullOrEmpty(this.SecondButtonText))
                    this.noButton.Content = this.SecondButtonText;
                if (this.FirstButtonCommand != null)
                    this.yesButton.Command = this.FirstButtonCommand;
                else
                    this.yesButton.Click += OnYesButtonClick;
                if (this.SecondButtonCommand != null)
                    this.noButton.Command = SecondButtonCommand;
                else
                    this.noButton.Click += OnNoButtonClick;

                var page = this.GetPage();
                page.BackKeyPress += OnBackKeyPress;
            }
        }

        private void OnYesButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.DialogCompleted != null)
            {
                this.DialogCompleted(this, new ConfirmationResulEventArgs() { DialogResult = PopUpResult.Ok });
                this.Hide();
            }
        }

        private void OnNoButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.DialogCompleted != null)
            {
                this.DialogCompleted(this, new ConfirmationResulEventArgs() { DialogResult = PopUpResult.NoResponse });
                this.Hide();
            }
        }

        private void OnBackKeyPress(object sender, EventArgs e)
        {
            if (this.DialogCompleted != null)
            {
                this.DialogCompleted(this, new ConfirmationResulEventArgs() { DialogResult = PopUpResult.UserDismissed });
            }
        }
    }

    public class ConfirmationResulEventArgs : EventArgs
    {
        public PopUpResult DialogResult { get; set; }
    }
}
