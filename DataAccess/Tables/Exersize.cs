using Models;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace DataAccess.Tables
{
    [Table]
    public class Exersize : RaisableObject
    {
        #region Columns

        #region ID

        private int id;

        [Column(Storage = "Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                RaisePropertyChanged("Id");
            }
        }

        #endregion ID

        #region Date

        private DateTime? date;

        [Column(Storage = "Date", DbType = "DateTime")]
        public DateTime? Date
        {
            get { return date; }
            set
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }

        #endregion Date

        #region Duration

        private int duration;

        /// <summary>
        /// Duration in minutes
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

        #region SpentEnergy

        private int caloriesSpent = 0;

        [Column(Storage = "CaloriesSpent", DbType = "Int NOT NULL")]
        public int CaloriesSpent
        {
            get { return caloriesSpent; }
            set
            {
                caloriesSpent = value;
                RaisePropertyChanged("CaloriesSpent");
            }
        }

        #endregion SpentEnergy

        #region ActivityId

        private int activityId;

        [Column(Name = "ActivityId", DbType = "Int")]
        public int ActivityId
        {
            get { return activityId; }
            set
            {
                activityId = value;
                RaisePropertyChanged("ActivityId");
            }
        }

        #endregion ActivityId

        #region TrainingId

        private int trainingId;

        [Column(Name = "TrainingId", DbType = "Int")]
        public int TrainingId
        {
            get { return trainingId; }
            set
            {
                trainingId = value;
                RaisePropertyChanged("TrainingId");
            }
        }

        #endregion TrainingId

        #endregion Columns

        public Exersize CreateCopy()
        {
            return new Exersize
            {
                Duration = this.Duration,
                CaloriesSpent = this.CaloriesSpent,
                ActivityId = this.ActivityId,
                TrainingId = this.TrainingId
            };
        }
    }
}
