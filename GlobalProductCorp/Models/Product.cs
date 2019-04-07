using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlobalProductCorp.Models
{
    public class Product
    {
        // Identificador o código del producto
        [Key]
        public int ProductID { get; set; }

        // Descripción o nombre del producto
        [Display(Name = "Description")]
        [Required (ErrorMessage = "The field {0} is required" )]
        [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        public string Description { get; set; }

        // Precio unitario del producto
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, 100000, ErrorMessage = "This is a not valid value for {0}")]
        public decimal Price { get; set; }

        // Unidades disponibles del producto
        [Display(Name = "Stock")]
        [Required(ErrorMessage = "The field {0} is required")]
        [Range(0, Int32.MaxValue, ErrorMessage = "This is a not valid value for {0}")]
        public int Stock { get; set; }

        // IVA del producto
        [DataType(DataType.Currency)]
        [Display(Name = "IVA")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [JsonIgnore]
        public virtual decimal IVA { get { return (Price * 19 / 100); } }

        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}