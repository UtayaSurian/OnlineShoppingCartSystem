using OSCS.Core.Contracts;
using OSCS.Core.Models;
using OSCS.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OSCS.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //Instances
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories; //Load from database

        //Enable the repository which implements Irepository
        //Refer it from IRepository interface class in OSCS.Core
        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoriesContext)
        {
            //initiliaze
            context = productContext;
            productCategories = productCategoriesContext;
        }
        public ActionResult Index(string Category=null)
        {
            List<Product> products; //Get the products and store into list //Create an empty list of products
            List<ProductCategory> categories = productCategories.Collection().ToList();
            
            if (Category == null)
            {
               products = context.Collection().ToList();  //If the category is empty the show the entire list of products
            }
            else
            {
                //Filteration here
                products = context.Collection().Where(p => p.Category == Category).ToList();
            }
            //Instantiate object
            ProductListViewModel model = new ProductListViewModel();
            model.Products = products;
            model.ProductCategories = categories;

            return View(model);
        }

        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}