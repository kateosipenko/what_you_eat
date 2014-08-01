using DataAccess.Tables;
using Microsoft.Phone.Data.Linq;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;

namespace DataAccess
{
    public class FoodContext : DataContext
    {
        private const string DbFilePath = "FoodDb.sdf";
        private static bool versionChecked = false;

        public FoodContext()
            : base("DataSource= 'isostore:/FoodDb.sdf';")
        {
            // If you want updated database schema you have to create database on windows phone application,
            // copy it from emulator to desktop using iso tool and than add to project
            // CreateDatabase();

            if (!versionChecked)
            {
                CheckDbVersion();
            }
        }

        /// <summary>
        /// If there is no database file in isolated storage, copy it from xap to isolated storage.
        /// It performs read/write access to db.
        /// Also check database for possible update if version has been changed.
        /// </summary>
        private void CheckDbVersion()
        {
            bool dbExists = false;
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                dbExists = storage.FileExists(DbFilePath);
                if (!dbExists)
                {
                    Stream resourceFile = Application.GetResourceStream(new Uri(DbFilePath, UriKind.Relative)).Stream;
                    IsolatedStorageFileStream isolatedStorageFile = new IsolatedStorageFileStream(DbFilePath, FileMode.CreateNew, storage);
                    resourceFile.CopyTo(isolatedStorageFile);
                    isolatedStorageFile.Flush();
                    resourceFile.Close();
                    isolatedStorageFile.Close();
                }
            }

            if (dbExists)
            {
                DatabaseSchemaUpdater updater = this.CreateDatabaseSchemaUpdater();
                if (updater.DatabaseSchemaVersion < 2)
                {
                }
            }
        }

        #region Tables

        public Table<Food> Foods
        {
            get { return GetTable<Food>(); }
        }

        public Table<PhysicalActivity> PhysicalAcivities
        {
            get { return GetTable<PhysicalActivity>(); }
        }

        public Table<BodyState> BodyStates
        {
            get { return GetTable<BodyState>(); }
        }

        #endregion Tables
    }
}
