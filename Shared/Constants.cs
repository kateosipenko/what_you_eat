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
            public const string User = "user";
            public const string Language = "language";
        }

        public struct Pages
        {
            public const string FristStartUserData = "/Views/FirstStart/UserDataPage.xaml";
            public const string FristStartGoal = "/Views/FirstStart/GoalPage.xaml";
            public const string Home = "/Views/Home/HomePage.xaml";
            public const string EatenPage = "/Views/Food/EatenPage.xaml";
            public const string Search = "/Views/SearchPage.xaml";
            public const string FoodDetails = "/Views/Food/FoodDetailsPage.xaml";
            public const string TodayActivity = "/Views/Activity/TodayActivityPage.xaml";
            public const string ActivityDetails = "/Views/Activity/ActivityDetailsPage.xaml";
            public const string DistributeCalories = "/Views/DietPlan/DistributeCalories.xaml";
            public const string Settings = "/Views/SettingsPage.xaml";
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
