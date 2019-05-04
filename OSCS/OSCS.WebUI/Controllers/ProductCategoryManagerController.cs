using OSCS.Core.Models;
using OSCS.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OSCS.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {   
        //Instances
        ProductCategoryRepository context;

        //Enable the repository
        public ProductCategoryManagerController()
        {
            context = new ProductCategoryRepository();
        }
        // GET: ProductCategoryManager
        public ActionResult Index()     
        {
            //Return a list of products category    
            List<ProductCategory> productCategories = context.Collection().ToList(); //Get it from the collection and convert into list
            return View(productCategories); //send it back to the view
        }

        //Just Display the product category
        public ActionResult Create()
        {   
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            else
            {
                context.Insert(productCategory);    //Insert products into collection
                context.Commit();   //Save the changes by commiting

                return RedirectToAction("Index");
            }
        }

        //Editing
        public ActionResult Edit(string Id)
        {
            ProductCategory productCategory = context.Find(Id);
            if (productCategory == null)
            {
                return HttpNotFound();  //If product not found
            }
            else
            {
                return View(productCategory);
            }
        }

        //View Edit template will update the result by sending into product database
        [HttpPost]
        public ActionResult Edit(ProductCategory product, string Id)
        {
            ProductCategory productCategoryToEdit = context.Find(Id);
            if (productCategoryToEdit == null)
            {
                return HttpNotFound();  //If product not found
            }
            else
            {   
                if (!ModelState.IsValid)
                {
                    return View(product);

                }
                //Edit the product category
                productCategoryToEdit.Category = product.Category;
              

                context.Commit();
                return RedirectToAction("Index");
            }

        }

        public ActionResult Delete(string Id)
        {   
            ProductCategory productCategoryToDelete = context.Find(Id);
            if (productCategoryToDelete == null)
            {
                return HttpNotFound();  //If product not found
            }
            else
            {
                return View(productCategoryToDelete);
            }

        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory productCategoryToDelete = context.Find(Id);
            if (productCategoryToDelete == null)
            {
                return HttpNotFound();  //If product not found
            }
            else
            {
                context.Delete(Id);
                return RedirectToAction("Index");
            }
        }
    }
}
