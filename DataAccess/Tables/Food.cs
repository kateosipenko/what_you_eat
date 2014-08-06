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
    public partial class Food : RaisableObject
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

        #region Calories

        private int calories;

        /// <summary>
        /// Calories for 100gramm/one portion of product
        /// </summary>
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

        #endregion Calories

        #region Proteins

        private float protein;

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

        #endregion Proteins

        #region Fats

        private float fats;

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

        #endregion Fats

        #region Carbohydrates

        private float carbohydrates;

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

        #endregion Carbohydrates

        #region StringId

        private string stringId;

        /// <summary>
        /// Key of translation
        /// </summary>
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

        #region EatenTimes

        private int eatenTimes = 0;

        /// <summary>
        /// How many times user has eaten food
        /// </summary>
        [Column(Storage = "EatenTimes")]
        public int EatenTimes
        {
            get { return eatenTimes; }
            set
            {
                eatenTimes = value;
                RaisePropertyChanged("EatenTimes");
            }
        }

        #endregion EatenTimes        

        #region IsFavorite

        private bool isFavorite = false;

        [Column(Storage = "IsFavorite", DbType = "Bit NOT NULL DEFAULT (0)")]
        public bool IsFavorite
        {
            get { return isFavorite; }
            set
            {
                isFavorite = value;
                RaisePropertyChanged("IsFavorite");
            }
        }

        #endregion IsFavorite

        #endregion Columns

        #region AmountOfEaten

        private int amountOfEaten = 0;

        /// <summary>
        /// Amount of food that user has eaten. For example, 300 gramm or 2 glasses
        /// Field is specified for food that was eaten today.
        /// </summary>
        public int AmountOfEaten
        {
            get { return amountOfEaten; }
            set
            {
                amountOfEaten = value;
                RaisePropertyChanged("AmountOfEaten");
            }
        }

        #endregion AmountOfEaten

        #region AmountOfCalories

        private int amountOfCalories = 0;

        /// <summary>
        /// Callories that were eaten by user.
        /// </summary>
        public int AmountOfCalories
        {
            get { return amountOfCalories; }
            set
            {
                amountOfCalories = value;
                RaisePropertyChanged("AmountOfCalories");
            }
        }

        #endregion AmountOfCalories

        #region FoodMeasure

        private FoodMeasure foodMeasure;

        /// <summary>
        /// Measure of this food: gramm, portion or glass that were used for calculation
        /// </summary>
        public FoodMeasure FoodMeasure
        {
            get { return foodMeasure; }
            set
            {
                foodMeasure = value;
                RaisePropertyChanged("FoodMeasure");
            }
        }

        #endregion FoodMeasure

        public Food CreateCopy()
        {
            Food result = new Food
            {
                Id = this.Id,
                IsFavorite = this.IsFavorite,
                Protein = this.Protein,
                StringId = this.StringId,
                Fats = this.Fats,
                EatenTimes = this.EatenTimes,
                Carbohydrates = this.Carbohydrates,
                Calories = this.Calories,
                AmountOfEaten = this.AmountOfEaten,
                FoodMeasure = this.FoodMeasure
            };

            return result;
        }
    }
}
