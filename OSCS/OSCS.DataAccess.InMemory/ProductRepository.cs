using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching; //Import Cacche library for in memory
using OSCS.Core.Models; //Import Model library for product

namespace OSCS.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products; //Calling List to store product info

        //Contructor
        public ProductRepository()
        {
            products = cache["products"] as List<Product>; //To look into the cache to see products
            if (products == null)
            {
                products = new List<Product>(); //If the products not found in cache then create a list
            }
        }

        //Save poducts into cache
        public void Commit()
        {
            cache["products"] = products;

        }

        public void Insert(Product p)
        {
            products.Add(p);
        }

        public void Update(Product product)
        {
            Product productToUpdate = products.Find(p => p.Id == product.Id);

            if (productToUpdate!=null){
                productToUpdate = product;
            }
            else
            {
                throw new Exception("Product not found!");
            }
        }

        public Product Find (string Id)
        {

            Product product = products.Find(p => p.Id == Id);

            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product not found!");
            }
        }

        //return the list to make query
        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        //Delete query
        public void Delete (string Id)
        {
            Product productToDelete = products.Find(p => p.Id == Id);

            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product not found!");
            }
        }
    }
}
