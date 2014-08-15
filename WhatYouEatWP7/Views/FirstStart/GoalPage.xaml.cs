using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections;
using Models;
using ViewModels;
using Resources.Enums;

namespace WhatYouEatWP7.Views.FirstStart
{
    public enum Period
    {
        Until,
        In
    }

    public partial class GoalPage : BasePage
    {
        private bool wasLoaded = false;

        public GoalPage()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!wasLoaded)
            {
                this.coursePicker.SummaryForSelectedItemsDelegate = new Func<IList, string>(OnCourseSummaryChanged);
                this.periodPicker.SummaryForSelectedItemsDelegate = new Func<IList,string>(OnPeriodSummaryChanged);

                List<Period> periods = new List<Period>(2);
                periods.Add(Period.Until);
                periods.Add(Period.In);
                this.periodPicker.ItemsSource = periods;
                wasLoaded = true;
            }
        }

        #region PeriodPicker

        private string OnPeriodSummaryChanged(IList selected)
        {
            if (selected == null || selected.Count == 0)
            {
                inData.Visibility = Visibility.Collapsed;
                untilData.Visibility = Visibility.Collapsed;
                return string.Empty;
            }

            Period period = (Period)selected[0];
            switch (period)
            {
                case Period.In:
                    Locator.GoalStatic.Goal.DesiredDate = null;
                    inData.Visibility = Visibility.Visible;
                    untilData.Visibility = Visibility.Collapsed;
                    break;
                case Period.Until:
                    Locator.GoalStatic.Goal.DesiredWeeksCount = 0;
                    inData.Visibility = Visibility.Collapsed;
                    untilData.Visibility = Visibility.Visible;
                    break;
            }

            return EnumsStrings.ResourceManager.GetString(Enum.GetName(typeof(Period), period));
        }

        #endregion PeriodPicker

        #region CoursePicker

        private string OnCourseSummaryChanged(IList selected)
        {
            if (selected == null || selected.Count == 0)
            {
                goalData.Visibility = Visibility.Collapsed;
                inData.Visibility = Visibility.Collapsed;
                untilData.Visibility = Visibility.Collapsed;
                return string.Empty;
            }

            Course course = (Course)selected[0];
            switch (course)
            {
                case Course.KeepWeight:
                    goalData.Visibility = Visibility.Collapsed;
                    inData.Visibility = Visibility.Collapsed;
                    untilData.Visibility = Visibility.Collapsed;
                    break;
                default:
                    goalData.Visibility = Visibility.Visible;
                    break;
            }

            return EnumsStrings.ResourceManager.GetString(Enum.GetName(typeof(Course), course));
        }

        #endregion CoursePicker
    }
}