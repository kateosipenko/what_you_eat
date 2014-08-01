using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class Repository : IDisposable
    {
        private FoodContext dbContext;

        public Repository()
        {
            try
            {
                dbContext = new FoodContext();
            }
            catch (Exception ex)
            {
                // TODO: implement exception logging
            }
        }

        protected FoodContext DbContext
        {
            get { return dbContext; }
            set { dbContext = value; }
        }


        public void Dispose()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
