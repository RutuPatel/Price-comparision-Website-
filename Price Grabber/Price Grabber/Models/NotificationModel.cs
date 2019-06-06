using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Price_Grabber.Helpers;

namespace Price_Grabber.Models
{
  
    public class NotificationModel
    {
      //  public ProductModel Product { get; set; }

        //public NotificationModel()
        //{
        //    Product = new ProductModel();          
        //}

        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The Price must be in two digits or more", MinimumLength = 2)]
        [Phone]
        [DataType(DataType.Currency)]
        public string Price { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
      
       
      
      
    }
}