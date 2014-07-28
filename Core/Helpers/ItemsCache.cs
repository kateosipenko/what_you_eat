using IsolatedStorageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public class ItemsCache<E>
    {
        private string storageKey;

        public DateTime LastUpdatedTime { get; private set; }

        private IEnumerable<E> items;
        public IEnumerable<E> Items
        {
            get
            {
                if (items == null)
                {
                    items = IsolatedStorage.ReadValue<IEnumerable<E>>(storageKey);
                }

                return items;
            }
            set
            {
                IsolatedStorage.WriteValue(storageKey, value);
                LastUpdatedTime = DateTime.UtcNow;
            }
        }

        public ItemsCache(string isolatedStorageKey)
        {
            this.storageKey = isolatedStorageKey;
        }
    }
}
