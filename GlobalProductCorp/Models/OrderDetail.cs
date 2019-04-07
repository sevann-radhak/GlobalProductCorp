using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GlobalProductCorp.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }

        // Descripción o nombre del producto
        [Display(Name = "Description")]
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        public string Description { get; set; }

        // Precio unitario del producto
        [DataType(DataType.Currency)]
        [Display(Name = "Price ($ USD)")]
        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }
        
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public float Quantity { get; set; }
        
        // IVA del producto
        [DataType(DataType.Currency)]
        [Display(Name = "IVA")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal IVA { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}