using OSCS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCS.Core.ViewModels
{
    public class ProductManagerViewModel
    {
        //Object
        public Product Product {get;set;}
        //List to interate through of product categories
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
}
