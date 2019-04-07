using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GlobalProductCorp.Models
{
    public class ProductOrder : Product
    {        
        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, Int32.MaxValue, ErrorMessage = "El valor {0} no es un valor válido")]
        public int Quantity { get; set; }

        [Display(Name = "Value IVA")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal IVAValue { get { return IVA * (decimal)Quantity; } }
        
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Value { get { return Price * (decimal)Quantity; } }
    }
}