using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Controls
{
    public class ExtendedSlider : Slider
    {
        #region Fields

        private TextBlock minimumText;
        private TextBlock valueText;
        private TextBlock maximumText;
        private bool isTemplateApplied = false;

        #endregion Fields

        public ExtendedSlider()
        {
            this.DefaultStyleKey = typeof(ExtendedSlider);
        }

        #region DimensionProperty

        public static readonly DependencyProperty DimensionProperty = DependencyProperty.Register(
            "Dimension",
            typeof(string),
            typeof(ExtendedSlider),
            new PropertyMetadata(null));

        public string Dimension
        {
            get { return (string)GetValue(DimensionProperty); }
            set { SetValue(DimensionProperty, value); }
        }

        #endregion DimensionProperty

        #region HeaderProperty

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(string),
            typeof(ExtendedSlider),
            new PropertyMetadata(null));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        #endregion HeaderProperty

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            minimumText = GetTemplateChild("minimumText") as TextBlock;
            valueText = GetTemplateChild("valueText") as TextBlock;
            maximumText = GetTemplateChild("maximumText") as TextBlock;
            isTemplateApplied = true;
            UpdateText();
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            UpdateText();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            UpdateText();
        }

        private void UpdateText()
        {
            if (isTemplateApplied && Maximum != 0)
            {
                minimumText.Text = (int)Minimum + Dimension;
                valueText.Text = (int)Value + Dimension;
                valueText.Margin = new Thickness((Value * ActualWidth) / Maximum - valueText.ActualWidth, 0, 0, 0);
                maximumText.Text = (int)Maximum + Dimension;
            }
        }
    }
}
