using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public sealed class Constants
    {
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
            public const string CurrentLanguage = "current_languge";
            public const string Birthday = "birthday";
            public const string Goal = "goal";
        }

        public struct Pages
        {
            public const string FristStartUserData = "/Views/FirstStart/UserDataPage.xaml";
            public const string FristStartGoal = "/Views/FirstStart/GoalPage.xaml";
            public const string Home = "/Views/Home/HomePage.xaml";
        }
    }
}
