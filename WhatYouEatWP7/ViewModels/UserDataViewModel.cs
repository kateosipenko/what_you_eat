using DataAccess.Repositories;
using DataAccess.Tables;
using GalaSoft.MvvmLight.Command;
using IsolatedStorageHelper;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class UserDataViewModel : ViewModel
    {
        #region Fields

        private BodyState bodyState = new BodyState();
        private DateTime birthday = DateTime.Now;

        #endregion Fields

        public UserDataViewModel()
        {
            this.InitializeViewModelCommand = new RelayCommand(InitializeViewModelExecute);
            SaveAndGoNextCommand = new RelayCommand(this.SaveAndGoNextExecute, this.SaveAndGoNextCanExecute);
        }

        #region Properties
              
        public BodyState BodyState
        {
            get { return bodyState; }
            set
            {
                bodyState = value;
                RaisePropertyChanged("BodyState");
            }
        }

        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                RaisePropertyChanged("Birthday");
                SaveAndGoNextCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion Properties

        protected override void InitializeViewModelExecute()
        {
            base.InitializeViewModelExecute();
            this.BodyState.PropertyChanged += OnBodyStateChanged;
        }

        private void OnBodyStateChanged(object sender, PropertyChangedEventArgs e)
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
                IsolatedStorage.WriteValue(Constants.CacheKeys.Birthday, birthday);
                using (var repo = new BodyStateRepository())
                {
                    var created = repo.Add(bodyState);
                    if (created == null)
                    {
                        HandleError(Messages.Messages.SaveInfoError);
                    }
                    else
                    {
                        InvokeInUIThread(() =>
                        {
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

            return BodyState.Height > 0 && BodyState.Weight > 0
                && Birthday != null;
        }

        #endregion SaveAndGoNextCommand
    }
}
