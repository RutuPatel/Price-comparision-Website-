using PriceGrabberConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;



namespace NotificationSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var notifList = new PriceGrabberDataContext().Notifications.ToList();

            foreach (var item in notifList) {
                var ebayProducts = SearchEbay(item.ProductName,false);
                var amazonProducts = SearchAmazon(item.ProductName);

                string body = string.Empty;

                if (ebayProducts != null)
                {
                    var notifiedProducts = ebayProducts.searchResult.ebayProducts.Where(x => x.sellingStatus.currentPrice < (double)item.Price);
                    if(notifiedProducts.Any())
                    {
                       
                        foreach (var notifItem in notifiedProducts)
                        {
                            body = string.Format("{0}{2}{1},{3}<br />{2}{4}", body, "Here is your Notification Links <br/> Enjoy Shopping..", notifItem.title, string.IsNullOrEmpty(body) ? "" : ",",notifItem.galleryURL);
                        }
                    }
                }
               if(amazonProducts != null)
                {
                    var notifiedProducts = amazonProducts.amazonItems.Items.Where(x => x.itemAttributes.attributeListPrice.Amount < (double)item.Price);
                    if (notifiedProducts.Any())
                    {
                         
                        foreach (var notifItem in notifiedProducts)
                        {
                            body =   string.Format("{0}{2}{1},{3}<br />{2}{4}",  body, "Here is your Notification Links <br/> Enjoy Shopping..", notifItem.itemAttributes.ProductTypeName , string.IsNullOrEmpty(body) ? "" : ",", notifItem.DetailPageURL);
                        }
                         
                    }
                }
               if(!string.IsNullOrEmpty(body))
                SendNotification(item.Email, body);

            }

        }
        
        public static void SendNotification(string email, string body)
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress("info.pricegrabber@gmail.com");
            mm.Subject = "Notification Link";
            mm.Body = body;//string.Format("<html><body><h2>Price Grabber</h2><p>For Reset your Account Password click below button</p><br/><a style='font-size: 16px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; background-color: #EB7035; border-top: 12px solid #EB7035; border-bottom: 12px solid #EB7035; border-right: 18px solid #EB7035; border-left: 18px solid #EB7035; display: inline-block;' href='http://localhost:59812/Account/ResetPassword?userhash={0}' target='_blank'>Reset Password</a></body></html>");
            // mm.Body = "@Html.ActionLink("reset""ResetPassword")";
            mm.To.Add(new MailAddress(email));
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mm);

        }

        public static AmazonSearchResult AmazonParsing(XmlDocument xml)
        {
            var amazonSearchResult = new AmazonSearchResult();
            if (xml.ChildNodes.Count > 1)
            {
                foreach (XmlNode node in xml.ChildNodes[1].ChildNodes)
                {
                    foreach (XmlNode items in node)
                    {
                        switch (items.Name)
                        {
                            case "Request":
                                foreach (XmlNode requestitems in items)
                                {
                                    switch (requestitems.Name)
                                    {
                                        case "IsValid":
                                            amazonSearchResult.amazonItems.request.IsValid = Convert.ToBoolean(requestitems.InnerText);
                                            break;

                                        case "ItemSearchRequest":
                                            // var itemsearchRequest = new AmazonItemSearchRequest();
                                            foreach (XmlNode ItemSearchRequestItems in requestitems)
                                            {
                                                switch (ItemSearchRequestItems.Name)
                                                {
                                                    case "Keywords":
                                                        amazonSearchResult.amazonItems.request.itemSearchRequest.Keywords = ItemSearchRequestItems.InnerText;
                                                        break;
                                                    case "SearchIndex":
                                                        amazonSearchResult.amazonItems.request.itemSearchRequest.SearchIndex = ItemSearchRequestItems.InnerText;
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }

                                break;

                            case "TotalResults":
                                amazonSearchResult.amazonItems.TotalResults = items.InnerText;
                                break;

                            case "TotalPages":
                                amazonSearchResult.amazonItems.TotalPages = Convert.ToDouble(items.InnerText);
                                break;

                            case "MoreSearchResultsUrl":
                                amazonSearchResult.amazonItems.MoreSearchResultsUrl = items.InnerText;
                                break;

                            case "Item":
                                var amazonItem = new AmazonItem();
                                foreach (XmlNode item in items)
                                {
                                    switch (item.Name)
                                    {

                                        case "ASIN":
                                            amazonItem.ASIN = item.InnerText;
                                            break;

                                        case "ParentASIN":
                                            amazonItem.ParentASIN = item.InnerText;
                                            break;

                                        case "DetailPageURL":
                                            amazonItem.DetailPageURL = item.InnerText;
                                            break;

                                        case "SalesRank":
                                            amazonItem.SalesRank = Convert.ToDouble(item.InnerText);
                                            break;

                                        case "ItemLinks":
                                            // var amazonItemLinkItems = new AmazonItemLinkItems();
                                            foreach (XmlNode Itemlink in item)
                                            {
                                                foreach (XmlNode ItemlinkItems in Itemlink)
                                                {
                                                    switch (ItemlinkItems.Name)
                                                    {
                                                        case "Description":
                                                            amazonItem.itemLinks.itemLinkItems.Description = ItemlinkItems.InnerText;
                                                            break;

                                                        case "URL":
                                                            amazonItem.itemLinks.itemLinkItems.Description = ItemlinkItems.InnerText;
                                                            break;
                                                    }
                                                }

                                            }
                                            //AmazonResult. = item.InnerText;
                                            break;

                                        case "SmallImage":
                                            foreach (XmlNode ItemSmallImage in item)
                                            {
                                                switch (ItemSmallImage.Name)
                                                {
                                                    case "URL":
                                                        amazonItem.itemSmallImage.URL = ItemSmallImage.InnerText;
                                                        break;
                                                    case "Height":
                                                        amazonItem.itemSmallImage.Height = Convert.ToDouble(ItemSmallImage.InnerText);
                                                        break;
                                                    case "Width":
                                                        amazonItem.itemSmallImage.Width = Convert.ToDouble(ItemSmallImage.InnerText);
                                                        break;
                                                }
                                            }
                                            break;
                                        //AmazonResult. = item.InnerText;

                                        case "MediumImage":
                                            foreach (XmlNode ItemSmallImage in item)
                                            {
                                                switch (ItemSmallImage.Name)
                                                {
                                                    case "URL":
                                                        amazonItem.itemMediumImage.URL = ItemSmallImage.InnerText;
                                                        break;
                                                    case "Height":
                                                        amazonItem.itemMediumImage.Height = Convert.ToDouble(ItemSmallImage.InnerText);
                                                        break;
                                                    case "Width":
                                                        amazonItem.itemMediumImage.Width = Convert.ToDouble(ItemSmallImage.InnerText);
                                                        break;
                                                }
                                            }
                                            break;
                                        //AmazonResult. = item.InnerText;

                                        case "LargeImage":
                                            foreach (XmlNode ItemSmallImage in item)
                                            {
                                                switch (ItemSmallImage.Name)
                                                {
                                                    case "URL":
                                                        amazonItem.itemLargeImage.URL = ItemSmallImage.InnerText;
                                                        break;
                                                    case "Height":
                                                        amazonItem.itemLargeImage.Height = Convert.ToDouble(ItemSmallImage.InnerText);
                                                        break;
                                                    case "Width":
                                                        amazonItem.itemLargeImage.Width = Convert.ToDouble(ItemSmallImage.InnerText);
                                                        break;
                                                }
                                            }
                                            break;
                                        //AmazonResult. = item.InnerText;

                                        case "ImageSets":
                                            foreach (XmlNode imageSetImages in item)
                                            {
                                                var amazonImageSets = new AmazonImageSets();
                                                var amazonImageSetImages = new AmazonImageSetImages();
                                                foreach (XmlNode images in imageSetImages)
                                                {

                                                    switch (images.Name)
                                                    {
                                                        case "SwatchImage":
                                                            //var swatchImage = new SwatchImage();
                                                            foreach (XmlNode SwatchImage in images)
                                                            {
                                                                switch (SwatchImage.Name)
                                                                {
                                                                    case "URL":
                                                                        amazonImageSetImages.swatchImage.URL = SwatchImage.InnerText;
                                                                        break;

                                                                    case "Height":
                                                                        amazonImageSetImages.swatchImage.Height = Convert.ToDouble(SwatchImage.InnerText);
                                                                        break;

                                                                    case "Width":
                                                                        amazonImageSetImages.swatchImage.Width = Convert.ToDouble(SwatchImage.InnerText);
                                                                        break;
                                                                }
                                                            }
                                                            break;

                                                        case "SmallImage":
                                                            // var smallImage = new SmallImage();
                                                            foreach (XmlNode SmallImage in images)
                                                            {
                                                                switch (SmallImage.Name)
                                                                {
                                                                    case "URL":
                                                                        amazonImageSetImages.smallImage.URL = SmallImage.InnerText;
                                                                        break;

                                                                    case "Height":
                                                                        amazonImageSetImages.smallImage.Height = Convert.ToDouble(SmallImage.InnerText);
                                                                        break;

                                                                    case "Width":
                                                                        amazonImageSetImages.smallImage.Width = Convert.ToDouble(SmallImage.InnerText);
                                                                        break;
                                                                }
                                                            }
                                                            break;


                                                        case "ThumbnailImage":
                                                            // var thumbnailImage = new ThumbnailImage();
                                                            foreach (XmlNode SmallImage in images)
                                                            {
                                                                switch (SmallImage.Name)
                                                                {
                                                                    case "URL":
                                                                        amazonImageSetImages.thumbnailImage.URL = SmallImage.InnerText;
                                                                        break;

                                                                    case "Height":
                                                                        amazonImageSetImages.thumbnailImage.Height = Convert.ToDouble(SmallImage.InnerText);
                                                                        break;

                                                                    case "Width":
                                                                        amazonImageSetImages.thumbnailImage.Width = Convert.ToDouble(SmallImage.InnerText);
                                                                        break;
                                                                }
                                                            }
                                                            break;


                                                        case "TinyImage":
                                                            // var tinyImage = new TinyImage();
                                                            foreach (XmlNode TinyImage in images)
                                                            {
                                                                switch (TinyImage.Name)
                                                                {
                                                                    case "URL":
                                                                        amazonImageSetImages.tinyImage.URL = TinyImage.InnerText;
                                                                        break;

                                                                    case "Height":
                                                                        amazonImageSetImages.tinyImage.Height = Convert.ToDouble(TinyImage.InnerText);
                                                                        break;

                                                                    case "Width":
                                                                        amazonImageSetImages.tinyImage.Width = Convert.ToDouble(TinyImage.InnerText);
                                                                        break;
                                                                }
                                                            }
                                                            break;


                                                        case "MediumImage":
                                                            //var mediumImage = new MediumImage();
                                                            foreach (XmlNode MediumImage in images)
                                                            {
                                                                switch (MediumImage.Name)
                                                                {
                                                                    case "URL":
                                                                        amazonImageSetImages.mediumImage.URL = MediumImage.InnerText;
                                                                        break;

                                                                    case "Height":
                                                                        amazonImageSetImages.mediumImage.Height = Convert.ToDouble(MediumImage.InnerText);
                                                                        break;

                                                                    case "Width":
                                                                        amazonImageSetImages.mediumImage.Width = Convert.ToDouble(MediumImage.InnerText);
                                                                        break;
                                                                }
                                                            }
                                                            break;


                                                        case "LargeImage":
                                                            //var largeImage = new LargeImage();
                                                            foreach (XmlNode LargeImage in images)
                                                            {
                                                                switch (LargeImage.Name)
                                                                {
                                                                    case "URL":
                                                                        amazonImageSetImages.largeImage.URL = LargeImage.InnerText;
                                                                        break;

                                                                    case "Height":
                                                                        amazonImageSetImages.largeImage.Height = Convert.ToDouble(LargeImage.InnerText);
                                                                        break;

                                                                    case "Width":
                                                                        amazonImageSetImages.largeImage.Width = Convert.ToDouble(LargeImage.InnerText);
                                                                        break;
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                //amazonItem.imageSets.imageSetImages.Add(amazonImageSetImages);
                                                amazonImageSets.imageSetImages.Add(amazonImageSetImages);
                                                amazonItem.imageSets.Add(amazonImageSets);
                                            }

                                            //AmazonResult. = item.InnerText;
                                            break;


                                        case "ItemAttributes":
                                            //var amazonItemAttributes = new AmazonItemAttributes();
                                            foreach (XmlNode ItemAttributes in item)
                                            {
                                                switch (ItemAttributes.Name)
                                                {
                                                    case "Binding":
                                                        amazonItem.itemAttributes.Binding = ItemAttributes.InnerText;
                                                        break;
                                                    case "Brand":
                                                        amazonItem.itemAttributes.Brand = ItemAttributes.InnerText;
                                                        break;
                                                    case "Color":
                                                        amazonItem.itemAttributes.Color = ItemAttributes.InnerText;
                                                        break;
                                                    case "Creator":
                                                        amazonItem.itemAttributes.Creator = ItemAttributes.InnerText;
                                                        break;
                                                    case "EAN":
                                                        amazonItem.itemAttributes.EAN = Convert.ToDouble(ItemAttributes.InnerText);
                                                        break;
                                                    case "Label":
                                                        amazonItem.itemAttributes.Label = ItemAttributes.InnerText;
                                                        break;
                                                    case "LegalDisclaimer":
                                                        amazonItem.itemAttributes.LegalDisclaimer = ItemAttributes.InnerText;
                                                        break;
                                                    case "Manufacturer":
                                                        amazonItem.itemAttributes.Manufacturer = ItemAttributes.InnerText;
                                                        break;
                                                    case "MPN":
                                                        amazonItem.itemAttributes.MPN = ItemAttributes.InnerText;
                                                        break;
                                                    case "UPC":
                                                        amazonItem.itemAttributes.UPC = ItemAttributes.InnerText;
                                                        break;
                                                    case "Feature":
                                                        foreach (XmlNode Feature in ItemAttributes)
                                                        {

                                                            amazonItem.itemAttributes.attributeFeature.Add(new AttributeFeature { Feature = Feature.Value });

                                                        }
                                                        break;

                                                    case "OperatingSystem":
                                                        amazonItem.itemAttributes.OperatingSystem = ItemAttributes.InnerText;
                                                        break;
                                                    case "PackageQuantity":
                                                        amazonItem.itemAttributes.PackageQuantity = ItemAttributes.InnerText;
                                                        break;
                                                    case "PartNumber":
                                                        amazonItem.itemAttributes.PartNumber = ItemAttributes.InnerText;
                                                        break;
                                                    case "ProductGroup":
                                                        amazonItem.itemAttributes.ProductGroup = ItemAttributes.InnerText;
                                                        break;
                                                    case "ProductTypeName":
                                                        amazonItem.itemAttributes.ProductTypeName = ItemAttributes.InnerText;
                                                        break;
                                                    case "Publisher":
                                                        amazonItem.itemAttributes.Publisher = ItemAttributes.InnerText;
                                                        break;
                                                    case "Size":
                                                        amazonItem.itemAttributes.Size = ItemAttributes.InnerText;
                                                        break;
                                                    case "Studio":
                                                        amazonItem.itemAttributes.Studio = ItemAttributes.InnerText;
                                                        break;
                                                    case "Title":
                                                        amazonItem.itemAttributes.Title = ItemAttributes.InnerText;
                                                        break;
                                                    case "Warranty":
                                                        amazonItem.itemAttributes.Warranty = ItemAttributes.InnerText;
                                                        break;


                                                    case "UPCList":
                                                        foreach (XmlNode UPCList in ItemAttributes)
                                                        {
                                                            switch (UPCList.Name)
                                                            {
                                                                case "UPCListElement":
                                                                    amazonItem.itemAttributes.attributeUPCList.UPCListElement = Convert.ToDouble(UPCList.InnerText);
                                                                    break;
                                                            }
                                                        }
                                                        break;


                                                    case "EANList":
                                                        foreach (XmlNode EANList in ItemAttributes)
                                                        {
                                                            switch (EANList.Name)
                                                            {
                                                                case "EANListElement":
                                                                    amazonItem.itemAttributes.attributeEANList.EANListElement = Convert.ToDouble(EANList.InnerText);
                                                                    break;
                                                            }
                                                        }
                                                        break;


                                                    case "PackageDimensions":
                                                        //var attributePackageDimensions = new AttributePackageDimensions();
                                                        foreach (XmlNode PackageDimensions in ItemAttributes)
                                                        {

                                                            switch (PackageDimensions.Name)
                                                            {
                                                                case "Height":
                                                                    amazonItem.itemAttributes.attributePackageDimensions.Height = Convert.ToDouble(PackageDimensions.InnerText);
                                                                    break;
                                                                case "Length":
                                                                    amazonItem.itemAttributes.attributePackageDimensions.Length = Convert.ToDouble(PackageDimensions.InnerText);
                                                                    break;
                                                                case "Weight":
                                                                    amazonItem.itemAttributes.attributePackageDimensions.Weight = Convert.ToDouble(PackageDimensions.InnerText);
                                                                    break;
                                                                case "Width":
                                                                    amazonItem.itemAttributes.attributePackageDimensions.Width = Convert.ToDouble(PackageDimensions.InnerText);
                                                                    break;
                                                            }
                                                        }
                                                        break;


                                                    case "ListPrice":
                                                        //var attributeListPrice = new AttributeListPrice();
                                                        foreach (XmlNode ListPrice in ItemAttributes)
                                                        {
                                                            switch (ListPrice.Name)
                                                            {
                                                                case "Amount":
                                                                    amazonItem.itemAttributes.attributeListPrice.Amount = Convert.ToDouble(ListPrice.InnerText);
                                                                    break;
                                                                case "CurrencyCode":
                                                                    amazonItem.itemAttributes.attributeListPrice.CurrencyCode = ListPrice.InnerText;
                                                                    break;
                                                                case "FormattedPrice":
                                                                    amazonItem.itemAttributes.attributeListPrice.FormattedPrice = ListPrice.InnerText;
                                                                    break;
                                                            }
                                                        }
                                                        break;


                                                    case "ItemDimensions":
                                                        // var attributeItemDimensions = new AttributeItemDimensions();
                                                        foreach (XmlNode ItemDimensions in ItemAttributes)
                                                        {
                                                            switch (ItemDimensions.Name)
                                                            {
                                                                case "Height":
                                                                    amazonItem.itemAttributes.attributeItemDimensions.Height = Convert.ToDouble(ItemDimensions.InnerText);
                                                                    break;
                                                                case "Length":
                                                                    amazonItem.itemAttributes.attributeItemDimensions.Length = Convert.ToDouble(ItemDimensions.InnerText);
                                                                    break;
                                                                case "Weight":
                                                                    amazonItem.itemAttributes.attributeItemDimensions.Weight = Convert.ToDouble(ItemDimensions.InnerText);
                                                                    break;
                                                                case "Width":
                                                                    amazonItem.itemAttributes.attributeItemDimensions.Width = Convert.ToDouble(ItemDimensions.InnerText);
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            //AmazonResult. = item.InnerText;
                                            break;

                                        case "OfferSummary":
                                            //var amazonOfferSummary = new AmazonOfferSummary();
                                            foreach (XmlNode OfferSummary in item)
                                            {

                                                switch (OfferSummary.Name)
                                                {

                                                    case "TotalNew":
                                                        amazonItem.offerSummary.TotalNew = Convert.ToDouble(OfferSummary.InnerText);
                                                        break;
                                                    case "TotalUsed":
                                                        amazonItem.offerSummary.TotalUsed = Convert.ToDouble(OfferSummary.InnerText);
                                                        break;
                                                    case "TotalCollectible":
                                                        amazonItem.offerSummary.TotalCollectible = Convert.ToDouble(OfferSummary.InnerText);
                                                        break;
                                                    case "TotalRefurbished":
                                                        amazonItem.offerSummary.TotalRefurbished = Convert.ToDouble(OfferSummary.InnerText);
                                                        break;
                                                    case "LowestNewPrice":
                                                        foreach (XmlNode LowestNewPrice in OfferSummary)
                                                        {
                                                            //var offerSummaryLowestNewPrice = new OfferSummaryLowestNewPrice();
                                                            switch (LowestNewPrice.Name)
                                                            {
                                                                case "Amount":
                                                                    amazonItem.offerSummary.offerSummaryLowestNewPrice.Amount = Convert.ToDouble(LowestNewPrice.InnerText);
                                                                    break;
                                                                case "CurrencyCode":
                                                                    amazonItem.offerSummary.offerSummaryLowestNewPrice.CurrencyCode = LowestNewPrice.InnerText;
                                                                    break;
                                                                case "FormattedPrice":
                                                                    amazonItem.offerSummary.offerSummaryLowestNewPrice.FormattedPrice = LowestNewPrice.InnerText;
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            //AmazonResult. = item.InnerText;
                                            break;

                                        case "Offers":
                                            //var amazonOffers = new AmazonOffers();
                                            foreach (XmlNode Offers in item)
                                            {
                                                switch (Offers.Name)
                                                {
                                                    case "TotalOffers":
                                                        amazonItem.offers.TotalOffers = Convert.ToDouble(Offers.InnerText);
                                                        break;
                                                    case "TotalOfferPages":
                                                        amazonItem.offers.TotalOfferPages = Convert.ToDouble(Offers.InnerText);
                                                        break;
                                                    case "MoreOffersUrl":
                                                        amazonItem.offers.MoreOffersUrl = Offers.InnerText;
                                                        break;
                                                    case "Offer":
                                                        foreach (XmlNode OfferItems in Offers)
                                                        {
                                                            switch (OfferItems.Name)
                                                            {
                                                                case "OfferListing":
                                                                    //var amazonOfferListing = new AmazonOfferListing();
                                                                    foreach (XmlNode OfferListing in OfferItems)
                                                                    {
                                                                        switch (OfferListing.Name)
                                                                        {
                                                                            case "OfferListingId":
                                                                                amazonItem.offers.amazonOfferItems.amazonOfferListing.OfferListingId = OfferListing.InnerText;
                                                                                break;
                                                                            case "Price":
                                                                                //var OfferItemsPrice = new OfferItemsPrice();
                                                                                foreach (XmlNode Price in OfferListing)
                                                                                {
                                                                                    switch (Price.Name)
                                                                                    {
                                                                                        case "Amount":
                                                                                            amazonItem.offers.amazonOfferItems.amazonOfferListing.offerItemsPrice.Amount = Convert.ToDouble(Price.InnerText);
                                                                                            break;
                                                                                        case "CurrencyCode":
                                                                                            amazonItem.offers.amazonOfferItems.amazonOfferListing.offerItemsPrice.CurrencyCode = Price.InnerText;
                                                                                            break;
                                                                                        case "FormattedPrice":
                                                                                            amazonItem.offers.amazonOfferItems.amazonOfferListing.offerItemsPrice.FormattedPrice = Price.InnerText;
                                                                                            break;
                                                                                    }
                                                                                }
                                                                                break;
                                                                            case "AmountSaved":
                                                                                //var offerItemsAmountSaved = new OfferItemsAmountSaved();
                                                                                foreach (XmlNode AmountSaved in OfferListing)
                                                                                {
                                                                                    switch (AmountSaved.Name)
                                                                                    {
                                                                                        case "Amount":
                                                                                            amazonItem.offers.amazonOfferItems.amazonOfferListing.offerItemsAmountSaved.Amount = Convert.ToDouble(AmountSaved.InnerText);
                                                                                            break;
                                                                                        case "CurrencyCode":
                                                                                            amazonItem.offers.amazonOfferItems.amazonOfferListing.offerItemsAmountSaved.CurrencyCode = AmountSaved.InnerText;
                                                                                            break;
                                                                                        case "FormattedPrice":
                                                                                            amazonItem.offers.amazonOfferItems.amazonOfferListing.offerItemsAmountSaved.FormattedPrice = AmountSaved.InnerText;
                                                                                            break;
                                                                                    }
                                                                                }
                                                                                break;
                                                                            case "PercentageSaved":
                                                                                amazonItem.offers.amazonOfferItems.amazonOfferListing.PercentageSaved = Convert.ToDouble(OfferListing.InnerText);
                                                                                break;
                                                                            case "Availability":
                                                                                amazonItem.offers.amazonOfferItems.amazonOfferListing.Availability = OfferListing.InnerText;
                                                                                break;
                                                                            case "AvailabilityAttributes":
                                                                                //var offerItemsAvailabilityAttributes = new OfferItemsAvailabilityAttributes();
                                                                                foreach (XmlNode AmountSaved in OfferListing)
                                                                                {
                                                                                    switch (AmountSaved.Name)
                                                                                    {
                                                                                        case "AvailabilityType":
                                                                                            amazonItem.offers.amazonOfferItems.amazonOfferListing.offerItemsAvailabilityAttributes.AvailabilityType = AmountSaved.InnerText;
                                                                                            break;
                                                                                        case "MaximumHours":
                                                                                            amazonItem.offers.amazonOfferItems.amazonOfferListing.offerItemsAvailabilityAttributes.MaximumHours = Convert.ToDouble(AmountSaved.InnerText);
                                                                                            break;
                                                                                        case "MinimumHours":
                                                                                            amazonItem.offers.amazonOfferItems.amazonOfferListing.offerItemsAvailabilityAttributes.MinimumHours = Convert.ToDouble(AmountSaved.InnerText);
                                                                                            break;
                                                                                    }
                                                                                }
                                                                                break;
                                                                            case "IsEligibleForSuperSaverShipping":
                                                                                amazonItem.offers.amazonOfferItems.amazonOfferListing.IsEligibleForSuperSaverShipping = Convert.ToDouble(OfferListing.InnerText);
                                                                                break;
                                                                            case "IsEligibleForPrime":
                                                                                amazonItem.offers.amazonOfferItems.amazonOfferListing.IsEligibleForPrime = Convert.ToDouble(OfferListing.InnerText);
                                                                                break;
                                                                        }
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            //AmazonResult. = item.InnerText;
                                            break;

                                        case "CustomerReviews":
                                            //var amazonCustomerReviews = new AmazonCustomerReviews();
                                            foreach (XmlNode CustomerReviews in item)
                                            {
                                                switch (CustomerReviews.Name)
                                                {
                                                    case "IFrameURL":
                                                        amazonItem.customerReviews.IFrameURL = CustomerReviews.InnerText;
                                                        break;

                                                    case "HasReviews":
                                                        amazonItem.customerReviews.HasReviews = Convert.ToBoolean(CustomerReviews.InnerText);
                                                        break;
                                                }
                                            }
                                            //AmazonResult. = item.InnerText;
                                            break;

                                        case "SimilarProducts":
                                            // var amazonSimilarProducts = new AmazonSimilarProducts();
                                            foreach (XmlNode SimilarProducts in item)
                                            {
                                                var amazonSimilarProductItems = new AmazonSimilarProductItems();
                                                foreach (XmlNode SimilarProductsItems in SimilarProducts)
                                                {
                                                    switch (SimilarProductsItems.Name)
                                                    {
                                                        case "ASIN":
                                                            amazonSimilarProductItems.ASIN = SimilarProductsItems.InnerText;
                                                            break;
                                                        case "Title":
                                                            amazonSimilarProductItems.Title = SimilarProductsItems.InnerText;
                                                            break;
                                                    }
                                                }
                                                amazonItem.similarProducts.similarProductItems.Add(amazonSimilarProductItems);
                                            }
                                            //AmazonResult. = item.InnerText;
                                            break;
                                    }
                                }
                                amazonSearchResult.amazonItems.Items.Add(amazonItem);
                                break;
                        }
                    }
                }
            }
            return amazonSearchResult;
        }

        public static EbaySearchResult EbayParsing(XmlDocument xml)
        {
            var ebayResult = new EbaySearchResult();
            if (xml.ChildNodes.Count > 1)
            {
                foreach (XmlNode node in xml.ChildNodes[1].ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "ack":
                            ebayResult.ack = node.InnerText;
                            break;

                        case "version":
                            ebayResult.version = node.InnerText;
                            break;

                        case "timestamp":
                            ebayResult.version = node.InnerText;
                            break;

                        case "itemSearchURL":
                            ebayResult.version = node.InnerText;
                            break;

                        case "paginationOutput":
                            foreach (XmlNode item in node)
                            {
                                switch (item.Name)
                                {
                                    case "pageNumber":
                                        ebayResult.paginationOutput.pageNumber = Convert.ToInt32(item.InnerText);
                                        break;
                                    case "entriesPerPage":
                                        ebayResult.paginationOutput.entriesPerPage = Convert.ToInt32(item.InnerText);
                                        break;
                                    case "totalPages":
                                        ebayResult.paginationOutput.totalPages = Convert.ToInt32(item.InnerText);
                                        break;
                                    case "totalEntries":
                                        ebayResult.paginationOutput.totalEntries = Convert.ToInt32(item.InnerText);
                                        break;
                                }
                            }
                            break;

                        case "searchResult":
                            foreach (XmlNode resultItems in node)
                            {
                                var ebayProduct = new EbayProduct();

                                foreach (XmlNode result in resultItems)
                                {

                                    switch (result.Name)
                                    {
                                        case "itemId":
                                            ebayProduct.itemId = Convert.ToDouble(result.InnerText);
                                            break;

                                        case "title":
                                            ebayProduct.title = result.InnerText;
                                            break;

                                        case "globalId":
                                            ebayProduct.globalId = result.InnerText;
                                            break;

                                        case "galleryURL":
                                            ebayProduct.galleryURL = result.InnerText;
                                            break;

                                        case "viewItemURL":
                                            ebayProduct.viewItemURL = result.InnerText;
                                            break;

                                        case "productId":
                                            ebayProduct.productId = Convert.ToDouble(result.InnerText);
                                            break;

                                        case "paymentMethod":
                                            ebayProduct.paymentMethod = result.InnerText;
                                            break;

                                        case "autoPay":
                                            ebayProduct.autoPay = Convert.ToBoolean(result.InnerText);
                                            break;

                                        case "postalCode":
                                            ebayProduct.postalCode = result.InnerText;
                                            break;

                                        case "location":
                                            ebayProduct.location = result.InnerText;
                                            break;

                                        case "country":
                                            ebayProduct.country = result.InnerText;
                                            break;

                                        case "primaryCategory":
                                            //var ebaycategory = new EbayProductCategory();
                                            foreach (XmlNode CategoryItem in result.ChildNodes)
                                            {
                                                switch (CategoryItem.Name)
                                                {
                                                    case "categoryId":
                                                        ebayProduct.productCategory.categoryId = Convert.ToDouble(CategoryItem.InnerText);
                                                        break;
                                                    case "categoryName":
                                                        ebayProduct.productCategory.categoryName = CategoryItem.InnerText;
                                                        break;
                                                }
                                            }
                                            // ebayProduct.productCategory.r.Add(ebayProduct);
                                            break;

                                        case "shippingInfo":
                                            // var ebayshippingInfo = new EbayShippingInfo();
                                            foreach (XmlNode ShippingItem in result.ChildNodes)
                                            {
                                                switch (ShippingItem.Name)
                                                {
                                                    case "shippingServiceCost ":
                                                        ebayProduct.shippingInfo.shippingServiceCost = Convert.ToDouble(ShippingItem.InnerText);
                                                        break;
                                                    case "shippingType":
                                                        ebayProduct.shippingInfo.shippingType = ShippingItem.InnerText;
                                                        break;
                                                    case "shipToLocations":
                                                        ebayProduct.shippingInfo.shipToLocations = ShippingItem.InnerText;
                                                        break;
                                                    case "expeditedShipping":
                                                        ebayProduct.shippingInfo.expeditedShipping = Convert.ToBoolean(ShippingItem.InnerText);
                                                        break;
                                                    case "oneDayShippingAvailable":
                                                        ebayProduct.shippingInfo.oneDayShippingAvailable = Convert.ToBoolean(ShippingItem.InnerText);
                                                        break;
                                                    case "handlingTime":
                                                        ebayProduct.shippingInfo.handlingTime = Convert.ToDouble(ShippingItem.InnerText);
                                                        break;
                                                }
                                            }
                                            // ebayResult.searchResult.ebayPshroducts.EbayShippingInfo = (shippingInfo);
                                            break;

                                        case "sellingStatus":
                                            //var ebaysellingStatus = new EbaySellingStatus();
                                            foreach (XmlNode SellingStatusItem in result.ChildNodes)
                                            {
                                                switch (SellingStatusItem.Name)
                                                {
                                                    case "currentPrice":
                                                        ebayProduct.sellingStatus.currentPrice = Convert.ToDouble(SellingStatusItem.InnerText);
                                                        break;
                                                    case "convertedCurrentPrice":
                                                        ebayProduct.sellingStatus.convertedCurrentPrice = Convert.ToDouble(SellingStatusItem.InnerText);
                                                        break;
                                                    case "sellingState":
                                                        ebayProduct.sellingStatus.sellingState = SellingStatusItem.InnerText;
                                                        break;
                                                    case "timeLeft":
                                                        ebayProduct.sellingStatus.timeLeft = SellingStatusItem.InnerText;
                                                        break;
                                                }
                                            }
                                            // ebayResult.searchResult.ebayProducts.Add();
                                            break;

                                        case "listingInfo":
                                            // var ebaylistingInfo = new EbayListingInfo();
                                            foreach (XmlNode ListingInfoItem in result.ChildNodes)
                                            {
                                                switch (ListingInfoItem.Name)
                                                {
                                                    case "bestOfferEnabled":
                                                        ebayProduct.listingInfo.bestOfferEnabled = Convert.ToBoolean(ListingInfoItem.InnerText);
                                                        break;
                                                    case "buyItNowAvailable":
                                                        ebayProduct.listingInfo.buyItNowAvailable = Convert.ToBoolean(ListingInfoItem.InnerText);
                                                        break;
                                                    case "startTime":
                                                        ebayProduct.listingInfo.startTime = Convert.ToDateTime(ListingInfoItem.InnerText);
                                                        break;
                                                    case "endTime":
                                                        ebayProduct.listingInfo.endTime = Convert.ToDateTime(ListingInfoItem.InnerText);
                                                        break;
                                                    case "listingType":
                                                        ebayProduct.listingInfo.listingType = ListingInfoItem.InnerText;
                                                        break;
                                                    case "gift":
                                                        ebayProduct.listingInfo.gift = Convert.ToBoolean(ListingInfoItem.InnerText);
                                                        break;
                                                }
                                            }
                                            //   ebayResult.searchResult.ebayProducts.Add();
                                            break;

                                        case "returnsAccepted":
                                            ebayProduct.returnsAccepted = Convert.ToBoolean(result.InnerText);
                                            break;

                                        case "condition":
                                            var ebaycondition = new EbayCondition();
                                            foreach (XmlNode ConditionItem in result.ChildNodes)
                                            {
                                                switch (ConditionItem.Name)
                                                {
                                                    case "conditionId":
                                                        ebaycondition.conditionId = Convert.ToDouble(ConditionItem.InnerText);
                                                        break;
                                                    case "conditionDisplayName":
                                                        ebaycondition.conditionDisplayName = ConditionItem.InnerText;
                                                        break;
                                                }
                                            }
                                            ebayResult.searchResult.ebayProducts.Add(ebayProduct);
                                            break;

                                        case "isMultiVariationListing":
                                            ebayProduct.isMultiVariationListing = Convert.ToBoolean(result.InnerText);
                                            break;

                                        case "topRatedListing":
                                            ebayProduct.topRatedListing = Convert.ToBoolean(result.InnerText);
                                            break;
                                    }
                                }
                            }
                            break;




                            //XmlSerializer serializer = new XmlSerializer(typeof(findItemsByKeywordsResponse));
                            //MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(res.FirstChild.NextSibling.InnerXml));
                            //findItemsByKeywordsResponse resultingMessage = (findItemsByKeywordsResponse)serializer.Deserialize(memStream);


                            //var ebaySearchResult  = new  List <findItemsByKeywordsResponse>();
                            //XmlSerializer deserializer = new XmlSerializer(typeof(List<findItemsByKeywordsResponse>),
                            //      new XmlRootAttribute("findItemsByKeywordsResponse"));

                            //XmlReader xmlReader = new XmlNodeReader(res);
                            //ebaySearchResult = (List<findItemsByKeywordsResponse>)deserializer.Deserialize(GenerateStreamFromString(res.InnerText));



                    }
                }
            }
            return ebayResult;
        }


        public static  AmazonSearchResult SearchAmazon(string keyword)
        {
            var urlReq = new URLRequest();
            urlReq.requestType = RequestType.AmazonRequest;

            urlReq.baseUrl = "http://webservices.amazon.com/onca/xml";
            //var dateStamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\%3Amm\\%3Ass.000Z");
            var dateStamp = URLHelper.UpercaseStringEncoding(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.000Z"));
            urlReq.requestParam.Add(new RequestParam { key = "AWSAccessKeyId", value = "AKIAJWOXS7D5DGAHHRIQ", paramType = ParamType.StringType });
            //urlReq.requestParam.Add(new RequestParam { key = "AWSSecretKey", value = "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", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "AssociateTag", value = "pricgrab21-20", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "Condition", value = "New", paramType = ParamType.StringType });

            urlReq.requestParam.Add(new RequestParam { key = "Availability", value = "Available", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "Keywords", value = string.IsNullOrEmpty(keyword) ? "iphone6s" : URLHelper.UpercaseStringEncoding(keyword), paramType = ParamType.StringType });

            urlReq.requestParam.Add(new RequestParam { key = "Operation", value = "ItemSearch", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "ResponseGroup", value = "Accessories%2CAlternateVersions%2CEditorialReview%2CImages%2CItemAttributes%2CItemIds%2COffers%2CReviews%2CSalesRank%2CSimilarities", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "SearchIndex", value = "All", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "Service", value = "AWSECommerceService", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "Timestamp", value = dateStamp, paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "Version", value = "2015-10-01", paramType = ParamType.StringType });
            //urlReq.requestParam.Add(new RequestParam { key = "Signature", value =  URLHelper.getSignatureKey("smDd2LH1GjOjrDZtTV4cjoP6mGi2CYN/I/mLxnfj", dateStamp, "ap-south-1", "AWSECommerceService"), paramType = ParamType.StringType });


            var xml = URLHelper.Request(urlReq);
            return AmazonParsing(xml);

        }

        public static EbaySearchResult SearchEbay(string searchQuery, bool isupc)
        {
            var urlReq = new URLRequest();
            urlReq.requestType = RequestType.EbayRequest;

            // urlReq.baseUrl = "http://svcs.ebay.com/services/search/FindingService/v1?OPERATION-NAME=findItemsByKeywords&SERVICE-VERSION=1.0.0&SECURITY-APPNAME=KakshaPa-PriceGra-PRD-39a6113c8-7268391b&RESPONSE-DATA-FORMAT=XML&REST-PAYLOAD&keywords=books";
            urlReq.baseUrl = "http://svcs.ebay.com/services/search/FindingService/v1";
            if (isupc)
            {
                urlReq.requestParam.Add(new RequestParam { key = "OPERATION-NAME", value = "findItemsByProduct", paramType = ParamType.StringType });
            }
            else
            {
                urlReq.requestParam.Add(new RequestParam { key = "OPERATION-NAME", value = "findItemsByKeywords", paramType = ParamType.StringType });
            }

            urlReq.requestParam.Add(new RequestParam { key = "SERVICE-VERSION", value = "1.0.0", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "SECURITY-APPNAME", value = "KakshaPa-PriceGra-PRD-39a6113c8-7268391b", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "RESPONSE-DATA-FORMAT", value = "XML", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "REST-PAYLOAD", value = "", paramType = ParamType.EmptyType });

            if (isupc)
            {
                urlReq.requestParam.Add(new RequestParam { key = "productId.@type", value = "UPC", paramType = ParamType.StringType });
                urlReq.requestParam.Add(new RequestParam { key = "productId", value = string.IsNullOrEmpty(searchQuery) ? "MacbookAir" : URLHelper.UpercaseStringEncoding(searchQuery), paramType = ParamType.StringType });
               
            }
            else
            {
                urlReq.requestParam.Add(new RequestParam { key = "keywords", value = string.IsNullOrEmpty(searchQuery) ? "MacbookAir" : URLHelper.UpercaseStringEncoding(searchQuery), paramType = ParamType.StringType });
                urlReq.requestParam.Add(new RequestParam { key = "paginationInput.entriesPerPage", value = "10", paramType = ParamType.StringType });
            }

            urlReq.requestParam.Add(new RequestParam { key = "item.conditionId", value = "1000", paramType = ParamType.StringType });

            //var urlRequest = new URLHelper();
            var xml = URLHelper.Request(urlReq);
            return EbayParsing(xml);
        }
    }



  



    public class AmazonSearchResult
    {
        public AmazonItems amazonItems { get; set; }
        public AmazonSearchResult()
        {
            amazonItems = new AmazonItems();
        }
    }

    public class AmazonItems
    {
        [XmlElement("TotalResults")]
        public string TotalResults { get; set; }

        [XmlElement("TotalPages")]
        public double TotalPages { get; set; }

        [XmlElement("MoreSearchResultsUrl")]
        public string MoreSearchResultsUrl { get; set; }

        public AmazonRequest request { get; set; }

        public List<AmazonItem> Items { get; set; }
        public AmazonItems()
        {
            request = new AmazonRequest();
            Items = new List<AmazonItem>();
        }

    }

    public class AmazonRequest
    {
        public bool IsValid { get; set; }
        public AmazonItemSearchRequest itemSearchRequest { get; set; }
        public AmazonRequest()
        {
            itemSearchRequest = new AmazonItemSearchRequest();
        }
    }

    public class AmazonItemSearchRequest
    {
        public string Keywords { get; set; }

        public string SearchIndex { get; set; }

        public string ResponseGroup { get; set; }

        public AmazonItemSearchRequest()
        {

        }
    }

    public class AmazonItem
    {
        public string ASIN { get; set; }
        public string ParentASIN { get; set; }

        public string DetailPageURL { get; set; }

        public double SalesRank { get; set; }

        public AmazonItemSmallImage itemSmallImage { get; set; }
        public AmazonItemMediumImage itemMediumImage { get; set; }
        public AmazonItemLargeImage itemLargeImage { get; set; }
        public AmazonCustomerReviews customerReviews { get; set; }
        public AmazonSimilarProducts similarProducts { get; set; }
        public AmazonItemLinks itemLinks { get; set; }
        public List<AmazonImageSets> imageSets { get; set; }
        public AmazonItemAttributes itemAttributes { get; set; }
        public AmazonOffers offers { get; set; }
        public AmazonOfferSummary offerSummary { get; set; }
        public AmazonEditorialReviews editorialReviews { get; set; }
        public AmazonItem()
        {

            itemSmallImage = new AmazonItemSmallImage();
            itemMediumImage = new AmazonItemMediumImage();
            itemLargeImage = new AmazonItemLargeImage();
            customerReviews = new AmazonCustomerReviews();
            similarProducts = new AmazonSimilarProducts();
            itemLinks = new AmazonItemLinks();
            imageSets = new List<AmazonImageSets>();
            itemAttributes = new AmazonItemAttributes();
            offers = new AmazonOffers();
            offerSummary = new AmazonOfferSummary();
            editorialReviews = new AmazonEditorialReviews();
        }
    }

    public class AmazonItemSmallImage
    {
        public string URL { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        public AmazonItemSmallImage()
        {

        }
    }

    public class AmazonItemMediumImage
    {
        public string URL { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        public AmazonItemMediumImage()
        {

        }
    }


    public class AmazonItemLargeImage
    {
        public string URL { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public AmazonItemLargeImage()
        {

        }
    }


    public class AmazonCustomerReviews
    {
        public string IFrameURL { get; set; }
        public bool HasReviews { get; set; }

        public AmazonCustomerReviews()
        {

        }
    }


    public class AmazonSimilarProducts
    {
        public List<AmazonSimilarProductItems> similarProductItems { get; set; }

        public AmazonSimilarProducts()
        {
            similarProductItems = new List<AmazonSimilarProductItems>();
        }
    }
    public class AmazonSimilarProductItems
    {
        public string ASIN { get; set; }
        public string Title { get; set; }
        public AmazonSimilarProductItems()
        {

        }
    }


    public class AmazonItemLinks
    {
        public AmazonItemLinkItems itemLinkItems { get; set; }
        public AmazonItemLinks()
        {
            itemLinkItems = new AmazonItemLinkItems();
        }
    }
    public class AmazonItemLinkItems
    {
        public string Description { get; set; }

        public string URL { get; set; }

        public AmazonItemLinkItems()
        {

        }
    }
    public class AmazonImageSets
    {
        public List<AmazonImageSetImages> imageSetImages { get; set; }
        public AmazonImageSets()
        {
            imageSetImages = new List<AmazonImageSetImages>();
        }
    }
    public class AmazonImageSetImages
    {
        public SwatchImage swatchImage { get; set; }
        public SmallImage smallImage { get; set; }
        public ThumbnailImage thumbnailImage { get; set; }
        public TinyImage tinyImage { get; set; }
        public MediumImage mediumImage { get; set; }
        public LargeImage largeImage { get; set; }

        public AmazonImageSetImages()
        {
            swatchImage = new SwatchImage();
            smallImage = new SmallImage();
            thumbnailImage = new ThumbnailImage();
            tinyImage = new TinyImage();
            mediumImage = new MediumImage();
            largeImage = new LargeImage();
        }
    }
    public class SwatchImage
    {
        public string URL { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        public SwatchImage()
        {

        }
    }
    public class SmallImage
    {
        public string URL { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public SmallImage()
        {

        }
    }
    public class ThumbnailImage
    {
        public string URL { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        public ThumbnailImage()
        {

        }
    }
    public class TinyImage
    {
        public string URL { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        public TinyImage()
        {

        }
    }
    public class MediumImage
    {
        public string URL { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public MediumImage()
        {

        }
    }
    public class LargeImage
    {
        public string URL { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        public LargeImage()
        {

        }
    }


    public class AmazonItemAttributes
    {
        public string Binding { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Creator { get; set; }
        public double EAN { get; set; }
        public string UPC { get; set; }
        public string Label { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string OperatingSystem { get; set; }
        public string LegalDisclaimer { get; set; }
        public string MPN { get; set; }
        public string PackageQuantity { get; set; }
        public string PartNumber { get; set; }
        public string ProductGroup { get; set; }
        public string ProductTypeName { get; set; }
        public string Publisher { get; set; }
        public string Size { get; set; }
        public string Studio { get; set; }
        public string Title { get; set; }
        public string Warranty { get; set; }
        public string HardwarePlatform { get; set; }

        public AttributeEANList attributeEANList { get; set; }
        public AttributeUPCList attributeUPCList { get; set; }
        public AttributeItemDimensions attributeItemDimensions { get; set; }
        public AttributeListPrice attributeListPrice { get; set; }
        public AttributePackageDimensions attributePackageDimensions { get; set; }

        public List<AttributeFeature> attributeFeature { get; set; }
        public AmazonItemAttributes()
        {
            attributeFeature = new List<AttributeFeature>();
            attributeUPCList = new AttributeUPCList();
            attributeEANList = new AttributeEANList();
            attributeItemDimensions = new AttributeItemDimensions();
            attributeListPrice = new AttributeListPrice();
            attributePackageDimensions = new AttributePackageDimensions();
        }
    }
    public class AttributeFeature
    {
        public string Feature { get; set; }
        public AttributeFeature()
        {

        }
    }
    public class AttributeUPCList
    {
        public double UPCListElement { get; set; }
        public AttributeUPCList()
        {

        }
    }
    public class AttributeEANList
    {
        public double EANListElement { get; set; }
        public AttributeEANList()
        {

        }
    }
    public class AttributeItemDimensions
    {

        public double Height { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
        public double Width { get; set; }

        public AttributeItemDimensions()
        {

        }
    }
    public class AttributeListPrice
    {
        public string CurrencyCode { get; set; }
        public double Amount { get; set; }
        public string FormattedPrice { get; set; }

        public AttributeListPrice()
        {

        }
    }
    public class AttributePackageDimensions
    {
        public double Height { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
        public double Width { get; set; }

        public AttributePackageDimensions()
        {

        }
    }


    public class AmazonEditorialReviews
    {

        public EditorialReviewsItems editorialReviewsItems { get; set; }

        public AmazonEditorialReviews()
        {
            editorialReviewsItems = new EditorialReviewsItems();

        }
    }
    public class EditorialReviewsItems
    {
        public string Source { get; set; }
        public string Content { get; set; }
        public double IsLinkSuppressed { get; set; }

        public EditorialReviewsItems()
        {

        }
    }
    public class AmazonOffers
    {
        public double TotalOffers { get; set; }
        public double TotalOfferPages { get; set; }

        public string MoreOffersUrl { get; set; }

        public AmazonOfferItems amazonOfferItems { get; set; }
        public AmazonOffers()
        {
            amazonOfferItems = new AmazonOfferItems();
        }

    }
    public class AmazonOfferItems
    {
        public AmazonOfferListing amazonOfferListing { get; set; }
        public AmazonOfferItems()
        {
            amazonOfferListing = new AmazonOfferListing();
        }
    }
    public class AmazonOfferListing
    {
        public string OfferListingId { get; set; }
        public double PercentageSaved { get; set; }
        public double IsEligibleForSuperSaverShipping { get; set; }
        public double IsEligibleForPrime { get; set; }

        public string Availability { get; set; }

        public OfferItemsPrice offerItemsPrice { get; set; }

        public OfferItemsAmountSaved offerItemsAmountSaved { get; set; }

        public OfferItemsAvailabilityAttributes offerItemsAvailabilityAttributes { get; set; }


        public AmazonOfferListing()
        {
            offerItemsPrice = new OfferItemsPrice();
            offerItemsAmountSaved = new OfferItemsAmountSaved();
            offerItemsAvailabilityAttributes = new OfferItemsAvailabilityAttributes();
        }
    }
    public class OfferItemsPrice
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string FormattedPrice { get; set; }

        public OfferItemsPrice()
        {

        }
    }
    public class OfferItemsAmountSaved
    {

        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string FormattedPrice { get; set; }

        public OfferItemsAmountSaved()
        {

        }
    }
    public class OfferItemsAvailabilityAttributes
    {

        public string AvailabilityType { get; set; }
        public double MinimumHours { get; set; }
        public double MaximumHours { get; set; }

        public OfferItemsAvailabilityAttributes()
        {

        }
    }
    public class AmazonOfferSummary
    {
        public double TotalNew { get; set; }
        public double TotalUsed { get; set; }
        public double TotalCollectible { get; set; }
        public double TotalRefurbished { get; set; }

        public OfferSummaryLowestNewPrice offerSummaryLowestNewPrice { get; set; }

        public AmazonOfferSummary()
        {
            offerSummaryLowestNewPrice = new OfferSummaryLowestNewPrice();
        }
    }
    public class OfferSummaryLowestNewPrice
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string FormattedPrice { get; set; }


        public OfferSummaryLowestNewPrice()
        {

        }
    }

    public class EbaySearchResult
    {

        public string ack { get; set; }

        public string version { get; set; }

        public string timestamp { get; set; }

        //[XmlElement("categoryName")]
        //public string categoryName { get; set; }
        //[XmlElement("itemSearchURL")]
        public string itemSearchURL { get; set; }
        //[XmlElement("searchResult")]
        public SearchResult searchResult { get; set; }
        //[XmlElement("paginationOutput")]
        public PaginationOutput paginationOutput { get; set; }

        public EbaySearchResult()
        {
            searchResult = new SearchResult();
            paginationOutput = new PaginationOutput();
        }
    }
    [Serializable()]
    public class PaginationOutput
    {
        [XmlElement("pageNumber")]
        public int pageNumber { get; set; }

        [XmlElement("entriesPerPage")]
        public int entriesPerPage { get; set; }

        [XmlElement("totalPages")]
        public int totalPages { get; set; }

        [XmlElement("totalEntries")]
        public int totalEntries { get; set; }

        public PaginationOutput()
        {

        }
    }

    public class SearchResult
    {
        public List<EbayProduct> ebayProducts { get; set; }

        public SearchResult()
        {
            ebayProducts = new List<EbayProduct>();
        }
    }

    [Serializable()]
    public class EbayProductCategory
    {
        [XmlElement("categoryId")]
        public double categoryId { get; set; }

        [XmlElement("categoryName")]
        public string categoryName { get; set; }

        public EbayProductCategory()
        {

        }
    }

    [Serializable()]
    public class EbayShippingInfo
    {
        [XmlElement("shippingServiceCost ")]
        public double shippingServiceCost { get; set; }

        [XmlElement("shippingType")]
        public string shippingType { get; set; }

        [XmlElement("shipToLocations")]
        public string shipToLocations { get; set; }

        [XmlElement("expeditedShipping")]
        public bool expeditedShipping { get; set; }

        [XmlElement("oneDayShippingAvailable")]
        public bool oneDayShippingAvailable { get; set; }

        [XmlElement("handlingTime")]
        public double handlingTime { get; set; }

        public EbayShippingInfo()
        {

        }
    }

    [Serializable()]
    public class EbayListingInfo
    {
        [XmlElement("bestOfferEnabled ")]
        public bool bestOfferEnabled { get; set; }

        [XmlElement("buyItNowAvailable")]
        public bool buyItNowAvailable { get; set; }

        [XmlElement("startTime")]
        public DateTime startTime { get; set; }

        [XmlElement("endTime")]
        public DateTime endTime { get; set; }

        [XmlElement("listingType")]
        public string listingType { get; set; }

        [XmlElement("gift")]
        public bool gift { get; set; }

        public EbayListingInfo()
        {

        }
    }

    [Serializable()]
    public class EbaySellingStatus
    {
        [XmlElement("currentPrice ")]
        public double currentPrice { get; set; }

        [XmlElement("convertedCurrentPrice ")]
        public double convertedCurrentPrice { get; set; }

        [XmlElement("sellingState")]
        public string sellingState { get; set; }

        [XmlElement("timeLeft")]
        public string timeLeft { get; set; }

        public EbaySellingStatus()
        {

        }
    }

    [Serializable()]
    public class EbayCondition
    {
        [XmlElement("conditionId")]
        public double conditionId { get; set; }

        [XmlElement("conditionDisplayName")]
        public string conditionDisplayName { get; set; }

        public EbayCondition()
        {

        }
    }

    [Serializable()]
    public class EbayProduct
    {
        [XmlElement("itemId")]
        public double itemId { get; set; }

        [XmlElement("title")]
        public string title { get; set; }

        [XmlElement("globalId")]
        public string globalId { get; set; }

        [XmlElement("galleryURL")]
        public string galleryURL { get; set; }

        [XmlElement("viewItemURL")]
        public string viewItemURL { get; set; }

        [XmlElement("productId")]
        public double productId { get; set; }

        [XmlElement("paymentMethod")]
        public string paymentMethod { get; set; }

        [XmlElement("autoPay")]
        public bool autoPay { get; set; }

        [XmlElement("postalCode")]
        public string postalCode { get; set; }
        [XmlElement("location")]
        public string location { get; set; }

        [XmlElement("country")]
        public string country { get; set; }


        [XmlElement("returnsAccepted")]
        public bool returnsAccepted { get; set; }

        [XmlElement("isMultiVariationListing")]
        public bool isMultiVariationListing { get; set; }

        [XmlElement("topRatedListing")]
        public bool topRatedListing { get; set; }

        public EbayProductCategory productCategory { get; set; }

        public EbayShippingInfo shippingInfo { get; set; }

        public EbayListingInfo listingInfo { get; set; }

        public EbaySellingStatus sellingStatus { get; set; }

        public EbayCondition condition { get; set; }
        public EbayProduct()
        {
            productCategory = new EbayProductCategory();
            shippingInfo = new EbayShippingInfo();
            listingInfo = new EbayListingInfo();
            sellingStatus = new EbaySellingStatus();
            condition = new EbayCondition();
        }

    }

    public enum ParamType
    {
        StringType = 1,
        FileType = 2,
        EmptyType = 3,
    }

    public enum RequestType
    {
        AmazonRequest = 1,
        EbayRequest = 2,

    }

    public class RequestParam
    {
        public string key { get; set; }
        public string value { get; set; }

        public ParamType paramType { get; set; }
    }

    public class URLRequest
    {
        public string baseUrl { get; set; }
        public List<RequestParam> requestParam { get; set; }
        public RequestType requestType { get; set; }

        public URLRequest()
        {
            requestParam = new List<RequestParam>();
        }
    }

    public static class URLHelper
    {

        public static string UpercaseStringEncoding(string input)
        {
            var encode = HttpUtility.UrlEncode(input).Replace("+", "%20");
            Regex reg = new Regex(@"%[a-f0-9]{2}");
            return reg.Replace(encode, m => m.Value.ToUpperInvariant());
        }
        private static string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }

        }

        static string hmacSHA256(String data, String key)
        {
            var encoding = new System.Text.ASCIIEncoding();
            using (HMACSHA256 hmac = new HMACSHA256(encoding.GetBytes(key)))
            {
                return Convert.ToBase64String(hmac.ComputeHash(encoding.GetBytes(data)));
            }
        }

        public static XmlDocument Request(URLRequest url)
        {
            string serviceUrl = string.Format("{0}?", url.baseUrl);
            var paramlist = "";
            foreach (var param in url.requestParam)
            {
                switch (param.paramType)
                {
                    case ParamType.EmptyType:
                        serviceUrl = string.Format("{0}&{1}", serviceUrl, param.key);

                        break;
                    case ParamType.StringType:
                        serviceUrl = string.Format("{0}{3}{1}={2}", serviceUrl, param.key, param.value, serviceUrl.EndsWith("?") ? "" : "&");
                        paramlist = string.Format("{0}{3}{1}={2}", paramlist, param.key, param.value, string.IsNullOrEmpty(paramlist) ? "" : "&");

                        break;

                    case ParamType.FileType:
                        break;
                }
            }

            try
            {

            }
            catch (Exception ex)
            {
                return new XmlDocument();
            }

            if (url.requestType == RequestType.AmazonRequest)
            {
                var secreat = "smDd2LH1GjOjrDZtTV4cjoP6mGi2CYN/I/mLxnfj";
                var endpoint = "webservices.amazon.com";
                var uri = "/onca/xml";
                var sign = "GET\n" + endpoint + "\n" + uri + "\n" + paramlist;
                var signature = hmacSHA256(sign, secreat);
                serviceUrl = string.Format("{0}&Signature={1}", serviceUrl, UpercaseStringEncoding(signature));
            }

            try
            {
                HttpWebRequest http = (HttpWebRequest)WebRequest.Create(serviceUrl);
                WebResponse response = http.GetResponse();


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return (xmlDoc);
            }
            catch (Exception ex)
            {
                XmlDocument xmlDoc = new XmlDocument();
                return (xmlDoc);
            }




            //var stream = response.GetResponseStream();
            //StreamReader sr = new StreamReader(stream);
            //string content = sr.ReadToEnd();
            //System.IO.File.WriteAllText(@"C:\Users\Rutu Patel\Documents\Price Grabber\Price Grabber\Price Grabber\ebay.xml", content);

            //// var settings = content.XmlDeserializeFromString<findItemsByKeywordsResponse>();

            //XmlSerializer ser = new XmlSerializer(typeof(findItemsByKeywordsResponse));
            //findItemsByKeywordsResponse myFoo = ser.Deserialize(new FileStream(@"C:\Users\Rutu Patel\Documents\Price Grabber\Price Grabber\Price Grabber\ebay.xml", FileMode.Open)) as findItemsByKeywordsResponse;
            //if (myFoo != null)
            //{

            //}
            //return content;

        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }
            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;
            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }
            return result;
        }







     

    }
}
