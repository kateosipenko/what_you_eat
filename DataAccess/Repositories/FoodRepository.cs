using DataAccess.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class FoodRepository : Repository
    {
        public List<Food> GetAllFoods()
        {
            return DbContext.Foods.ToList();
        }
    }
}
