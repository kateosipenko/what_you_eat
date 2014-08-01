using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace DataAccess.Tables
{
    [Table]
    public class Food : RaisableObject
    {
        #region Fields

        private int id;
        private int calories;
        private float protein;
        private float fats;
        private float carbohydrates;
        private string stringId;

        #endregion Fields

        #region Columns

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

        [Column(Storage = "Calories", DbType = "Int NOT NULL")]
        public int Calories
        {
            get { return calories; }
            set
            {
                calories = value;
                RaisePropertyChanged("Calories");
            }
        }

        [Column(Storage = "Protein", DbType = "Float")]
        public float Protein
        {
            get { return protein; }
            set
            {
                protein = value;
                RaisePropertyChanged("Protein");
            }
        }

        [Column(Storage = "Fats", DbType = "Float")]
        public float Fats
        {
            get { return fats; }
            set
            {
                fats = value;
                RaisePropertyChanged("Fats");
            }
        }

        [Column(Storage = "Carbohydrates", DbType = "Float")]
        public float Carbohydrates
        {
            get { return carbohydrates; }
            set
            {
                carbohydrates = value;
                RaisePropertyChanged("Carbohydrates");
            }
        }

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

        #endregion Columns
    }
}
