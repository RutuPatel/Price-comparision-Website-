using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Price_Grabber.Helpers;

namespace Price_Grabber.Models
{
    public class ProductModel
    {  
        public string Name { get; set; }

        public string Image { get; set; }

        public string AmazonPrice { get; set; }
        public string EbayPrice { get; set; }

        public string AmazonRedirect { get; set; }
        public string EbayRedirect { get; set; }

        public string AmaozonAvailable { get; set; }
        public string EbayAvailable { get; set; }

        public string AmaozonshippingInfo { get; set; }
        public string EbayshippingInfo { get; set; }
        public string Feature { get; set; }





        public string similiarProduct { get; set; }    
        
        public List<string> SwatchImage { get; set; }
        public List<string> SmallImage { get; set; }
        public List<string> ThumbnailImage { get; set; }
        public List<string> TinyImage { get; set; }        
        public List<string> MediumImage { get; set; }
        public List<string> LargeImage { get; set; }

        public List<string> Features { get; set; }

        public ProductModel()
        {
            SwatchImage = new List<string>();
            SmallImage = new List<string>();
            ThumbnailImage = new List<string>();
            TinyImage = new List<string>();
            MediumImage = new List<string>();
            LargeImage = new List<string>();
            Features = new List<string>();
        } 
    }
}