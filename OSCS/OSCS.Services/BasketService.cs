using OSCS.Core.Contracts;
using OSCS.Core.Models;
using OSCS.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OSCS.Services
{
    //Read cookies from user
    //Creating,adding and removing from baskets
    public class BasketService : IBasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;      //Load the basket and basket items

        //Use this string to identify for any particular cookie
        //No one can update this string since its in a const :)) Additional security feature
        public const string BasketSessionName = "wappBasket"; //cookie name 

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
        {
            this.basketContext = BasketContext;
            this.productContext = ProductContext;
        }

        //Read the basket via httpcontextbase using cookie
        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull) //Null if the basket not exist then it will create a new one
        {
            //Read cookie
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName); //Get into user's session and look for their cookies

            Basket basket = new Basket();
            //If cookies exist, httpCookie will read the cookies
            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                { //Further check if null or not null //To avoid security issues :))
                    basket = basketContext.Find(basketId);
                }
                //If not then create a new basket
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {

                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            //return the actual basket after checking was done through httpcontext
            return basket;

        }
        //Using this way to create a basket 
        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            basketContext.Insert(basket);   //Store into the database
            basketContext.Commit();     //Save changes //Refer to Irepository

            //Write cookie to user's machine
            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);   //Set expiration for the cookie //For security reasons
            //Send it back to the user
            httpContext.Response.Cookies.Add(cookie);

            return basket; //Return the basket once created successfully
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);    //load from database

            //If any item exist in the basket
            if (item == null)
            {
                //Adding 
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item);
            }
            //if exist just add the quantities
            else
            {
                item.Quantity = item.Quantity + 1;
                //Not required to update here since the entity framework maintain the cache
            }

            basketContext.Commit(); //Save changes
        }
        //Deleting from basket based on item id
        public void RemoveFromBasket(HttpContextBase httpContext, string ItemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i=>i.Id==ItemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            //Get the basket from the database
            Basket basket = GetBasket(httpContext, false);

            if (basket != null)
            {
                //Query the product table and basket item table and select things needed to show: Inner Join using LINQ
                var results = (from b in basket.BasketItems
                              join p in productContext.Collection() on b.ProductId equals p.Id
                              select new BasketItemViewModel()
                              {
                                  Id = b.Id,
                                  Quantity = b.Quantity,
                                  ProductName = p.Name,
                                  Image = p.Image,
                                  Price = p.Price
                              }
                              ).ToList();
                return results;
            }
            else
            {
                //If nothng in basket, then return a new empty list of the basket items
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);  //If empty then not required to create basket
            BasketSummaryViewModel model = new BasketSummaryViewModel(0,0); //Set by default by 0 items n total in basket 

            //If any items currently exist then sum the quanutity which newly added by customer and calculate for total price
            if(basket!=null)
            {   //int? is when empty the it will send a null value such as quantity and price
                int? basketCount = (from item in basket.BasketItems     //querying the basket items table
                                    select item.Quantity).Sum();
                decimal? basketTotal = (from item in basket.BasketItems
                                        join p in productContext.Collection() on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();

                model.BasketCount = basketCount ?? 0; //if the basket is empty then return with 0 by default
                model.BasketTotal = basketTotal ?? decimal.Zero; //if the basket is empty then return with decimal 0 by default
                return model;
            }
            else
            {
                return model;
            }
        }

        //TO clear the basket once service has been done
        public void ClearBasket(HttpContextBase httpContext)       //Send the context to get the basket 
        {
            Basket basket = GetBasket(httpContext,false);
            basket.BasketItems.Clear();
            basketContext.Commit();
        }
    }
}
