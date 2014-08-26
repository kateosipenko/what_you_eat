﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Models
{
    [DataContract]
    public class FoodPlan : RaisableObject
    {
        #region Metabolism

        private int metabolism;

        /// <summary>
        /// Calories that required for metabolism per day.
        /// </summary>
        [DataMember]
        public int Metabolism
        {
            get { return metabolism; }
            set
            {
                metabolism = value;
                RaisePropertyChanged("Metabolism");
            }
        }

        #endregion Metabolism

        #region NormalPerDay

        private int normalPerDay;

        /// <summary>
        /// Amount of calories per day for keeping current weight
        /// </summary>
        [DataMember]
        public int NormalPerDay
        {
            get { return normalPerDay; }
            set
            {
                normalPerDay = value;
                RaisePropertyChanged("NormalPerDay");
            }
        }

        #endregion NormalPerDay

        #region CriticalMinimum

        private int criticalMinimum;

        /// <summary>
        /// Critical minimum of calories per day which is safe for health
        /// </summary>
        [DataMember]
        public int CriticalMinimum
        {
            get { return criticalMinimum; }
            set
            {
                criticalMinimum = value;
                RaisePropertyChanged("CriticalMinimum");
            }
        }

        #endregion CriticalMinimum

        #region DailyCalories

        private int dailyCalories;

        [DataMember]
        public int DailyCalories
        {
            get { return dailyCalories; }
            set
            {
                dailyCalories = value;
                RaisePropertyChanged("DailyCalories");
            }
        }

        #endregion DailyCalories

        #region Proteins

        private float protein;

        [DataMember]
        public float Protein
        {
            get { return protein; }
            set
            {
                protein = value;
                RaisePropertyChanged("Protein");
            }
        }

        #endregion Proteins

        #region Fats

        private float fats;

        [DataMember]
        public float Fats
        {
            get { return fats; }
            set
            {
                fats = value;
                RaisePropertyChanged("Fats");
            }
        }

        #endregion Fats

        #region Carbohydrates

        private float carbohydrates;

        [DataMember]
        public float Carbohydrates
        {
            get { return carbohydrates; }
            set
            {
                carbohydrates = value;
                RaisePropertyChanged("Carbohydrates");
            }
        }

        #endregion Carbohydrates

        #region MealsCount

        private int mealsCount = 0;

        [DataMember]
        public int MealsCount
        {
            get { return mealsCount; }
            set
            {
                mealsCount = value;
                RaisePropertyChanged("MealsCount");
            }
        }

        #endregion MealsCount
    }
}
