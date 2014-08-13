using DataAccess.Repositories;
using DataAccess.Tables;
using GalaSoft.MvvmLight.Command;
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
            this.InitializeViewModelCommand = new RelayCommand(InitializeViewModelExecute);
            SaveAndGoNextCommand = new RelayCommand(this.SaveAndGoNextExecute, this.SaveAndGoNextCanExecute);
        }

        #region Properties

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

        protected override void InitializeViewModelExecute()
        {
            base.InitializeViewModelExecute();
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
            BusyCount++;
            RunInBackground(() =>
            {
                
                using (var repo = new BodyStateRepository())
                {
                    var created = repo.Add(user.BodyState);
                    if (created == null)
                    {
                        HandleError(ErrorsStrings.SaveInfoError);                        
                    }
                    else
                    {                        
                        InvokeInUIThread(() =>
                        {
                            user.BodyState = created;
                            CacheManager.Instance.SaveUser(user);
                            NavigationProvider.NavigateAndRemoveBackEntry(Constants.Pages.FristStartGoal);
                        });
                    }
                    
                    InvokeInUIThread(() => BusyCount--);
                }
            });
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
    }
}
