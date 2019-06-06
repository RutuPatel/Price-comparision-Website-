using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Price_Grabber.Helpers;

namespace Price_Grabber.Models
{
    public class SearchKeywordModel
    {
       public string keyword { get; set;}
    }
}