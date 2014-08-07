using DataAccess.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class PhysicalActivityRepository : Repository
    {
        public List<PhysicalActivity> GetAllActivities()
        {
            return (from activity in DbContext.PhysicalAcivities
                    select activity).ToList(); ;
        }

        public PhysicalActivity GetById(int id)
        {
            return (from activity in DbContext.PhysicalAcivities
                    where activity.Id == id
                    select activity).FirstOrDefault();
        }

        public List<PhysicalActivity> Search(IEnumerable<string> keys)
        {
            return (from activity in DbContext.PhysicalAcivities
                    where keys.Contains(activity.StringId)
                    select activity).ToList();
        }

        public List<PhysicalActivity> GetTopTwenty()
        {
            return (from activity in DbContext.PhysicalAcivities
                    select activity).Take(20).ToList();
        }
    }
}
