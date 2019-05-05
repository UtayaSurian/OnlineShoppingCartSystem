using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCS.Core.ViewModels
{
    public class BasketItemViewModel
    {   //Refering from Basket Table
        public string Id { get; set; }  //product id
        public int Quantity { get; set; }   //product quantity
        //Referring from product table
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
     
    }
}
