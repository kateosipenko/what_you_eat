using Models;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace DataAccess.Tables
{
    [Table]
    public class PhysicalActivity : RaisableObject
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

        #region StringId

        private string stringId;

        [Column(Storage = "StringId", DbType = "NVarChar(256) NOT NULL")]
        public string StringId
        {
            get { return stringId; }
            set
            {
                stringId = value;
                RaisePropertyChanged("StringId");
            }
        }

        #endregion StringId

        #region Calories

        private float calories;

        /// <summary>
        /// Amount of spent calories per one kilo during hour
        /// </summary>
        [Column(Storage = "Calories", DbType = "Float NOT NULL")]
        public float Calories
        {
            get { return calories; }
            set
            {
                calories = value;
                RaisePropertyChanged("Calories");
            }
        }

        #endregion Calories

        #endregion Columns

        #region Duration

        private DateTime? duration;

        public DateTime? Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                RaisePropertyChanged("Duration");
            }
        }

        public float GetTotalHours()
        {
            return Duration == null ? 0 : (Duration.Value.Hour + (float)Duration.Value.Minute / 60);
        }

        #endregion Duration

        #region SpentEnergy

        private int spentEnery = 0;

        public int SpentEnergy
        {
            get { return spentEnery; }
            set
            {
                spentEnery = value;
                RaisePropertyChanged("SpentEnergy");
            }
        }

        #endregion SpentEnergy

        public PhysicalActivity CreateCopy()
        {
            return new PhysicalActivity
            {
                StringId = this.StringId,
                Id = this.Id,
                Calories = this.Calories,
                Duration = this.Duration,
                SpentEnergy = this.SpentEnergy
            };
        }
    }
}
