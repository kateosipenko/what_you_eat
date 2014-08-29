
using DataAccess.Tables;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Tasks;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ViewModels.Helpers;

namespace ViewModels
{
    public class ProfileViewModel : ViewModel
    {
        public ProfileViewModel()
        {
            SaveCommand = new RelayCommand(SaveExecute);
            UpdatePhotoCommand = new RelayCommand(UpdatePhotoExecute);
        }

        #region ActivitiesTypes

        public Type EnumType
        {
            get { return typeof(ActivityType); }
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
            Diet.UpdateBodyState(BodyState, activityType);
            if (NavigationProvider.CanGoBack())
                NavigationProvider.GoBack();
        }

        #endregion SaveCommand

        #region UserImage

        private ImageSource userImage;

        public ImageSource UserImage
        {
            get { return userImage; }
            set
            {
                userImage = value;
                RaisePropertyChanged("UserImage");
            }
        }

        #endregion UserImage

        #region UpdatePhotoCommand

        public RelayCommand UpdatePhotoCommand { get; private set; }

        private void UpdatePhotoExecute()
        {
            PhotoChooserTask task = new PhotoChooserTask();
            task.ShowCamera = true;
            task.Completed += (sender, args) =>
            {
                if (args.TaskResult == TaskResult.OK)
                {
                    byte[] buffer = new byte[args.ChosenPhoto.Length];
                    args.ChosenPhoto.Read(buffer, 0, buffer.Length);
                    args.ChosenPhoto.Close();
                    InvokeInUIThread(() =>
                    {
                        BodyState.Image = buffer;
                        UserImage = BodyState.GetUserImage();
                    });
                }
            };
            task.Show();
        }

        #endregion UpdatePhotoCommand

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            BodyState = Diet.User.BodyState.CreateCopy();
            ActivityType = Diet.User.ActivityType;
            UserImage = BodyState.GetUserImage();
        }

        public override void Cleanup()
        {
            UserImage = null;
            BodyState = null;
            ActivityType = null;
            base.Cleanup();
        }
    }
}
