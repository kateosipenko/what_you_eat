using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Models
{
    [DataContract]
    public class UserReminder : RaisableObject
    {
        #region DaysOfWeek

        private List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();

        [DataMember]
        public List<DayOfWeek> DaysOfWeek
        {
            get { return daysOfWeek; }
            set
            {
                daysOfWeek = value;
                RaisePropertyChanged("DaysOfWeek");
            }
        }

        #endregion DaysOfWeek

        #region Type

        private EnergyType type = EnergyType.None;

        [DataMember]
        public EnergyType Type
        {
            get { return type; }
            set
            {
                type = value;
                RaisePropertyChanged("Type");
            }
        }

        #endregion Type

        #region Time

        private DateTime time;
        [DataMember]
        public DateTime Time
        {
            get { return time; }
            set
            {
                time = value;
                RaisePropertyChanged("Time");
            }
        }

        #endregion Time

        #region Title

        private string title;
        [DataMember]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        #endregion Title

        #region Content

        private string content;
        [DataMember]
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                RaisePropertyChanged("Content");
            }
        }

        #endregion Content

        /// <summary>
        /// There could be several days in reminder. Reminder interval does not support specific days.
        /// </summary>
        /// <param name="indexOfDay">Index of selected day from daysOfWeek list.</param>
        /// <returns>Name of reminder.</returns>
        public string GetReminderName(int indexOfDay)
        {
            string result = String.Concat(Enum.GetName(typeof(EnergyType), type), time.Hour, time.Minute, title, content);
            if (indexOfDay >= 0 && indexOfDay < daysOfWeek.Count)
            {
                result += Enum.GetName(typeof(DayOfWeek), daysOfWeek[indexOfDay]);
            }

            return result;
        }
    }
}
