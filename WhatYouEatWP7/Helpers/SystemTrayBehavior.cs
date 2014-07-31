namespace WhatYouEatWP7.Helpers
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

    /// <summary>
    /// Behavior for dislpaying system tray for 3 seconds by flip.
    /// </summary>
    public class SystemTrayBehavior : Behavior<UIElement>
    {
        private const int SystemTrayVisibilityDutration = 3000;

        private static DispatcherTimer timer;

        static SystemTrayBehavior()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(SystemTrayVisibilityDutration);
            timer.Tick += OnTimerTick;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseMove += this.AssociatedObject_MouseMove;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.MouseMove -= this.AssociatedObject_MouseMove;
            base.OnDetaching();
        }

        private static void OnTimerTick(object sender, EventArgs e)
        {
            SystemTray.IsVisible = false;
            timer.Stop();
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            SystemTray.Opacity = 0;
            SystemTray.ForegroundColor = (Color)Application.Current.Resources["PhoneForegroundColor"];
            SystemTray.IsVisible = true;
            timer.Start();
        }
    }
}
