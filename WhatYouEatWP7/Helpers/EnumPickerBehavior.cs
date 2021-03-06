﻿using Microsoft.Phone.Controls;
using Models;
using Resources.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WhatYouEatWP7.Helpers
{
    public class EnumPickerBehavior : Behavior<ListPicker>
    {
        private List<Object> items = new List<object>();

        #region EnumTypeProperty

        public static readonly DependencyProperty EnumTypeProperty = DependencyProperty.Register(
            "EnumType",
            typeof(Type),
            typeof(EnumPickerBehavior),
            new PropertyMetadata(OnEnumTypeChanged));

        public static void OnEnumTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((EnumPickerBehavior)sender).OnEnumTypeChanged();
        }

        public Type EnumType
        {
            get { return (Type)GetValue(EnumTypeProperty); }
            set { SetValue(EnumTypeProperty, value); }
        }

        #endregion EnumTypeProperty

        #region SelectedItem

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem",
            typeof(object),
            typeof(EnumPickerBehavior),
            new PropertyMetadata(OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((EnumPickerBehavior)sender).OnSeletedItemChanged();
        }

        private void OnSeletedItemChanged()
        {
            if (this.AssociatedObject == null)
                return;

            if (this.AssociatedObject.SelectedItem == null && SelectedItem != null && items != null && items.Count > 0)
            {
                if (EnumType == typeof(ActivityType) && SelectedItem is ActivityType)
                {
                    foreach (var item in items)
                    {

                        if (((ActivityType)item).Key == ((ActivityType)SelectedItem).Key)
                        {
                            SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    SelectedItem = this.items.SingleOrDefault(item => item == SelectedItem);
                }

                AssociatedObject.SelectedItem = this.SelectedItem;
                AssociatedObject.OnSelectedItemChanged(null, SelectedItem);
            }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        #endregion SelectedItem

        protected override void OnAttached()
        {
            base.OnAttached();
            OnEnumTypeChanged();
            OnSeletedItemChanged();
        }

        protected override void OnDetaching()
        {
            items.Clear();
            this.AssociatedObject.SelectionChanged -= OnSelectionChanged;
            this.AssociatedObject.ItemsSource = null;
            this.AssociatedObject.SummaryForSelectedItemsDelegate = null;
            base.OnDetaching();
        }

        private void OnEnumTypeChanged()
        {
            if (this.EnumType != null && this.AssociatedObject != null)
            {
                var fields = EnumType.GetFields();
                if (fields.Length > 1)
                {
                    int startIndex = EnumType == typeof(ActivityType) ? 0 : 1;
                    for (int i = startIndex; i < fields.Length; i++)
                    {
                        items.Add(fields[i].GetValue(null));
                    }
                }

                this.AssociatedObject.SelectionChanged += OnSelectionChanged;
                this.AssociatedObject.ItemsSource = items;
                this.AssociatedObject.SummaryForSelectedItemsDelegate = new Func<IList, string>(OnSummaryChanged);
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItem = AssociatedObject.SelectedItem;
        }

        private string OnSummaryChanged(IList selectedItems)
        {
            if (selectedItems == null || selectedItems.Count == 0)
                return string.Empty;

            if (selectedItems[0] is ActivityType)
                return EnumsStrings.ResourceManager.GetString(((ActivityType)selectedItems[0]).Key);

            return EnumsStrings.ResourceManager.GetString(Enum.GetName(EnumType, selectedItems[0]));
        }
    }
}
