using GalaSoft.MvvmLight.Command;
using IsolatedStorageHelper;
using Models;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class GoalViewModel : ViewModel
    {
        #region Fields

        private Goal goal = new Goal();
        private List<Course> courses = new List<Course>(3);

        #endregion Fields

        public GoalViewModel()
        {
            courses = new List<Course>(3);
            courses.Add(Course.KeepWeight);
            courses.Add(Course.LoseWeight);
            courses.Add(Course.PutOnWeight);
            Courses = courses;

            SaveAndGoNextCommand = new RelayCommand(SaveAndGoNextExecute, SaveAndGoNextCanExecute);
        }

        #region Properties

        public List<Course> Courses
        {
            get { return courses; }
            private set
            {
                courses = value;
                RaisePropertyChanged("Courses");
            }
        }

        public Goal Goal
        {
            get { return goal; }
            set
            {
                goal = value;
                RaisePropertyChanged("Goal");
            }
        }

        #endregion Properties

        #region SaveAndGoNextCommand

        public RelayCommand SaveAndGoNextCommand { get; private set; }

        private void SaveAndGoNextExecute()
        {
            BusyCount++;
            RunInBackground(() =>
            {
                IsolatedStorage.WriteValue(Constants.CacheKeys.Goal, goal);
                InvokeInUIThread(() =>
                {
                    BusyCount--;
                    NavigationProvider.NavigateAndRemoveBackEntry(Constants.Pages.Home);
                });
            });            
        }

        private bool SaveAndGoNextCanExecute()
        {
            // TODO: implement correct check for goal

            return true;
        }

        #endregion SaveAndGoNextCommand
    }
}
