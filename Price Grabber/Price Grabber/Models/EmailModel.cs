using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Price_Grabber.Helpers;

namespace Price_Grabber.Models
{
    public class EmailModel
    {
        [Required]
        [Display(Name = "From")]
        [DataType(DataType.EmailAddress)]
        public string From { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "To")]
        [DataType(DataType.EmailAddress)]
        public string To { get; set; }



        [Required]
        [Display(Name = "Subject")]
        [DataType(DataType.MultilineText)]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

       

     

    }

}