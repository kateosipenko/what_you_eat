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
            if (currentDate == null || currentDate.Value.Date != DateTime.Now.Date)
            {
                currentDate = DateTime.Now;
                eatenFood.Clear();
                spentToday.Clear();
                waterToday = 0;
                IsolatedStorage.WriteValue(Constants.CacheKeys.CurrentDate, currentDate);
                IsolatedStorage.WriteValue(Constants.CacheKeys.EatenFood, eatenFood);
                IsolatedStorage.WriteValue(Constants.CacheKeys.SpentToday, spentToday);
                IsolatedStorage.WriteValue(Constants.CacheKeys.WaterToday, waterToday);
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

        public void Initialize()
        {
            if (!wasInitialized)
            {
                user = IsolatedStorage.ReadValue<User>(Constants.CacheKeys.User);
                currentDate = IsolatedStorage.ReadValue<DateTime>(Constants.CacheKeys.CurrentDate);
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (sender, args) =>
                {
                    eatenFood = IsolatedStorage.ReadValue<List<Food>>(Constants.CacheKeys.EatenFood);
                    spentToday = IsolatedStorage.ReadValue<List<PhysicalActivity>>(Constants.CacheKeys.SpentToday);
                    waterToday = IsolatedStorage.ReadValue<int>(Constants.CacheKeys.WaterToday);
                    goal = IsolatedStorage.ReadValue<Goal>(Constants.CacheKeys.Goal);
                    plan = IsolatedStorage.ReadValue<DietPlan>(Constants.CacheKeys.DietPlan);
                    if (eatenFood == null)
                        eatenFood = new List<Food>();

                    if (spentToday == null)
                        spentToday = new List<PhysicalActivity>();
                };

                worker.RunWorkerAsync();
                wasInitialized = true;
            }
        }

        #endregion Singleton

        #region User

        private User user;

        public User User
        {
            get { return user; }
        }

        public bool IsFirstStart()
        {
            return user == null;
        }

        public void SaveUser(User user)
        {
            this.user = user;
            UpdateDietPlan();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                using (var repo = new BodyStateRepository())
                {
                    user.UpdateBodyState(repo.Add(user.BodyState));
                }

                IsolatedStorage.WriteValue(Constants.CacheKeys.User, user);
            };
            worker.RunWorkerAsync();
        }

        public void UpdateBodyState(BodyState state, ActivityType type)
        {
            user.ActivityType = type;
            user.BodyState = state;
            SaveUser(user);
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
            float caloriesPerBody = (float)user.BodyState.Weight * newActivity.Calories;
            newActivity.SpentEnergy = (int)(newActivity.GetTotalHours() * caloriesPerBody);
            spentToday.Add(newActivity);
            IsolatedStorage.WriteValue(Constants.CacheKeys.SpentToday, spentToday);
            if (goal.Course != Course.LoseWeight)
            {
                int totalSpent = spentToday.Sum(item => item.SpentEnergy);
                plan.DailyCalories = plan.NormalPerDay + plan.PutOnPerDay + totalSpent;
            }

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

        #region Water

        private int waterToday = 0;

        public int WaterToday
        {
            get
            {
                CheckCurrentDay();
                return waterToday;
            }
        }

        public void DrinkWater(int amount)
        {
            waterToday += amount;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                IsolatedStorage.WriteValue(Constants.CacheKeys.WaterToday, waterToday);
            };

            worker.RunWorkerAsync();
        }

        public void RemoveFromWater(int amount)
        {
            waterToday -= amount;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                IsolatedStorage.WriteValue(Constants.CacheKeys.WaterToday, waterToday);
            };

            worker.RunWorkerAsync();
        }

        #endregion Water

        #region GoalandDiet

        private Goal goal;
        private DietPlan plan;

        public Goal Goal
        {
            get { return goal; }
        }

        public DietPlan Plan
        {
            get { return plan; }
        }

        public void SaveGoal(Goal goal, float weightDif)
        {
            this.goal = goal;
            switch (goal.Course)
            {
                case Course.LoseWeight:
                    this.goal.DesiredWeight = user.BodyState.Weight - weightDif;
                    break;
                case Course.PutOnWeight:
                    this.goal.DesiredWeight = user.BodyState.Weight + weightDif;
                    break;
            }

            UpdateDietPlan();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                IsolatedStorage.WriteValue(Constants.CacheKeys.Goal, this.goal);
            };

            worker.RunWorkerAsync();
        }

        public void UpdateDietPlan()
        {
            if (plan == null)
            {
                plan = new DietPlan();
            }

            if (user != null && user.BodyState != null && goal != null)
            {
                // weight(gramm) / 450 * 8
                plan.CriticalMinimum = (int)((user.BodyState.Weight * 1000) / 450) * 8;
                if (plan.CriticalMinimum < 1200)
                    plan.CriticalMinimum = 1200;

                int ratio = 5;
                if (user.Sex == Sex.Male)
                    ratio = -161;
                // 999 * weight(kg) + 6.25 * height(sm) - 4/92 * age (-161 for men) (+5 for women)
                plan.Metabolism = (int)(9.99 * user.BodyState.Weight + 6.25 * user.BodyState.Height - 4.92 * user.Age + ratio);
                plan.NormalPerDay = (int)(plan.Metabolism * user.ActivityType.Value);
                int totalSpent = spentToday.Sum(item => item.SpentEnergy);
                plan.DailyCalories = plan.NormalPerDay;
                if (goal.Course != Course.KeepWeight)
                {
                    // weight difference in gramms
                    float weightDif = Math.Abs((user.BodyState.Weight - goal.DesiredWeight) * 1000);
                    float weightPerDay = (weightDif / goal.DesiredWeeksCount) / 7;
                    if (goal.Course == Course.LoseWeight)
                    {
                        // 1gramm = 7.7 calories (for loosing weight)
                        plan.ThrowOffPerDay = weightPerDay != 0 ? (int)(weightPerDay * 7.7) : 0;
                        plan.DailyCalories = plan.NormalPerDay - (int)plan.PlanForFood * plan.ThrowOffPerDay;
                    }
                    else
                    {
                        // 1gramm = 11 calories (for put on weight)
                        plan.PutOnPerDay = weightDif != 0 ? (int)weightPerDay * 11 : 0;
                        plan.DailyCalories = plan.NormalPerDay + plan.PutOnPerDay + totalSpent;
                    }
                }
                else
                {
                    plan.DailyCalories += plan.PutOnPerDay + totalSpent;
                }

                // water in milliliters
                plan.Water = (int) ((user.BodyState.Weight * 0.03) * 1000);
            }

            IsolatedStorage.WriteValue(Constants.CacheKeys.DietPlan, plan);
        }

        public void UpdateDietPlan(DietPlan plan)
        {
            this.plan = plan;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                IsolatedStorage.WriteValue(Constants.CacheKeys.DietPlan, plan);
            };
            worker.RunWorkerAsync();
        }

        #endregion GoalandDiet

        #region Language

        public void SaveCurrentLanguage(Language language)
        {
            IsolatedStorage.WriteValue(Constants.CacheKeys.Language, language);
        }

        public Language GetCurrentLanguage()
        {
            return IsolatedStorage.ReadValue<Language>(Constants.CacheKeys.Language);
        }

        #endregion Language
    }
}
