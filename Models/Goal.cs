using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Models
{
    public enum Course
    {
        PutOnWeight,
        LoseWeight,
        KeepWeight
    }

    [DataContract]
    public class Goal : RaisableObject
    {
        #region Fields

        private Course course;
        private DateTime desiredDate;
        private int desiredWeeksCount;
        private float desiredWeightDif;
        private float desiredWeightDigPerWeek;

        #endregion Fields

        #region Properties

        [DataMember]
        public Course Course
        {
            get { return course; }
            set
            {
                course = value;
                RaisePropertyChanged("Course");
            }
        }

        [DataMember]
        public DateTime DesiredDate
        {
            get { return desiredDate; }
            set
            {
                desiredDate = value;
                RaisePropertyChanged("DesiredDate");
            }
        }

        [DataMember]
        public int DesiredWeeksCount 
        {
            get { return desiredWeeksCount; }
            set
            {
                desiredWeeksCount = value;
                RaisePropertyChanged("DesiredWeeksCount");
            }
        }

        [DataMember]
        public float DesiredWeightDif
        {
            get { return desiredWeightDif; }
            set
            {
                desiredWeightDif = value;
                RaisePropertyChanged("DesiredWeightDif");
            }
        }

        [DataMember]
        public float DesiredWeightDigPerWeek
        {
            get { return desiredWeightDigPerWeek; }
            set
            {
                desiredWeightDigPerWeek = value;
                RaisePropertyChanged("DesiredWeightDigPerWeek");
            }
        }

        #endregion Properties
    }
}
