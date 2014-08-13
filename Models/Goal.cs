using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Models
{
    public enum Course
    {
        KeepWeight,
        PutOnWeight,
        LoseWeight
    }

    [DataContract]
    public class Goal : RaisableObject
    {
        #region Fields

        private Course course;
        private DateTime? desiredDate = null;
        private int desiredWeeksCount;
        private float desiredWeight;
        private int caloriesPerDay;
        private float weightPerWeek;
        private int forFood;
        private int forExersizes;

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
        public int ForFood
        {
            get { return forFood; }
            set
            {
                forFood = value;
                RaisePropertyChanged("ForFood");
            }
        }

        [DataMember]
        public int ForExersizes
        {
            get { return forExersizes; }
            set
            {
                forExersizes = value;
                RaisePropertyChanged("ForExersizes");
            }
        }

        [DataMember]
        public DateTime? DesiredDate
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
        public float DesiredWeight
        {
            get { return desiredWeight; }
            set
            {
                desiredWeight = value;
                RaisePropertyChanged("DesiredWeight");
            }
        }

        #endregion Properties
    }
}
