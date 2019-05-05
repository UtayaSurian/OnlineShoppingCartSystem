using OSCS.Core.Contracts;
using OSCS.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCS.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity   //Use Generic
    {
        internal DataContext context;
        internal DbSet<T> dbSet;        //To access the nderline table for data context

        //Passing in DataCOntext
        //Inject the datacontext model
        public SQLRepository(DataContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();         //set the underline table based on using model <T>
        }

        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var t = Find(Id);
            if (context.Entry(t).State == EntityState.Detached)
            {
                dbSet.Attach(t);

                dbSet.Remove(t);
            }
        }

        public T Find(string Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void Update(T t)
        {
            dbSet.Attach(t);    //Attach the passing object called t to the entity framework table
            context.Entry(t).State = EntityState.Modified;  //To look for the save changes 
        }
    }
}
