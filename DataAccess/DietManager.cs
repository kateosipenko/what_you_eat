using DataAccess.Repositories;
using DataAccess.Tables;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Shared;
using IsolatedStorageHelper;

namespace DataAccess
{
    public class DietManager
    {
        #region DietManager

        private static DietManager instance;

        private DietManager() { }

        public static DietManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DietManager();
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
                RunInBackground(() =>
                {
                    eatenFood = IsolatedStorage.ReadValue<List<Food>>(Constants.CacheKeys.EatenFood);
                    waterToday = IsolatedStorage.ReadValue<int>(Constants.CacheKeys.WaterToday);
                    goal = IsolatedStorage.ReadValue<Goal>(Constants.CacheKeys.Goal);
                    plan = IsolatedStorage.ReadValue<DietPlan>(Constants.CacheKeys.DietPlan);
                    if (eatenFood == null)
                        eatenFood = new List<Food>();

                    using (var exersizeRepo = new ExersizesRepository())
                    {
                        spentToday = exersizeRepo.GetForToday();
                    }

                    if (spentToday == null)
                        spentToday = new List<Exersize>();
                });
                
                wasInitialized = true;
            }
        }

        #endregion DietManager

        #region User

        private User user;

        public User User
        {
            get { return user; }
        }

        public bool IsFirstStart()
        {
            //TODO: remove fake
            return true;
            //return user == null;
        }

        public void SaveUser(User user)
        {
            this.user = user;
            UpdateDietPlan();
            RunInBackground(() =>
            {
                using (var repo = new BodyStateRepository())
                {
                    user.UpdateBodyState(repo.Add(user.BodyState));
                }

                IsolatedStorage.WriteValue(Constants.CacheKeys.User, user);
            });
        }

        public void UpdateBodyState(BodyState state, ActivityType type)
        {
            user.ActivityType = type;
            user.BodyState = state;
            SaveUser(user);
        }

        #endregion User

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
                IsolatedStorage.WriteValue(Constants.CacheKeys.WaterToday, waterToday);
                using (var repo = new ExersizesRepository())
                {
                    repo.DeleteOldExersizes();
                }
            }
        }

        #endregion CurrentDay

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

        private List<Exersize> spentToday = new List<Exersize>();

        public List<Exersize> GetSpentToday()
        {
            CheckCurrentDay();
            return spentToday;
        }

        public Exersize DoExersize(Exersize invoked)
        {
            Exersize exersize = invoked.CreateCopy();
            // all calories specified for one kilo per minute
            float caloriesPerBody = 0;
            using (var activityRepo = new PhysicalActivityRepository())
            {
                var activity = activityRepo.GetById(invoked.ActivityId);
                caloriesPerBody = (float)(user.BodyState.Weight * activity.Calories) / 60;
            }
            
            exersize.CaloriesSpent = (int)(invoked.Duration * caloriesPerBody);
            using (var repository = new ExersizesRepository())
            {
                exersize = repository.Add(exersize);
            }


            spentToday.Add(exersize);
            if (goal.Course != Course.LoseWeight)
            {
                int totalSpent = spentToday.Sum(item => item.CaloriesSpent);
                plan.FoodPerDay.DailyCalories = plan.FoodPerDay.NormalPerDay + plan.FoodPerDay.ExtraCaloriesPerDay + totalSpent;
            }

            return exersize;
        }

        public void DeleteExersize(Exersize exersize)
        {
            if (spentToday.Contains(exersize))
            {
                spentToday.Remove(exersize);
                using (var repo = new ExersizesRepository())
                {
                    repo.Delete(exersize);
                }
            }
        }

        public int GetMustSpentToday()
        {
            int result = 0;
            if (plan != null)
            {
                result = plan.Trainigs.Where(item => item.DayOfWeek == DateTime.Now.DayOfWeek).Sum(el => el.CaloriesMustBurned);
            }

            return result;
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
            RunInBackground(() =>
            {
                IsolatedStorage.WriteValue(Constants.CacheKeys.WaterToday, waterToday);
            });
        }

        public void RemoveFromWater(int amount)
        {
            waterToday -= amount;
            RunInBackground(() =>
            {
                IsolatedStorage.WriteValue(Constants.CacheKeys.WaterToday, waterToday);
            });
        }

        #endregion Water

        #region Goal

        private Goal goal;

        public Goal Goal
        {
            get { return goal; }
        }

        public void SaveGoal(Goal goal)
        {
            this.goal = goal;
            UpdateDietPlan();
            RunInBackground(() =>
            {
                IsolatedStorage.WriteValue(Constants.CacheKeys.Goal, this.goal);
            });
        }

        #endregion Goal

        #region DietPlan

        private DietPlan plan;

        public DietPlan Plan
        {
            get { return plan; }
        }

        public void UpdateDietPlan()
        {
            if (plan == null)
            {
                plan = new DietPlan();
            }

            if (user != null && user.BodyState != null && goal != null)
            {
                plan.Clear();

                // weight(gramm) / 450 * 8
                plan.FoodPerDay.CriticalMinimum = (int)((user.BodyState.Weight * 1000) / 450) * 8;
                if (plan.FoodPerDay.CriticalMinimum < 1200)
                    plan.FoodPerDay.CriticalMinimum = 1200;

                int ratio = 5;
                if (user.Sex == Sex.Male)
                    ratio = -161;
                // 999 * weight(kg) + 6.25 * height(sm) - 4/92 * age (-161 for men) (+5 for women)
                plan.FoodPerDay.Metabolism = (int)(9.99 * user.BodyState.Weight + 6.25 * user.BodyState.Height - 4.92 * user.Age + ratio);
                plan.FoodPerDay.NormalPerDay = (int)(plan.FoodPerDay.Metabolism * user.ActivityType.Value);
                plan.FoodPerDay.MealsCount = 4;
                CalculateFoodAndTrainingPlan();
                // water in milliliters
                plan.WaterPlan.Amount = (int)((user.BodyState.Weight * 0.03) * 1000);
                plan.WaterPlan.IntakeCount = 7;
            }

            IsolatedStorage.WriteValue(Constants.CacheKeys.DietPlan, plan);
        }

        private void CalculateFoodAndTrainingPlan()
        {
            plan.FoodPerDay.DailyCalories = plan.FoodPerDay.NormalPerDay;
            float weightDif = Math.Abs((user.BodyState.Weight - goal.DesiredWeight) * 1000);
            float weightPerWeek = (weightDif / goal.DesiredWeeksCount);
            switch (goal.Course)
            {
                case Course.KeepWeight:
                case Course.UserPlan:
                    plan.FoodPerDay.Carbohydrates = 64;
                    plan.FoodPerDay.Protein = 20;
                    plan.FoodPerDay.Fats = 16;
                    break;
                case Course.LoseWeight:
                    plan.ThrowOffPerWeek = (int)weightPerWeek;
                    plan.FoodPerDay.UselessCaloriesPerDay = (int)(((plan.ThrowOffPerWeek * plan.ProcentForFood) / 100 / 7) * Constants.CaloriesInGrammLose);
                    plan.FoodPerDay.DailyCalories = plan.FoodPerDay.DailyCalories - plan.FoodPerDay.UselessCaloriesPerDay;
                    plan.MustSpentPerWeek = (int)((plan.ThrowOffPerWeek * plan.ProcentForTrainings / 100) * Constants.CaloriesInGrammLose);
                    plan.FoodPerDay.Carbohydrates = 45;
                    plan.FoodPerDay.Protein = 35;
                    plan.FoodPerDay.Fats = 20;
                    break;
                case Course.PutOnWeight:
                    plan.PutOnPerWeek = (int)weightPerWeek;
                    plan.FoodPerDay.ExtraCaloriesPerDay = (int)((plan.PutOnPerWeek / 7) * Constants.CaloriesInGrammPutOn);
                    plan.FoodPerDay.DailyCalories = plan.FoodPerDay.DailyCalories + plan.FoodPerDay.ExtraCaloriesPerDay;
                    plan.FoodPerDay.Carbohydrates = 50;
                    plan.FoodPerDay.Protein = 30;
                    plan.FoodPerDay.Fats = 20;
                    break;
            }
        }

        public void SaveDietPlan()
        {
            RunInBackground(() =>
            {
                IsolatedStorage.WriteValue(Constants.CacheKeys.DietPlan, plan);
            });
        }

        #endregion DietPlan

        private void RunInBackground(Action action)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                action.Invoke();
            };
            worker.RunWorkerAsync();
        }
    }
}
