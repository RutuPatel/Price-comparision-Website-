using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Price_Grabber.Helpers;

namespace Price_Grabber.Models
{
    public class SearchModel
    {
        public AmazonSearchResult amazonSearchResult { get; set; }
        public EbaySearchResult ebaySearchResult { get; set; }
      
        public string SearchQuery { get; set; }

        //public RegisterViewModel Users { get; set; }
        //public SearchModel()
        //{
        //    Users = new RegisterViewModel();
        //}
    }
 


}