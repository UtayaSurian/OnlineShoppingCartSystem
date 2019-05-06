using OSCS.Core.Contracts;
using OSCS.Core.Models;
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
        IOrderService orderService;
        //To inject the basket service
        public BasketController(IBasketService BasketService, IOrderService OrderService)
        {
            this.basketService = BasketService;
            this.orderService = OrderService;

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

        public ActionResult Checkout()
        {
            return View();  //Return to ask the user to checkout in a new page
        }
        [HttpPost]
        public ActionResult Checkout(Order order)   //For process the order
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created";

            //process payment
            order.OrderStatus = "Payment Processed";
            orderService.CreateOrder(order, basketItems);
            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou", new { OrderId = order.Id });
        }

        //Direct user to thank you page
        public ActionResult ThankYou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }
    }
}