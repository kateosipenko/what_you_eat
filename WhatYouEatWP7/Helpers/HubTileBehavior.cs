using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace WhatYouEatWP7.Helpers
{
    public class HubTileBehavior : Behavior<HubTile>
    {
        #region StartBrush

        public static readonly DependencyProperty StartBrushProperty = DependencyProperty.Register(
            "StartBrush",
            typeof(Color),
            typeof(HubTileBehavior),
            new PropertyMetadata(null));

        public Color StartBrush
        {
            get { return (Color)GetValue(StartBrushProperty); }
            set { SetValue(StartBrushProperty, value); }
        }

        #endregion StartBrush

        #region EndBrush

        public static readonly DependencyProperty EndBrushProperty = DependencyProperty.Register(
            "EndBrush",
            typeof(Color),
            typeof(HubTileBehavior),
            new PropertyMetadata(null));

        public Color EndBrush
        {
            get { return (Color)GetValue(EndBrushProperty); }
            set { SetValue(EndBrushProperty, value); }
        }

        #endregion EndBrush

        #region Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(HubTileBehavior),
            new PropertyMetadata(OnValueChanged));

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((HubTileBehavior)sender).OnValueChanged();
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion Value

        #region MaxValue

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue",
            typeof(double),
            typeof(HubTileBehavior),
            new PropertyMetadata(OnValueChanged));

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, MaxValue); }
        }

        #endregion MaxValue

        #region Measure

        public static readonly DependencyProperty MeasureProperty = DependencyProperty.Register(
            "Measure",
            typeof(string),
            typeof(HubTileBehavior),
            new PropertyMetadata(null));

        public string Measure
        {
            get { return (string)GetValue(MeasureProperty); }
            set { SetValue(MeasureProperty, value); }
        }

        #endregion Measure

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnValueChanged();            
        }

        private void OnValueChanged()
        {
            AssociatedObject.Message = Value + Measure;
            if(MaxValue != 0)
                AssociatedObject.Message += "/ " + MaxValue + Measure;

            GradientStopCollection colors = new GradientStopCollection();
            var startGradient = new GradientStop();
            startGradient.Color = StartBrush;
            startGradient.Offset = MaxValue == 0 ? MaxValue : Value / MaxValue;

            var endGradient = new GradientStop();
            endGradient.Color = EndBrush;
            endGradient.Offset = MaxValue == 0 ? MaxValue : Value / MaxValue - 0.2;

            colors.Add(startGradient);
            colors.Add(endGradient);

            var gradient = new LinearGradientBrush(colors, 0);
            gradient.StartPoint = new Point(0.5, 0);
            gradient.EndPoint = new Point(0.5, 1);

            this.AssociatedObject.Background = gradient;
        }
    }
}
