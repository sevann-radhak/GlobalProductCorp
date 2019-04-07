using GlobalProductCorp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GlobalProductCorp.ViewModels
{
    public class OrderView
    {
        public Customer Customer { get; set; }
        public ProductOrder Product { get; set; }
        public List<ProductOrder> Products { get; set; }
    }
}