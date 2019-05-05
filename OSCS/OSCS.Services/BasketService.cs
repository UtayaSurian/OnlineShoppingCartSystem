using OSCS.Core.Contracts;
using OSCS.Core.Models;
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
    public class BasketService
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
    }
}
