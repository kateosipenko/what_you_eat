using Core.Helpers;
using DataAccess.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class BodyStateRepository : Repository
    {
        public List<BodyState> GetAllStates()
        {
            return DbContext.BodyStates.ToList();
        }

        public BodyState GetLastState()
        {
            return DbContext.BodyStates.OrderByDescending(item => item.Date).FirstOrDefault();
        }

        public BodyState Add(BodyState state)
        {
            var newState = BodyState.CreateCopy(state);
            newState.Date = DateTime.Now;
            try
            {
                DbContext.BodyStates.InsertOnSubmit(newState);
                DbContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                newState = null;
                ErrorLogger.LogException(ex);
            }

            return newState;
        }
    }
}
