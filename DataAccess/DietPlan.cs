using DataAccess.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Models
{
    [DataContract]
    public class DietPlan : RaisableObject
    {
        #region FoodPlan

        private FoodPlan foodPerDay = new FoodPlan();

        public FoodPlan FoodPerDay
        {
            get { return foodPerDay; }
            set
            {
                foodPerDay = value;
                RaisePropertyChanged("FoodPerDay");
            }
        }

        #endregion FoodPlan

        #region TrainigsPlan

        private List<Training> trainings = new List<Training>();

        [DataMember]
        public List<Training> Trainigs
        {
            get { return trainings; }
            set
            {
                trainings = value;
                RaisePropertyChanged("Trainigs");
            }
        }

        #endregion TrainigsPlan

        #region ThrowOffPerWeek

        private int throwOffPerWeek;

        /// <summary>
        /// Amount of gramms that user has to throw off per week for losing weight by the plan
        /// </summary>
        [DataMember]
        public int ThrowOffPerWeek
        {
            get { return throwOffPerWeek; }
            set
            {
                throwOffPerWeek = value;
                RaisePropertyChanged("ThrowOffPerWeek");
            }
        }

        #endregion ThrowOffPerWeek

        #region PutOnPerWeek

        private int putOnPerWeek;

        /// <summary>
        /// Amount of gramms that user has to eat per week for put on weight by the plan
        /// </summary>
        [DataMember]
        public int PutOnPerWeek
        {
            get { return putOnPerWeek; }
            set
            {
                putOnPerWeek = value;
                RaisePropertyChanged("PutOnPerWeek");
            }
        }

        #endregion PutOnPerWeek

        #region WaterPlan

        private WaterPlan waterPlan = new WaterPlan();

        [DataMember]
        public WaterPlan WaterPlan
        {
            get { return waterPlan; }
            set
            {
                waterPlan = value;
                RaisePropertyChanged("WaterPlan");
            }
        }

        #endregion WaterPlan

        #region ProcentForFood

        private int procentForFood = 0;

        [DataMember]
        public int ProcentForFood
        {
            get { return procentForFood; }
            set
            {
                procentForFood = value;
                RaisePropertyChanged("ProcentForFood");
            }
        }

        #endregion ProcentForFood

        #region ProcentForTrainings

        private int procentForTrainings = 0;

        [DataMember]
        public int ProcentForTrainings
        {
            get { return procentForTrainings; }
            set
            {
                procentForTrainings = value;
                RaisePropertyChanged("ProcentForTrainings");
            }
        }

        #endregion ProcentForTrainings

        #region MustSpentPerWeek

        private int mustSpentPerWeek;

        [DataMember]
        public int MustSpentPerWeek
        {
            get { return mustSpentPerWeek; }
            set
            {
                mustSpentPerWeek = value;
                RaisePropertyChanged("MustSpentPerWeek");
            }
        }

        #endregion MustSpentPerWeek

        public void Clear()
        {
            FoodPerDay = new FoodPlan();
            if (Trainigs != null)
                Trainigs.Clear();
            else
                Trainigs = new List<Training>();

            ThrowOffPerWeek = 0;
            PutOnPerWeek = 0;
            WaterPlan = new WaterPlan();
            ProcentForFood = 0;
            ProcentForTrainings = 0;
            MustSpentPerWeek = 0;
        }
    }
}
