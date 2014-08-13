using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Models
{
    [DataContract]
    public class DietPlan : RaisableObject
    {
        #region MetabolismCalories

        private int metabolismCalories;

        [DataMember]
        public int MetabolismCalories
        {
            get { return metabolismCalories; }
            set
            {
                metabolismCalories = value;
                RaisePropertyChanged("MetabolismCalories");
            }
        }

        #endregion MetabolismCalories

        #region NormalCaloriesPerDay

        private int normalCaloriesPerDay;

        [DataMember]
        public int NormalCaloriesPerDay
        {
            get { return normalCaloriesPerDay; }
            set
            {
                normalCaloriesPerDay = value;
                RaisePropertyChanged("NormalCaloriesPerDay");
            }
        }

        #endregion NormalCaloriesPerDay

        #region CriticalCaloriesMin

        private int criticalCaloriesMin;

        [DataMember]
        public int CriticalCaloriesMin
        {
            get { return criticalCaloriesMin; }
            set
            {
                criticalCaloriesMin = value;
                RaisePropertyChanged("CriticalCaloriesMin");
            }
        }

        #endregion CriticalCaloriesMin

        #region UselessCalories

        private int uselessCalories;

        [DataMember]
        public int UselessCalories
        {
            get { return uselessCalories; }
            set
            {
                uselessCalories = value;
                RaisePropertyChanged("UselessCalories");
            }
        }

        #endregion UselessCalories

        #region RequiredCalories

        private int requiredCalories;

        [DataMember]
        public int RequiredCalories
        {
            get { return requiredCalories; }
            set
            {
                requiredCalories = value;
                RaisePropertyChanged("RequiredCalories");
            }
        }

        #endregion RequiredCalories

        #region CaloriesPerDay

        private int caloriesPerDay;

        [DataMember]
        public int CaloriesPerDay
        {
            get { return caloriesPerDay; }
            set
            {
                caloriesPerDay = value;
                RaisePropertyChanged("CaloriesPerDay");
            }
        }

        #endregion CaloriesPerDay

        #region ExersizesPerDay

        private int exersizesPerDay;

        [DataMember]
        public int ExersizesPerDay
        {
            get { return exersizesPerDay; }
            set
            {
                exersizesPerDay = value;
                RaisePropertyChanged("ExersizesPerDay");
            }
        }

        #endregion ExersizesPerDay
    }
}
