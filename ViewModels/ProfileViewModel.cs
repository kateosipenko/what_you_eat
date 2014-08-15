
using DataAccess.Tables;
using GalaSoft.MvvmLight.Command;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModels.Helpers;

namespace ViewModels
{
    public class ProfileViewModel : ViewModel
    {
        public ProfileViewModel()
        {
            SaveCommand = new RelayCommand(SaveExecute);
            AllTypes.Add(ActivityType.Sitting);
            AllTypes.Add(ActivityType.Light);
            AllTypes.Add(ActivityType.Medium);
            AllTypes.Add(ActivityType.High);
            AllTypes.Add(ActivityType.Extreme);
        }

        #region ActivitiesTypes

        private List<ActivityType> allTypes = new List<ActivityType>();

        public List<ActivityType> AllTypes
        {
            get { return this.allTypes; }
            set
            {
                this.allTypes = value;
                RaisePropertyChanged("AllTypes");
            }
        }

        #endregion ActivitiesTypes

        #region BodyState

        private BodyState bodyState;

        public BodyState BodyState
        {
            get { return bodyState; }
            set
            {
                bodyState = value;
                RaisePropertyChanged("BodyState");
            }
        }

        #endregion BodyState

        #region ActivityType

        private ActivityType activityType;

        public ActivityType ActivityType
        {
            get { return activityType; }
            set
            {
                activityType = value;
                RaisePropertyChanged("ActivityType");
            }
        }

        #endregion ActivityType

        #region SaveCommand

        public RelayCommand SaveCommand { get; private set; }

        private void SaveExecute()
        {
            CacheManager.Instance.UpdateBodyState(BodyState, activityType);
            if (NavigationProvider.CanGoBack())
                NavigationProvider.GoBack();
        }

        #endregion SaveCommand

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            BodyState = CacheManager.Instance.User.BodyState;
            ActivityType = AllTypes.SingleOrDefault(item => item.Value == CacheManager.Instance.User.ActivityType.Value);
        }

        public override void Cleanup()
        {
            BodyState = null;
            ActivityType = null;
            base.Cleanup();
        }
    }
}
