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
            return DbContext.PhysicalAcivities.ToList();
        }
    }
}
