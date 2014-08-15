using GalaSoft.MvvmLight.Command;
using Models;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModels.Helpers;

namespace ViewModels
{
    public class GoalViewModel : ViewModel
    {
        #region Fields

        private Goal goal = new Goal();
        private float weightDif;

        #endregion Fields

        public GoalViewModel()
        {
            SaveAndGoNextCommand = new RelayCommand(SaveAndGoNextExecute);
            Courses.Add(Course.KeepWeight);
            Courses.Add(Course.LoseWeight);
            Courses.Add(Course.PutOnWeight);
        }

        #region AllCourses

        private List<Course> courses = new List<Course>(3);

        public List<Course> Courses
        {
            get { return courses; }
            set
            {
                courses = value;
                RaisePropertyChanged("Courses");
            }
        }

        #endregion AllCourses

        #region Properties

        public Goal Goal
        {
            get { return goal; }
            set
            {
                goal = value;
                RaisePropertyChanged("Goal");
            }
        }

        public float WeightDif
        {
            get { return weightDif; }
            set
            {
                weightDif = value;
                RaisePropertyChanged("WeightDif");
            }
        }

        #endregion Properties

        #region SaveAndGoNextCommand

        public RelayCommand SaveAndGoNextCommand { get; private set; }

        private void SaveAndGoNextExecute()
        {
            CacheManager.Instance.SaveGoal(goal, weightDif);
            NavigationProvider.NavigateAndRemoveBackEntry(Constants.Pages.DistributeCalories.AddPageParameter(Constants.NavigationParameters.FromGoal, true));
        }

        #endregion SaveAndGoNextCommand

        #region Cleanup

        public override void Cleanup()
        {
            base.Cleanup();
            this.Goal = null;
            this.WeightDif = 0;
        }

        #endregion Cleanup
    }
}
