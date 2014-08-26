using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public sealed class Constants
    {
        public const int GlassGramms = 200;
        public const int PortionGramms = 350;
        public const float CaloriesInGrammLose = 7.7f;
        public const float CaloriesInGrammPutOn = 11.0f;

        public struct Languages
        {
            public const string EnglishCode = "en-US";
            public const string English = "Enlglish";

            public const string RussianCode = "ru-RU";
            public const string Russian = "Русский";

            public const string UkrainianCode = "uk-UA";
            public const string Ukrainian = "Українська";
        }

        public struct CacheKeys
        {
            public const string Goal = "goal";
            public const string CurrentDate = "current_date";
            public const string EatenFood = "eaten_food";
            public const string SpentToday = "spent_today";
            public const string WaterToday = "water_today";
            public const string User = "user";
            public const string Language = "language";
            public const string DietPlan = "diet_plan";
        }

        public struct Pages
        {
            public const string FristStartUserData = "/Views/DietPlan/UserDataPage.xaml";
            public const string FristStartGoal = "/Views/DietPlan/GoalPage.xaml";
            public const string DesiredWeight = "/Views/DietPlan/DesiredWeight.xaml";
            public const string FoodPlan = "/Views/DietPlan/FoodPlan.xaml";
            public const string LoseWeightPlan = "/Views/DietPlan/LoseWeightPlan.xaml";
            public const string TrainingsPlan = "/Views/DietPlan/TrainingsPlan.xaml";
            public const string Home = "/Views/Home/HomePage.xaml";
            public const string HomePanorama = "/Views/Home/HomePanoramaPage.xaml";
            public const string EatenPage = "/Views/Food/EatenPage.xaml";
            public const string Search = "/Views/SearchPage.xaml";
            public const string FoodDetails = "/Views/Food/FoodDetailsPage.xaml";
            public const string TodayActivity = "/Views/Activity/TodayActivityPage.xaml";
            public const string ActivityDetails = "/Views/Activity/ActivityDetailsPage.xaml";
            public const string DistributeCalories = "/Views/DietPlan/DistributeCalories.xaml";
            public const string Settings = "/Views/SettingsPage.xaml";
            public const string Profile = "/Views/Profile/ProfilePage.xaml";
            public const string Water = "/Views/Water/WaterPage.xaml";
        }

        public struct NavigationParameters
        {
            public const string FoodId = "food_id";
            public const string ActivityId = "activity_id";
            public const string EnergyType = "energy_type";
            public const string FromGoal = "from_goal";
            public const string FromHome = "from_home";
        }
    }
}
