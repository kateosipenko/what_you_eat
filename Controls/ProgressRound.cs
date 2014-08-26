using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Controls
{
    public class ProgressRound : Control
    {
        #region FIELDS

        private RingSlice ringSlice;

        #endregion FIELDS

        #region CONSTRUCTOR

        public ProgressRound()
        {
            this.DefaultStyleKey = typeof(ProgressRound);
            this.Loaded += this.ProgressRoundLoaded;
        }

        #endregion CONSTRUCTOR

        #region PROPERTIES

        #region Percent

        public int Percent
        {
            get { return (int)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }

        public static readonly DependencyProperty PercentProperty =
            DependencyProperty.Register("Percent", typeof(int), typeof(ProgressRound), new PropertyMetadata(0));

        #endregion Percent

        #region Background

        public SolidColorBrush Background
        {
            get { return (SolidColorBrush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(SolidColorBrush), typeof(SolidColorBrush), new PropertyMetadata(Application.Current.Resources["PhoneAccentBrush"]));
        
        #endregion Background

        #endregion PROPERTIES

        #region METHODS

        public override void OnApplyTemplate()
        {
            this.ringSlice = this.GetTemplateChild("ringSlice") as RingSlice;
            this.ringSlice.Fill = this.Background;
            base.OnApplyTemplate();
        }

        private void ProgressRoundLoaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 360 * this.Percent / 100;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            QuarticEase easingFunc = new QuarticEase();
            easingFunc.EasingMode = EasingMode.EaseOut;
            animation.EasingFunction = easingFunc;
            Storyboard storyBoard = new Storyboard();
            storyBoard.Children.Add(animation);
            Storyboard.SetTarget(animation, this.ringSlice);
            Storyboard.SetTargetProperty(animation, new PropertyPath(RingSlice.EndAngleProperty));
            storyBoard.Begin();
        }

        #endregion METHODS
    }
}
