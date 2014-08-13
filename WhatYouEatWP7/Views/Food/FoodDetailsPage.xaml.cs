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

namespace WhatYouEatWP7.Views.Food
{
    public partial class FoodDetailsPage : PhoneApplicationPage
    {
        public FoodDetailsPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            List<FoodMeasure> items = new List<FoodMeasure>(3);
            items.Add(FoodMeasure.Gramm);
            items.Add(FoodMeasure.Glass);
            items.Add(FoodMeasure.Portion);
            foodMeasure.SummaryForSelectedItemsDelegate += OnSummaryDelegate;
            foodMeasure.ItemsSource = items;
        }

        private string OnSummaryDelegate(IList items)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            return EnumsStrings.ResourceManager.GetString(Enum.GetName(items[0].GetType(), items[0]));
        }
    }
}