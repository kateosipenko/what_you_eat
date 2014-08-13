using DataAccess.Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DataAccess.Tables
{
    [DataContract]
    public class User : RaisableObject
    {
        #region Sex

        private Sex sex;

        [DataMember]
        public Sex Sex
        {
            get { return sex; }
            set
            {
                sex = value;
                RaisePropertyChanged("Sex");
            }
        }

        #endregion Sex

        #region Birthday

        private DateTime? birthday;

        [DataMember]
        public DateTime? Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                RaisePropertyChanged("Birthday");
            }
        }

        #endregion Birthday

        #region BodyState

        private BodyState bodyState = new BodyState();

        public BodyState BodyState
        {
            get { return bodyState; }
            set
            {
                bodyState = value;
                RaisePropertyChanged("BodyState");
            }
        }

        #endregion BodyState

        #region ActivityType

        private ActivityType activityType;

        [DataMember]
        public ActivityType ActivityType
        {
            get { return activityType; }
            set
            {
                activityType = value;
                RaisePropertyChanged("ActivityType");
            }
        }

        public int Age
        {
            get
            {
                return Birthday != null ? DateTime.Now.Year - Birthday.Value.Year : 0;
            }
        }

        #endregion ActivityType

        [OnDeserialized]
        public void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            using (var stateRepo = new BodyStateRepository())
            {
                bodyState = stateRepo.GetLastState();
            }
        }
    }
}
