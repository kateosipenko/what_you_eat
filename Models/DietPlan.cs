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

        #region ThrowOffPerDay

        private int throwOffPerDay;

        /// <summary>
        /// Amount of calories that user has to throw off per day for losing weight by the plan
        /// </summary>
        [DataMember]
        public int ThrowOffPerDay
        {
            get { return throwOffPerDay; }
            set
            {
                throwOffPerDay = value;
                RaisePropertyChanged("ThrowOffPerDay");
            }
        }

        #endregion ThrowOffPerDay

        #region PutOnPerDay

        private int putOnPerDay;

        /// <summary>
        /// Amount of calories that user has to eat per day for put on weight by the plan
        /// </summary>
        [DataMember]
        public int PutOnPerDay
        {
            get { return putOnPerDay; }
            set
            {
                putOnPerDay = value;
                RaisePropertyChanged("PutOnPerDay");
            }
        }

        #endregion PutOnPerDay

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

        #region PlanForFood

        private float planForFood;

        /// <summary>
        /// Procent from daily ThrowOffPerDay value.
        /// </summary>
        [DataMember]
        public float PlanForFood
        {
            get { return planForFood; }
            set
            {
                planForFood = value;
                RaisePropertyChanged("PlanForFood");
            }
        }

        #endregion PlanForFood

        #region PlanForExersizes

        private float planForExersizes;

        /// <summary>
        /// Procent from ThrowOffPerDay value for exersizes
        /// </summary>
        [DataMember]
        public float PlanForExersizes
        {
            get { return planForExersizes; }
            set
            {
                planForExersizes = value;
                RaisePropertyChanged("PlanForExersizes");
            }
        }

        #endregion PlanForExersizes

        #region Water

        private int water = 0;

        /// <summary>
        /// Amount of water in milliliters that user has to drink per day.
        /// </summary>
        [DataMember]
        public int Water
        {
            get { return water; }
            set
            {
                water = value;
                RaisePropertyChanged("Water");
            }
        }

        #endregion Water
    }
}
