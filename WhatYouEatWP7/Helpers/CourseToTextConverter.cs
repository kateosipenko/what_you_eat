using Models;
using Resources.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ViewModels.Helpers;

namespace WhatYouEatWP7.Helpers
{
    public class CourseToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            string result = string.Empty;
            if (value is DietPlan)
            {
                result = GetStringByPlan((DietPlan)value);
            }
            else if (parameter is string && ((string) parameter) == "food")
            {
                switch (CacheManager.Instance.Goal.Course)
                {
                    case Course.PutOnWeight:
                        result = CommonStrings.ExtraEat;
                        break;
                    case Course.LoseWeight:
                        result = CommonStrings.NotEat;
                        break;
                }
            }


            return result;
        }

        private string GetStringByPlan(DietPlan plan)
        {
            string result = string.Empty;
            switch (CacheManager.Instance.Goal.Course)
            {
                case Course.LoseWeight:
                    try
                    {
                        result = string.Format(CommonStrings.RedOf, plan.UselessCalories);
                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                case Course.PutOnWeight:
                    try
                    {
                        result = string.Format(CommonStrings.PutOn, plan.RequiredCalories);
                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                case Course.KeepWeight:
                    result = CommonStrings.KeepWeight;
                    break;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
