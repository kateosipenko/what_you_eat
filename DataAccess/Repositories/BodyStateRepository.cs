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
    }
}
