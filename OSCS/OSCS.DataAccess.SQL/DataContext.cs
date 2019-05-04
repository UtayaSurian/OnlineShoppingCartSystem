using OSCS.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCS.DataAccess.SQL
{
    public class DataContext : DbContext //Base methods that can help us Entity Frameworks
    {
        public DataContext()
            : base("DefaultConnection")
        {

        }

        //Creating model for SQL to create table
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
