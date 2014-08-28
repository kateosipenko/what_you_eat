using Core.Helpers;
using DataAccess.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class ExersizesRepository : Repository
    {
        public List<Exersize> GetAll()
        {
            return (from Exersizes in DbContext.Exersizes
                   select Exersizes).ToList();
        }

        public List<Exersize> GetByTraining(int id)
        {
            return (from exersize in DbContext.Exersizes
                    where exersize.TrainingId == id
                    select exersize).ToList();
        }

        public List<Exersize> GetForToday()
        {
            return (from exersize in DbContext.Exersizes
                    where exersize.Date != null && exersize.Date.Value.Date == DateTime.Now.Date
                    select exersize).ToList();

        }

        public void DeleteOldExersizes()
        {
            var oldExersizes = from exersize in DbContext.Exersizes
                               where exersize.Date != null && exersize.Date.Value != null && exersize.Date.Value.Date != DateTime.Now.Date
                               select exersize;
            foreach (var exersize in oldExersizes)
            {
                Delete(exersize);
            }
        }

        public Exersize GetById(int id)
        {
            return (from exersize in DbContext.Exersizes
                    where exersize.Id == id
                    select exersize).SingleOrDefault();
        }

        public bool Delete(Exersize exersize)
        {
            bool result = false;
            try
            {
                var existing = GetById(exersize.Id);
                if (existing != null)
                {
                    DbContext.Exersizes.DeleteOnSubmit(existing);
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

        public Exersize Add(Exersize exersize)
        {
            Exersize newExersize = exersize.CreateCopy();
            if (newExersize.Date == null)
                newExersize.Date = DateTime.Now;

            try
            {
                DbContext.Exersizes.InsertOnSubmit(newExersize);
                DbContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogException(ex);
            }

            return newExersize;
        }
    }
}
