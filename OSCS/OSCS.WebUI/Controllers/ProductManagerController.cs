using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OSCS.Core.Models; 
using OSCS.DataAccess.InMemory;


namespace OSCS.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        //Instances
        ProductRepository context;

        //Enable the repository
        public ProductManagerController()
        {
            context = new ProductRepository();
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            //Return a list of products 
            List<Product> products = context.Collection().ToList(); //Get it from the collection and convert into list
            return View(products); //send it back to the view
        }   

        //Just Display the product
        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);    //Insert products into collection
                context.Commit();   //Save the changes by commiting

                return RedirectToAction("Index");
            }
        }

        //Editing
        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();  //If product not found
            }
            else
            {
                return View(product);
            }
        }

        //View Edit template will update the result by sending into product database
        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToEdit = context.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();  //If product not found
            }
            else
            {
                if (!ModelState.IsValid) {
                    return View(product);
                }
                //Edit the products 
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                context.Commit();
                return RedirectToAction("Index");
            }

        }

        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();  //If product not found
            }
            else
            {
                return View(productToDelete);
            }

        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product  productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();  //If product not found
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }

}