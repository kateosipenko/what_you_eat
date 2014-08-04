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
using WhatYouEatWP7.Resources.Common;

namespace WhatYouEatWP7.Views.FirstStart
{
    public partial class GoalPage : PhoneApplicationPage
    {
        public GoalPage()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;            
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.coursePicker.SummaryForSelectedItemsDelegate = new Func<IList, string>(OnSummaryDelegate);
        }

        public string OnSummaryDelegate(IList selected)
        {
            if (selected == null || selected.Count == 0)
            {
                goalData.Visibility = Visibility.Collapsed;
                return string.Empty;
            }

            Course course = (Course)selected[0];
            switch (course)
            {
                case Course.KeepWeight:
                    goalData.Visibility = Visibility.Collapsed;
                    break;
                default:
                    goalData.Visibility = Visibility.Visible;
                    break;
            }

            return CommonStrings.ResourceManager.GetString(Enum.GetName(typeof(Course), course));
        }
    }
}