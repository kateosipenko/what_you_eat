using Models;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace DataAccess.Tables
{
    [Table]
    public class BodyState : RaisableObject
    {
        #region Fields

        private DateTime date;
        private float height;
        private float weight;
        private float waist;
        private float hips;

        #endregion Fields

        #region Columns

        [Column(Storage = "Date", DbType = "DateTime NOT NULL", IsPrimaryKey = true)]
        public DateTime Date
        {
            get { return date; }
            set 
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }

        [Column(Storage = "Height", DbType = "Float")]
        public float Height
        {
            get { return height; }
            set
            {
                height = value;
                RaisePropertyChanged("Height");
            }
        }

        [Column(Storage = "Weight", DbType = "Float")]
        public float Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                RaisePropertyChanged("Weight");
            }
        }

        [Column(Storage = "Waist", DbType = "Float")]
        public float Waist
        {
            get { return waist; }
            set
            {
                waist = value;
                RaisePropertyChanged("Waist");
            }
        }

        [Column(Storage = "Hips", DbType = "Float")]
        public float Hips
        {
            get { return hips; }
            set
            {
                hips = value;
                RaisePropertyChanged("Hips");
            }
        }

        #endregion Columns

        public static BodyState CreateCopy(BodyState state)
        {
            return new BodyState
            {
                height = state.height,
                hips = state.hips,
                waist = state.waist,
                weight = state.weight
            };
        }
    }
}
