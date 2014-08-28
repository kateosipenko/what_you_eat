using Core.Helpers;
using DataAccess.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class TrainingRepository : Repository
    {
        public List<Training> GetAllTrainings()
        {
            return (from training in DbContext.Trainings
                    select training).ToList();
        }

        public Training GetById(int id)
        {
            return (from training in DbContext.Trainings
                    where training.Id == id
                    select training).SingleOrDefault();
        }

        public bool Delete(Training training)
        {
            bool result = false;
            try
            {
                var existing = GetById(training.Id);
                if (existing != null)
                {
                    DbContext.Trainings.DeleteOnSubmit(existing);
                    DbContext.SubmitChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogException(ex);
            }

            return result;
        }

        public Training Add(Training training)
        {
            Training newTraining = training.CreateCopy();
            try
            {
                DbContext.Trainings.InsertOnSubmit(newTraining);
                DbContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogException(ex);
            }

            return newTraining;
        }
    }
}
