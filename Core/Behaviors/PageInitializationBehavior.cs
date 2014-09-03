using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Navigation;

namespace Core.Behaviors
{
    public class PageInitializationBehavior : Behavior<UserControl>
    {
        private bool isPageLoaded;

        private bool isInitialized;

        private bool isNavigatedBack;

        private volatile object locObject = new object();

        /// <summary>
        /// The uri of the page this behavior is on
        /// </summary>
        private Uri pageSource;

        #region InitializeOnNavigating

        public static readonly DependencyProperty InitializeOnNavigatingProperty = DependencyProperty.Register(
            "InitializeOnNavigating",
            typeof(bool),
            typeof(PageInitializationBehavior),
            new PropertyMetadata(false));

        public bool InitializeOnNavigating
        {
            get { return (bool)GetValue(InitializeOnNavigatingProperty); }
            set { SetValue(InitializeOnNavigatingProperty, value); }
        }

        #endregion InitializeOnNavigating

        #region InitializeCommand

        public ICommand InitializeCommand
        {
            get { return (ICommand)this.GetValue(InitializeCommandProperty); }
            set { this.SetValue(InitializeCommandProperty, value); }
        }

        public static readonly DependencyProperty InitializeCommandProperty =
            DependencyProperty.RegisterAttached("InitializeCommand", typeof(ICommand), typeof(PageInitializationBehavior), null);

        #endregion InitializeCommand

        #region CleanupCommand

        public ICommand CleanupCommand
        {
            get { return (ICommand)this.GetValue(CleanupCommandProperty); }
            set { this.SetValue(CleanupCommandProperty, value); }
        }

        public static readonly DependencyProperty CleanupCommandProperty =
            DependencyProperty.RegisterAttached("CleanupCommand", typeof(ICommand), typeof(PageInitializationBehavior), null);

        #endregion CleanupCommand

        protected override void OnAttached()
        {
            this.AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
            if (Application.Current.RootVisual != null)
            {
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigated += PageInitializationBehavior_Navigated;
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigating += PageInitializationBehavior_Navigating;
            }
            else
            {
                this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            }

            base.OnAttached();
        }

        void PageInitializationBehavior_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
            if (rootVisual != null && rootVisual.CurrentSource != null && pageSource != null)
            {
                this.isNavigatedBack = (e.NavigationMode == NavigationMode.Back && rootVisual.CurrentSource.Equals(pageSource));
                if (!isNavigatedBack && InitializeOnNavigating)
                {
                    OnStartLoading();
                }
            }
            else
            {
                this.isNavigatedBack = false;
            }
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;

            lock (locObject)
            {
                isPageLoaded = true;

                if (isPageLoaded && !isInitialized)
                {
                    isInitialized = true;
                    isPageLoaded = false;
                    this.OnStartLoading();
                }
            }

            (Application.Current.RootVisual as PhoneApplicationFrame).Navigated += PageInitializationBehavior_Navigated;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigating += PageInitializationBehavior_Navigating;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        /// <summary>
        /// Dispose data
        /// </summary>
        private void Cleanup()
        {
            if (this.CleanupCommand != null)
            {
                this.CleanupCommand.Execute(null);
            }


            this.AssociatedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigated -= PageInitializationBehavior_Navigated;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigating -= PageInitializationBehavior_Navigating;
            this.ClearValue(InitializeCommandProperty);
            this.ClearValue(CleanupCommandProperty);
        }

        void PageInitializationBehavior_Navigated(object sender, NavigationEventArgs e)
        {
            isPageLoaded = true;

            if (isNavigatedBack && !e.Uri.Equals(pageSource))
            {
                Cleanup();
                this.Detach();
            }
        }

        private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
        {
            lock (locObject)
            {
                if (isPageLoaded && !isInitialized)
                {
                    isInitialized = true;
                    isPageLoaded = false;
                    this.OnStartLoading();
                }
            }
        }

        protected virtual void OnStartLoading()
        {
            var rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
            if (rootVisual != null && rootVisual.CurrentSource != null)
            {
                this.pageSource = rootVisual.CurrentSource;
            }

            if (this.InitializeCommand != null)
            {
                if (this.AssociatedObject is PhoneApplicationPage && ((PhoneApplicationPage) this.AssociatedObject).NavigationContext != null)
                {
                    this.InitializeCommand.Execute(((PhoneApplicationPage)this.AssociatedObject).NavigationContext.QueryString);
                }
                else
                {
                    this.InitializeCommand.Execute(null);
                }
            }
        }
    }
}
