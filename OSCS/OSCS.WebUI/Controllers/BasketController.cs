using OSCS.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OSCS.WebUI.Controllers
{
    public class BasketController : Controller
    {
        //Add the implement service in order to create its methods under controllers
        IBasketService basketService;

        //To inject the basket service
        public BasketController(IBasketService BasketService)
        {
            this.basketService = BasketService;
        }
        // GET: Basket
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            basketService.AddToBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            basketService.RemoveFromBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        //Summary to get the basket summary result
        //Refer BasketService under contract in DataAccess.SQL
        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);

            return PartialView(basketSummary);
        }
    }
}