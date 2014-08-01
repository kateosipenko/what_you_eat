using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Controls.Primitives;

namespace WhatYouEatWP7
{
    public static class VisualTreeHelperExtensions
    {
        public static bool Unfocus(this PhoneApplicationPage page)
        {
            return UnfocusTextBox(page);
        }

        public static double GetActualHeight(this Grid grid)
        {
            double result = 0;
            foreach (var item in grid.Children)
            {
                var element = item as FrameworkElement;
                if (element != null)
                {
                    result += element.ActualHeight;
                    result += element.Margin.Top + element.Margin.Bottom;
                }
            }

            return result;
        }

        private static bool UnfocusTextBox(DependencyObject element)
        {
            bool result = false;
            var childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is TextBox)
                {
                    ((TextBox)child).IsEnabled = false;
                    ((TextBox)child).IsEnabled = true;
                    result = true;
                }
                else if (child is PasswordBox)
                {
                    ((PasswordBox)child).IsEnabled = false;
                    ((PasswordBox)child).IsEnabled = true;
                    result = true;
                }
                else
                {
                    bool wasUnfocus = UnfocusTextBox(child);
                    result = !result && wasUnfocus ? wasUnfocus : result;
                }
            }

            return result;
        }

        public static void DisableCurrentPage(this PhoneApplicationFrame rootFrame)
        {
            var page = (PhoneApplicationPage)rootFrame.Content;
            SetIsEnabledAllControls(page, false);
        }

        public static void EnableCurrenPage(this PhoneApplicationFrame rootFrame)
        {
            var page = (PhoneApplicationPage)rootFrame.Content;
            SetIsEnabledAllControls(page, true);
        }

        private static void SetIsEnabledAllControls(DependencyObject element, bool isEnabled)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is Control && !(child is PhoneApplicationPage) && !(child is Pivot) && !(child is Panorama)
                    && !(child is Grid))
                {
                    ((Control)child).IsEnabled = isEnabled;
                }
                else
                {
                    SetIsEnabledAllControls(child, isEnabled);
                }
            }
        }

        public static void DisableScroll(this WebBrowser browser, Action<double> callback)
        {
            var border = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(browser, 0), 0), 0), 0), 0) as Border;
            border.ManipulationDelta += (s, a) =>
            {
                if (a.DeltaManipulation.Translation.Y != 0)
                {
                    a.Handled = true;
                    callback.Invoke(a.DeltaManipulation.Translation.Y);
                }
            };
        }

        public static ScrollViewer GetScrollViewer(this ItemsControl control)
        {
            return VisualTreeHelper.GetChild((DependencyObject)control, 0) as ScrollViewer;
        }

        public static void ScrollToVerticalOffset(this ItemsControl control, double offset, double actualHeight)
        {
            var scrollViewer = VisualTreeHelper.GetChild((DependencyObject)control, 0) as ScrollViewer;
            double scrollHeight = (offset * scrollViewer.ScrollableHeight) / actualHeight;
            if (double.IsNaN(scrollHeight))
            {
                scrollHeight = 0;
            }

            scrollViewer.ScrollToVerticalOffset(scrollHeight);
        }

        public static double GetOffset(this ItemsControl control, int index)
        {
            double result = 0;
            for (int i = 0; i < index; i++)
            {
                var item = control.ItemContainerGenerator.ContainerFromIndex(i) as ContentPresenter;
                if (item != null)
                {
                    result += item.ActualHeight;
                }
            }

            return result;
        }

        public static ScrollBar GetScrollBar(this ScrollViewer scroll, string name)
        {
            ScrollBar result = new ScrollBar();
            result = GetChildByType<ScrollBar>(scroll, name);
            return result;
        }

        public static T GetChildByType<T>(DependencyObject parent) where T : DependencyObject
        {
            T result = default(T);
            var childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child.GetType() == typeof(T))
                {
                    result = (T)child;
                    break;
                }
                else
                {
                    result = GetChildByType<T>(child);
                }
            }

            return result;
        }

        public static T GetChildByType<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            T result = default(T);
            var childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child.GetType() == typeof(T) && child.GetValue(FrameworkElement.NameProperty) as string == name)
                {
                    result = (T)child;
                    break;
                }
                else
                {
                    result = GetChildByType<T>(child, name);
                }
            }

            return result;
        }

        public static int GetIndexOfVisibleItem(this ScrollViewer scroll, ItemsControl itemsControl)
        {
            int result = 0;
            result = (int)((scroll.VerticalOffset * itemsControl.Items.Count) / scroll.ScrollableHeight) - 1;
            return result;
        }

        public static PhoneApplicationPage GetPage(this UIElement element)
        {
            bool isPageFounded = false;
            var parent = element as DependencyObject;
            PhoneApplicationPage page = new PhoneApplicationPage();
            while (!isPageFounded)
            {
                if (parent is PhoneApplicationPage)
                {
                    page = (PhoneApplicationPage)parent;
                    isPageFounded = true;
                }
                else
                    parent = VisualTreeHelper.GetParent(parent);
            }

            return page;
        }
    }
}
