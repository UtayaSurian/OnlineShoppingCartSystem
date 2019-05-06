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
        //To tell the SQL to store these models into the database
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Customer> Customers { get; set; } 
    }
}
