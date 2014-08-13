using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Models
{
    [DataContract]
    public class ActivityType
    {
        public static readonly ActivityType Sitting = new ActivityType(1.1, "Sitting");
        public static readonly ActivityType Light = new ActivityType(1.2, "Light");
        public static readonly ActivityType Medium = new ActivityType(1.4, "Medium");
        public static readonly ActivityType High = new ActivityType(1.5, "High");
        public static readonly ActivityType Extreme = new ActivityType(1.8, "Extreme");
        
        private double value;
        
        private string key;

        public ActivityType(double value, string key)
        {
            this.value = value;
            this.key = key;
        }

        [DataMember]
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        [DataMember]
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
