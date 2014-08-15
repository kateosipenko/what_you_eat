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
using ViewModels.Helpers;

namespace WhatYouEatWP7.Views.Profile
{
    public partial class ProfilePage : BasePage
    {
        public ProfilePage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            activitiesPicker.SummaryForSelectedItemsDelegate = new Func<IList, string>(OnSummaryChanged);
        }

        private string OnSummaryChanged(IList items)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            return EnumsStrings.ResourceManager.GetString(((ActivityType)items[0]).Key);
        }
    }
}