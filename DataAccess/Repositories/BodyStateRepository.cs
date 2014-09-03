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
            return (from states in DbContext.BodyStates
                   select states).ToList();
        }

        public BodyState GetLastState()
        {
            return GetAllStates().OrderByDescending(item => item.Date).FirstOrDefault();
        }

        public BodyState Add(BodyState state)
        {
            var newState = state.CreateCopy();
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

        public List<BodyState> GetListForProgress(int amount)
        {
            List<BodyState> result = new List<BodyState>();

            int count = DbContext.BodyStates.Count();
            if (count <= amount)
            {
                result = GetAllStates();
            }
            else
            {
                int ratio = (int) count / amount + 1;
                var allStates = GetAllStates();
                for (int i = 0; i < count; i += ratio)
                {
                    result.Add(allStates[i]);
                }
            }

            return result;
        }
    }
}
