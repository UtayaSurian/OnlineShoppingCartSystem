using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCS.Core.Models
{
    public class BasketItem : BaseEntity
    {
        //To link to basket that contains items by using id
        //For server references
        public string BasketId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
