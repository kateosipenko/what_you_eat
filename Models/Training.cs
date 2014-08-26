using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Models
{
    [DataContract]
    public class Training : RaisableObject
    {
        #region Duration

        private int duration;

        /// <summary>
        /// Duration of traingin in minutes.
        /// </summary>
        [DataMember]
        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                RaisePropertyChanged("Duration");
            }
        }

        #endregion Duration

        #region CaloriesMustBurned

        private int caloriesMustBurned = 0;

        [DataMember]
        public int CaloriesMustBurned
        {
            get { return caloriesMustBurned; }
            set
            {
                caloriesMustBurned = value;
                RaisePropertyChanged("CaloriesMustBurned");
            }
        }

        #endregion CaloriesMustBurned

        #region DayOfWeek

        private DayOfWeek dayOfWeek;

        [DataMember]
        public DayOfWeek DayOfWeek
        {
            get { return dayOfWeek; }
            set
            {
                dayOfWeek = value;
                RaisePropertyChanged("DayOfWeek");
            }
        }

        #endregion DayOfWeek
    }
}
