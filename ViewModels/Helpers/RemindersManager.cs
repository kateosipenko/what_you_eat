using Core.Helpers;
using IsolatedStorageHelper;
using Microsoft.Phone.Scheduler;
using Models;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ViewModels.Helpers
{
    public class RemindersManager
    {
        #region NavigationUrl

        private const string FoodPage = Constants.Pages.EatenPage;
        private const string TrainingsPage = Constants.Pages.TodayActivity;
        private const string WaterPage = Constants.Pages.Water;

        #endregion NavigationUrl

        private List<UserReminder> userReminders = new List<UserReminder>();

        #region IsTrainingOn

        private bool isForTrainingsOn = false;
        public bool IsTrainingOn { get { return isForTrainingsOn; } }

        #endregion IsTrainingOn

        #region IsWaterOn

        private bool isForWaterOn = false;
        public bool IsWaterOn { get { return IsWaterOn; } }

        #endregion IsWaterOn

        #region IsFoodOn

        private bool isForFoodOn = false;
        public bool IsFoodOn { get { return isForFoodOn; } }

        #endregion IsFoodOn

        #region Singleton

        private static RemindersManager instance;

        private RemindersManager()
        {
            isForFoodOn = IsolatedStorage.ReadValue<bool>(Constants.CacheKeys.AllovedForFood);
            isForTrainingsOn = IsolatedStorage.ReadValue<bool>(Constants.CacheKeys.AllovedForTrainings);
            isForWaterOn = IsolatedStorage.ReadValue<bool>(Constants.CacheKeys.AllovedForWater);
            userReminders = IsolatedStorage.ReadValue<List<UserReminder>>(Constants.CacheKeys.UserReminders);
            if (userReminders == null)
            {
                userReminders = new List<UserReminder>();
            }
        }

        public static RemindersManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RemindersManager();
                }

                return instance;
            }
        }

        #endregion Singleton

        #region OnOffReminders

        #region Food

        public void OnFoodReminders()
        {
            isForFoodOn = true;
            SaveReminders();
        }

        public void OffFoodReminders()
        {
            isForFoodOn = false;
            SaveReminders();
        }

        #endregion Food

        #region Trainings

        public void OnTrainingsReminders()
        {
            isForTrainingsOn = true;
            SaveReminders();
        }

        public void OffTrainingsReminders()
        {
            isForTrainingsOn = false;
            SaveReminders();
        }

        #endregion Trainings

        #region Water

        public void OnWaterReminders()
        {
            isForWaterOn = true;
            SaveReminders();
        }

        public void OffWaterReminders()
        {
            isForWaterOn = false;
            SaveReminders();
        }

        #endregion Water

        public void OnAllReminders()
        {
            isForWaterOn = true;
            isForTrainingsOn = true;
            isForFoodOn = true;
            SaveReminders();
        }

        public void OffAllReminders()
        {
            isForWaterOn = false;
            isForTrainingsOn = false;
            isForFoodOn = false;
            SaveReminders();
        }

        #endregion OnOffReminders

        public void AddReminder(UserReminder reminder)
        {
            userReminders.Add(reminder);
            ScheduleReminder(reminder);
            SaveReminders();

        }

        private void ScheduleReminder(UserReminder userReminder)
        {
            // TODO: implement reminders scheduling
            bool canBeAdded = false;
            string navigationUrl = string.Empty;
            switch (userReminder.Type)
            {
                case EnergyType.Water:
                    canBeAdded = isForWaterOn;
                    navigationUrl = WaterPage;
                    break;
                case EnergyType.Food:
                    canBeAdded = isForFoodOn;
                    navigationUrl = FoodPage;
                    break;
                case EnergyType.Activity:
                    canBeAdded = isForTrainingsOn;
                    navigationUrl = TrainingsPage;
                    break;
            }

            if (canBeAdded)
            {
                // if for all days of week
                if (userReminder.DaysOfWeek.Count == 7)
                {
                    string name = userReminder.GetReminderName(0);
                    var reminder = CreateSystemReminder(userReminder, name, navigationUrl);
                    reminder.RecurrenceType = RecurrenceInterval.Daily;
                    var existing = ScheduledActionService.Find(name);
                    if (existing != null)
                    {
                        ScheduledActionService.Remove(name);
                    }

                    try
                    {
                        ScheduledActionService.Add(reminder);
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.LogException(ex);
                    }
                }
            }
        }

        private Reminder CreateSystemReminder(UserReminder userReminder, string name, string navigationUrl)
        {
            return new Reminder(name)
            {
                Title = userReminder.Title,
                Content = userReminder.Content,
                BeginTime = userReminder.Time,
                NavigationUri = new Uri(navigationUrl, UriKind.Relative)
            };
        }

        private void SaveReminders()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                IsolatedStorage.WriteValue(Constants.CacheKeys.UserReminders, userReminders);
                IsolatedStorage.WriteValue(Constants.CacheKeys.AllovedForWater, isForWaterOn);
                IsolatedStorage.WriteValue(Constants.CacheKeys.AllovedForTrainings, isForTrainingsOn);
                IsolatedStorage.WriteValue(Constants.CacheKeys.AllovedForFood, isForFoodOn);
            };
            worker.RunWorkerAsync();
        }
    }
}
