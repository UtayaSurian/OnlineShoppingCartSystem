using OSCS.Core.Contracts;
using OSCS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace OSCS.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity //Placeholder which inherits from BaseEntity class
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;

        string className;   //For storing object in the cache

        //To Initialize the repository
        public InMemoryRepository()
        {
            className = typeof(T).Name; //To initialze the class wherever the cache is being called inside the class
            items = cache[className] as List<T>;    //Checking for any items in cache
            if(items == null)
            {
                items = new List<T>();  //If cache is empty, then create list for items
            }

        }

        public void Commit()
        {
            cache[className] = items;   //Store items in memory

        }


        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw new Exception(className + "Not Found!");
            }
        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(className + "Not Found!");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {

            T tToDelete = items.Find(i => i.Id == Id);

            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + "Not Found!");
            }
        }
    }

}
