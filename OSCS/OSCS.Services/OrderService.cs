using OSCS.Core.Contracts;
using OSCS.Core.Models;
using OSCS.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
                                                        //Create order\\

namespace OSCS.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> orderContext;
        public OrderService(IRepository<Order> OrderContext)
        {
            this.orderContext = OrderContext;
        }

        void IOrderService.CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems)
        {
            //Retreive added items from basket 
            foreach(var item in basketItems)
            {
                baseOrder.OrderItems.Add(new OrderItem() {
                    ProductId = item.Id,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity=item.Quantity
                });

            }

            orderContext.Insert(baseOrder);
            orderContext.Commit();
        }

                                                        //For Admin to view\\
        public List<Order> GetOrdersList()
        {
            return orderContext.Collection().ToList();
        }

        public Order GetOrder(string Id)
        {
            return orderContext.Find(Id);
        }

        public void UpdateOrder(Order updatedOrder)
        {
            orderContext.Update(updatedOrder);
            orderContext.Commit();
        }
    }
}
