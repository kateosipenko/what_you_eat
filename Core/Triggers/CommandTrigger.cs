using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Core.Triggers
{
    public class CommandTrigger : TriggerAction<FrameworkElement>
    {
        #region Command

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(CommandTrigger),
            new PropertyMetadata(null));

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

        #endregion Command

        #region CommandParameter

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(CommandTrigger),
            new PropertyMetadata(null));

        public object CommandParameter
        {
            get
            {
                return this.GetValue(CommandParameterProperty);
            }

            set
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        #endregion CommandParameter

        protected override void Invoke(object parameter)
        {
            if (this.Command != null && this.Command.CanExecute(parameter))
            {
                this.Command.Execute(this.CommandParameter);
            }
        }
    }
}
