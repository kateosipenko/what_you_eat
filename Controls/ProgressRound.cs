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
        private ContentPresenter contentPresenter;

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

        #region AnimatedPercent

        public double AnimatedPercent
        {
            get { return (double)GetValue(AnimatedPercentProperty); }
            set { SetValue(AnimatedPercentProperty, value); }
        }

        public static readonly DependencyProperty AnimatedPercentProperty =
            DependencyProperty.Register("AnimatedPercent", typeof(double), typeof(ProgressRound), new PropertyMetadata(0d));

        #endregion AnimatedPercent

        #region Radius

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(ProgressRound), new PropertyMetadata(100d));

        #endregion Radius

        #region ContentTemplate

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(ProgressRound), null);

        #endregion ContentTemplate

        #region InnerRadius

        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("InnerRadius", typeof(double), typeof(ProgressRound), new PropertyMetadata(80d));

        #endregion InnerRadius

        #endregion PROPERTIES

        #region METHODS

        public override void OnApplyTemplate()
        {
            this.ringSlice = this.GetTemplateChild("ringSlice") as RingSlice;
            this.contentPresenter = this.GetTemplateChild("contentPresenter") as ContentPresenter;
            this.contentPresenter.Clip = new EllipseGeometry
            {
                Center = new Point { X = this.Radius, Y = this.Radius },
                RadiusX = this.InnerRadius - 5,
                RadiusY = this.InnerRadius - 5
            };

            base.OnApplyTemplate();
        }

        private void ProgressRoundLoaded(object sender, RoutedEventArgs e)
        {
            Storyboard storyBoard = new Storyboard();

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 360 * this.Percent / 100;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            QuarticEase easingFunc = new QuarticEase();
            easingFunc.EasingMode = EasingMode.EaseOut;
            animation.EasingFunction = easingFunc;
            storyBoard.Children.Add(animation);
            Storyboard.SetTarget(animation, this.ringSlice);
            Storyboard.SetTargetProperty(animation, new PropertyPath(RingSlice.EndAngleProperty));

            DoubleAnimation percentAnimation = new DoubleAnimation();
            percentAnimation.From = 0;
            percentAnimation.To = this.Percent;
            percentAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            QuarticEase percentEasingFunc = new QuarticEase();
            easingFunc.EasingMode = EasingMode.EaseOut;
            percentAnimation.EasingFunction = percentEasingFunc;
            storyBoard.Children.Add(percentAnimation);
            Storyboard.SetTarget(percentAnimation, this);
            Storyboard.SetTargetProperty(percentAnimation, new PropertyPath(ProgressRound.AnimatedPercentProperty));

            storyBoard.Begin();
        }

        #endregion METHODS
    }
}
