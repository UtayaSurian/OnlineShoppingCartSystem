using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCS.Core.Contracts;
using OSCS.Core.Models;
using OSCS.Core.ViewModels;
using OSCS.Services;
using OSCS.WebUI.Controllers;
using OSCS.WebUI.Tests.Mocks;

namespace OSCS.WebUI.Tests.Controllers
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void CanAddBasketItem()
        {   //Mock COntext
            //setup
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();
            var httpContext = new MockHttpContext();


            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);
            var controller = new BasketController(basketService, orderService,customers);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
            
            //Act
            //basketService.AddToBasket(httpContext, "1");

            controller.AddToBasket("1");

            Basket basket = baskets.Collection().FirstOrDefault();
            //Assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }
        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            //setup
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();

            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1 });
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);
            var controller = new BasketController(basketService, orderService,customers);

    
            var httpContext = new MockHttpContext();

            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("wappBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel) result.ViewData.Model;

            Assert.AreEqual(3, basketSummary.BasketCount);
            Assert.AreEqual(25.00m, basketSummary.BasketTotal);
        }
        [TestMethod]
        public void CanCheckoutAndCreateOrder()
        {
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Customer> customers = new MockContext<Customer>();
            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 11.00m });

            IRepository<Basket> baskets = new MockContext<Basket>();
            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId="1", Quantity=2,BasketId=basket.Id});
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 1, BasketId = basket.Id });

            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);
            IRepository<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders);

            //Fake user email
            customers.Insert(new Customer() { Id="1", Email = "utayasurian97@gmail.com",ZipCode="57000" });

            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("utayasurian97@gmail.com", "Forms"), null);


            var controller = new BasketController(basketService, orderService,customers);
            var HttpContext = new MockHttpContext();
            HttpContext.User = FakeUser;

            HttpContext.Request.Cookies.Add(new System.Web.HttpCookie("wappBasket") {
                Value = basket.Id

            });
            
            controller.ControllerContext = new ControllerContext(HttpContext, new System.Web.Routing.RouteData(),controller);

            //Act
            Order order = new Order();
            controller.Checkout(order);

            //Assert
            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, basket.BasketItems.Count);

            Order orderInRep = orders.Find(order.Id);
            Assert.AreEqual(2, orderInRep.OrderItems.Count);//To check whether the item in repo or not
        }

    }
}
