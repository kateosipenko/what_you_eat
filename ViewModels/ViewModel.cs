using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ViewModels.Helpers;

namespace ViewModels
{
    public class ViewModel : ViewModelBase
    {
        #region Fields

        private static bool isNavigationProviderChecked = false;
        private int busyCount = 0;
        protected bool isNetworkAvaible = true;
        private static SynchronizationContext synContext;
        private static NavigationProvider navigationProvider = new NavigationProvider();

        #endregion Fields

        public ViewModel()
        {
            if (synContext == null)
            {
                SynchronizationContextProvider.Initialize();
                synContext = SynchronizationContextProvider.UIThreadSyncContext;
            }

            this.InitializeCommand = new RelayCommand(this.InitializeExecute);
            this.CleanupCommand = new RelayCommand(this.CleanupExecute);
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value that indicates count of current executing process or requests.
        /// </summary>
        public int BusyCount
        {
            get
            {
                return this.busyCount;
            }

            set
            {
                this.busyCount = value;
                if (busyCount < 0)
                    busyCount = 0;
                RaisePropertyChanged("BusyCount");
                RaisePropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Gets a value that indicates is current view model busy now or not.
        /// </summary>
        public bool IsBusy
        {
            get { return this.BusyCount > 0; }
        }

        /// <summary>
        /// Gets provider for executing navigation.
        /// </summary>
        public NavigationProvider NavigationProvider
        {
            get
            {
                return navigationProvider;
            }
        }

        /// <summary>
        /// Gets synchronization context for changing properties in UI thread from background thread.
        /// </summary>
        public SynchronizationContext SyncContext
        {
            get
            {
                return synContext;
            }
        }

        #endregion Properties

        #region Commands

        public RelayCommand InitializeCommand { get; protected set; }
        public RelayCommand CleanupCommand { get; protected set; }

        #endregion Commands

        #region Methods

        /// <summary>
        /// Invokes callback in background thread;
        /// </summary>
        /// <param name="callback">Callback for invokation.</param>
        protected void RunInBackground(Action callback)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                callback.Invoke();
            };
            worker.RunWorkerAsync();
        }

        protected void InvokeInUIThread(Action callback)
        {
            SyncContext.Post((parameter) => callback.Invoke(), null);
        }

        protected void InvokeInUIThread<T>(Action<T> callback, T parameter)
        {
            SyncContext.Post((o) => callback.Invoke(parameter), null);
        }

        protected virtual void InitializeExecute()
        {
            if (!isNavigationProviderChecked)
            {
                this.NavigationProvider.CheckRootFrame();
                isNavigationProviderChecked = true;
            }
        }

        protected virtual void CleanupExecute()
        {
            this.Cleanup();
        }

        protected void HandleError(string error)
        {
            if (error != null)
                InvokeInUIThread(() => MessageBox.Show(error));
        }

        #endregion Methods
    }
}
