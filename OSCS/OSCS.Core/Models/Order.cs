using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCS.Core.Models
{
    //The actual order
    public class Order :BaseEntity
    {
        public Order()
        {
            this.OrderItems = new List<OrderItem>();
        }

        //Keep a copy of our customer details
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string OrderStatus { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } //To tell the entity framework when get the items
        
    }
}
