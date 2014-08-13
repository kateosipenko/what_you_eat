using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Models;
using System.Collections;
using Resources.Enums;
using ViewModels;

namespace WhatYouEatWP7.Views.FirstStart
{
    public partial class UserDataPage : BasePage
    {
        public UserDataPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            activityPicker.SummaryForSelectedItemsDelegate = new Func<IList,string>(OnSummaryChanged);
            sexPicker.SummaryForSelectedItemsDelegate = new Func<IList,string>(OnSummaryChanged);

            List<Sex> itemsSource = new List<Sex>(2);
            itemsSource.Add(Sex.Male);
            itemsSource.Add(Sex.Female);
            sexPicker.ItemsSource = itemsSource;

            List<ActivityType> activitySource = new List<ActivityType>();
            activitySource.Add(ActivityType.Sitting);
            activitySource.Add(ActivityType.Light);
            activitySource.Add(ActivityType.Medium);
            activitySource.Add(ActivityType.High);
            activitySource.Add(ActivityType.Extreme);
            activityPicker.ItemsSource = activitySource;
        }

        private string OnSummaryChanged(IList items)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items[0] is ActivityType)
                return EnumsStrings.ResourceManager.GetString(((ActivityType)items[0]).Key);

            return EnumsStrings.ResourceManager.GetString(Enum.GetName(items[0].GetType(), items[0]));
        }
    }
}