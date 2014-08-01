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
        #region Fields

        private int id;
        private int calories;
        private string stringId;

        #endregion Fields

        #region Columns

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

        [Column(Storage = "StringId", DbType = "NVarChar(100) NOT NULL")]
        public string StringId
        {
            get { return stringId; }
            set
            {
                stringId = value;
                RaisePropertyChanged("StringId");
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

        #endregion Columns

    }
}
