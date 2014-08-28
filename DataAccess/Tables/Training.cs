using Models;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace DataAccess.Tables
{
    [Table]
    public class Training : RaisableObject
    {
        #region Columns

        #region ID

        private int id;

        [Column(Storage = "Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                if ((this.id != value))
                {
                    this.id = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }

        #endregion ID

        #region Duration

        private int duration;

        /// <summary>
        /// Duration of traingin in minutes.
        /// </summary>
        [Column(Storage = "Duration", DbType = "Int NOT NULL")]
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

        [Column(Storage = "CaloriesMustBurned", DbType = "Int NOT NULL")]
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

        [Column(Storage = "DayOfWeek", DbType = "Int NOT NULL")]
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

        #endregion Columns

        public Training CreateCopy()
        {
            return new Training
            {
                Duration = this.Duration,
                CaloriesMustBurned = this.CaloriesMustBurned,
                DayOfWeek = this.DayOfWeek
            };
        }
    }
}
