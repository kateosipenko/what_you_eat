using Controls;
using DataAccess.Repositories;
using DataAccess.Tables;
using GalaSoft.MvvmLight.Command;
using Models;
using Resources.Buttons;
using Resources.Common;
using Resources.Errors;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ViewModels.Helpers;

namespace ViewModels
{
    public class UserDataViewModel : ViewModel
    {
        #region Fields

        private User user = new User();

        #endregion Fields

        public UserDataViewModel()
        {
            this.InitializeCommand = new RelayCommand(InitializeExecute);
            SaveAndGoNextCommand = new RelayCommand(this.SaveAndGoNextExecute, this.SaveAndGoNextCanExecute);
        }

        #region Properties

        public Type ActivityType
        {
            get { return typeof(ActivityType); }
        }

        public Type SexType
        {
            get { return typeof(Sex); }
        }

        public User User
        {
            get { return user; }
            set
            {
                user = value;
                RaisePropertyChanged("User");
            }
        }

        #endregion Properties

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            this.User.PropertyChanged += OnUserDataChanged;
        }

        private void OnUserDataChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SaveAndGoNextCommand.RaiseCanExecuteChanged();
        }

        #region SaveAndGoNextCommand

        public RelayCommand SaveAndGoNextCommand { get; private set; }

        private void SaveAndGoNextExecute()
        {
            Diet.SaveUser(user);
            ConfirmationBox popup = new ConfirmationBox();
            popup.Message = CommonStrings.SetupPlanNow;
            popup.FirstButtonText = ButtonsStrings.SetupNow;
            popup.SecondButtonText = ButtonsStrings.SetupLater;
            popup.FirstButtonCommand = new RelayCommand(() =>
            {
                InvokeInUIThread(() => NavigationProvider.NavigateAndRemoveBackEntry(Constants.Pages.FristStartGoal));
            });

            popup.SecondButtonCommand = new RelayCommand(() =>
            {
                InvokeInUIThread(() => NavigationProvider.NavigateAndRemoveBackEntry(Constants.Pages.Home));
            });

            popup.Show();
        }

        private bool SaveAndGoNextCanExecute()
        {
            // TODO: implement correct check for brithday

            return user.Birthday != null && user.Birthday.Value.Year > (DateTime.Now.Year - 100)
                && user.Birthday.Value.Year < (DateTime.Now.Year - 14)
                && user.BodyState.Height > 130 && user.BodyState.Height < 250
                && user.BodyState.Weight > 30 && user.BodyState.Weight < 300;
        }

        #endregion SaveAndGoNextCommand

        #region Cleanup

        protected override void CleanupExecute()
        {
            this.User = null;
            base.CleanupExecute();
        }

        #endregion Cleanup
    }
}
