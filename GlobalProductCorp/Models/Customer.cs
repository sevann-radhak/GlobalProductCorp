using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GlobalProductCorp.Models
{
    public class Customer
    {
        [Key]
        [Display(Name = "Customer")]
        public int CustomerID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(60, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(60, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        public string LastName { get; set; }

        [Display(Name = "Document")]
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 5)]
        public string Document { get; set; }

        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [NotMapped]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        [JsonIgnore]
        public virtual ICollection<Order> Order { get; set; }
    }
}