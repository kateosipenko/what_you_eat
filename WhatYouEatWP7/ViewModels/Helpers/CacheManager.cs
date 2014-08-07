using Core.Helpers;
using DataAccess.Repositories;
using DataAccess.Tables;
using IsolatedStorageHelper;
using Models;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ViewModels.Helpers
{
    public class CacheManager
    {
        #region CurrentDay

        private bool wasInitialized = false;
        private DateTime? currentDate;

        private void CheckCurrentDay()
        {
            if (!wasInitialized)
            {
                currentDate = IsolatedStorage.ReadValue<DateTime>(Constants.CacheKeys.CurrentDate);
                eatenFood = IsolatedStorage.ReadValue<List<Food>>(Constants.CacheKeys.EatenFood);
                spentToday = IsolatedStorage.ReadValue<List<PhysicalActivity>>(Constants.CacheKeys.SpentToday);
                if (eatenFood == null)
                    eatenFood = new List<Food>();

                if (spentToday == null)
                    spentToday = new List<PhysicalActivity>();

                wasInitialized = true;
            }

            if (currentDate == null || currentDate.Value.Date != DateTime.Now.Date)
            {
                currentDate = DateTime.Now;
                eatenFood.Clear();
                spentToday.Clear();
                IsolatedStorage.WriteValue(Constants.CacheKeys.CurrentDate, currentDate);
                IsolatedStorage.WriteValue(Constants.CacheKeys.EatenFood, eatenFood);
                IsolatedStorage.WriteValue(Constants.CacheKeys.SpentToday, spentToday);
            }
        }

        #endregion CurrentDay

        #region Singleton

        private static CacheManager instance = new CacheManager();

        private CacheManager()
        {
        }

        public static CacheManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CacheManager();
                }

                return instance;
            }
        }

        #endregion Singleton

        #region User

        private DateTime birthday;
        private BodyState bodyState;


        public DateTime Birthday
        {
            get { return birthday; }
        }

        public BodyState BodyState
        {
            get { return bodyState; }
        }

        public bool IsFirstStart()
        {
            try
            {
                using (BodyStateRepository repo = new BodyStateRepository())
                {
                    bodyState = repo.GetLastState();
                }

                birthday = IsolatedStorage.ReadValue<DateTime>(Constants.CacheKeys.Birthday);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogException(ex);
            }

            return bodyState == null || birthday == default(DateTime);
        }

        #endregion User

        #region Eaten

        private List<Food> eatenFood = new List<Food>();

        public List<Food> GetEatenToday()
        {
            CheckCurrentDay();
            return eatenFood;
        }

        public Food EatFood(Food eaten)
        {
            using (var repo = new FoodRepository())
            {
                repo.EatFood(eaten);
            }

            Food newItem = eaten.CreateCopy();
            int eatenGramms = newItem.AmountOfEaten;
            switch (newItem.FoodMeasure)
            {
                case FoodMeasure.Glass:
                    eatenGramms = newItem.AmountOfEaten * Constants.GlassGramms;
                    break;
                case FoodMeasure.Portion:
                    eatenGramms = newItem.AmountOfEaten * Constants.PortionGramms;
                    break;
            }


            // in db Calories is specified for 100 gramm of product
            newItem.AmountOfCalories = (int)(((float)eatenGramms / 100) * (float)(newItem.Calories));
            eatenFood.Add(newItem);
            IsolatedStorage.WriteValue(Constants.CacheKeys.EatenFood, eatenFood);
            return newItem;
        }

        public void DeleteEatenFood(Food eaten)
        {
            if (eatenFood.Contains(eaten))
            {
                eatenFood.Remove(eaten);
                IsolatedStorage.WriteValue(Constants.CacheKeys.EatenFood, eatenFood);
            }
        }

        #endregion Eaten

        #region Spent

        private List<PhysicalActivity> spentToday = new List<PhysicalActivity>();

        public List<PhysicalActivity> GetSpentToday()
        {
            CheckCurrentDay();
            return spentToday;
        }

        public PhysicalActivity SpentEnergy(PhysicalActivity activity)
        {
            var newActivity = activity.CreateCopy();
            // all calories specified for one kilo per hour - 60minutes
            float caloriesPerBody = (float)bodyState.Weight * newActivity.Calories;
            newActivity.SpentEnergy = (int) (newActivity.GetTotalHours() * caloriesPerBody);
            spentToday.Add(newActivity);
            IsolatedStorage.WriteValue(Constants.CacheKeys.SpentToday, spentToday);
            return newActivity;
        }

        public void DeleteActivity(PhysicalActivity activity)
        {
            if (spentToday.Contains(activity))
            {
                spentToday.Remove(activity);
                IsolatedStorage.WriteValue(Constants.CacheKeys.SpentToday, spentToday);
            }
        }

        #endregion Spent
    }
}
