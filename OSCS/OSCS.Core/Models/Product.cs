using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCS.Core.Models
{
    public class Product
    {
        public string Id { get; set; }

        [StringLength(20)]  //Validations for Name in max lenght 
        [DisplayName("Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0,1000)] //Maxing amount
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }

        //Contructor method to help in generating ID autmatically
        public Product()
        {
            this.Id = Guid.NewGuid().ToString(); //auto generate id using guid
        }


    }
}
