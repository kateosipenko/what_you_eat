using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Models
{
    [DataContract]
    public class WaterPlan : RaisableObject
    {
        #region Amount

        private int amount;

        /// <summary>
        /// Amount of water per day in milliliters
        /// </summary>
        [DataMember]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                RaisePropertyChanged("Amount");
            }
        }

        #endregion Amount

        #region IntakeCount

        private int intakeCount = 0;

        [DataMember]
        public int IntakeCount
        {
            get { return intakeCount; }
            set
            {
                intakeCount = value;
                RaisePropertyChanged("IntakeCount");
            }
        }

        #endregion IntakeCount
    }
}
