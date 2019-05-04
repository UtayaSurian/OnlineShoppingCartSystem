using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using OSCS.Core.Models;
namespace OSCS.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {

        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories; //Creating List variable called products to store product info
        
        //Contructor
        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>; //To look into the cache to see products
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>(); //If the products not found in cache then create a list
            }
        }

        //Save poducts into cache
        public void Commit()
        {
            cache["productCategories"] = productCategories;

        }

        public void Insert(ProductCategory p)
        {
            productCategories.Add(p);
        }

        public void Update(ProductCategory productCategory)
        {
            ProductCategory productCategoryToUpdate = productCategories.Find(p => p.Id == productCategory.Id);

            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate = productCategory;
            }
            else
            {
                throw new Exception("Product Category not found!");
            }
        }

        public ProductCategory Find(string Id)
        {

            ProductCategory productCategory = productCategories.Find(p => p.Id == Id);

            if (productCategory != null)
            {
                return productCategory;
            }
            else
            {
                throw new Exception("Product Category not found!");
            }
        }

        //return the list to make query
        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        //Delete query
        public void Delete(string Id)
        {
            ProductCategory productCategoryToDelete = productCategories.Find(p => p.Id == Id);

            if (productCategoryToDelete != null)
            {
                productCategories.Remove(productCategoryToDelete);
            }
            else
            {
                throw new Exception("Product not found!");
            }
        }
    }
}

