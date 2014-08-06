using Core.Helpers;
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
            return (from food in DbContext.Foods
                       select food).ToList();
        }

        public List<Food> Search(IEnumerable<string> keys)
        {
            return (from food in DbContext.Foods
                    where keys.Contains(food.StringId)
                    select food).ToList();
        }

        public void EatFood(Food food)
        {
            try
            {
                var saved = (from foods in DbContext.Foods
                             where foods.Id == food.Id
                             select food).FirstOrDefault();
                saved.EatenTimes++;
                DbContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogException(ex);
            }
        }

        public Food GetById(int id)
        {
            return (from food in DbContext.Foods
                       where food.Id == id
                       select food).FirstOrDefault();
        }
    }
}
