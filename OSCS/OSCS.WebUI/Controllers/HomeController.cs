using OSCS.Core.Contracts;
using OSCS.Core.Models;
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
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList(); //Get the products and store into list
            return View(products);
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