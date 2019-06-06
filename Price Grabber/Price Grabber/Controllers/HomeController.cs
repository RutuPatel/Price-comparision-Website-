using Price_Grabber.Helpers;
using Price_Grabber.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace Price_Grabber.Controllers
{
    public class HomeController : Controller
    {

        public AmazonSearchResult SearchAmazon(string keyword)
        {
            var urlReq = new URLRequest();
            urlReq.requestType = RequestType.AmazonRequest;

            urlReq.baseUrl = "http://webservices.amazon.com/onca/xml";
            //var dateStamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\%3Amm\\%3Ass.000Z");
            var dateStamp = URLHelper.UpercaseStringEncoding(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.000Z"));
            urlReq.requestParam.Add(new RequestParam { key = "AWSAccessKeyId", value = "AKIAJWOXS7D5DGAHHRIQ", paramType = ParamType.StringType });
            //urlReq.requestParam.Add(new RequestParam { key = "AWSSecretKey", value = "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", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "AssociateTag", value = "pricgrab21-20", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "Availability", value = "Available", paramType = ParamType.StringType });
            urlReq.requestParam.Add(new RequestParam { key = "Keywords", value = string.IsNullOrEmpty(keyword) ? "Delllaptop" : URLHelper.UpercaseStringEncoding(keyword), paramType = ParamType.StringType });

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

        private EbaySearchResult SearchEbay(string searchQuery,bool isupc)
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
            //var urlRequest = new URLHelper();
            var xml = URLHelper.Request(urlReq);
            return EbayParsing(xml);
        }

        [HttpPost]
        public ActionResult Index(SearchModel model)
        {
            model.amazonSearchResult = SearchAmazon(model.SearchQuery);
            //model.ebaySearchResult = SearchEbay(model.SearchQuery,false);
            Session["SearchResult"] = model;

            return View(model);
        }

      

        [HttpGet]
        public ActionResult Index()
        {
            var model = new SearchModel();
            model.amazonSearchResult = SearchAmazon(model.SearchQuery);
            //model.ebaySearchResult = SearchEbay(model.SearchQuery,false);            
            Session["SearchResult"] = model;
            return View(model);

        }
 
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult AmazonProductDetails()
        {
                return View();
        }


        public AmazonSearchResult  AmazonParsing(XmlDocument xml)
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
                                                                    amazonItem.itemAttributes.attributeListPrice.Amount = ListPrice.InnerText;
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


        public EbaySearchResult EbayParsing(XmlDocument xml)
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


        public ActionResult ProductDetail(string ProductId , string ProductType)
        {
            var productList = new List<ProductModel>();
            if (Session["SearchResult"]!= null)
            {
                var model = (SearchModel)Session["SearchResult"];
                //if (ProductType == "EBAY")
                //{
                //    var product = model.ebaySearchResult.searchResult.ebayProducts.FirstOrDefault(x => x.itemId == Convert.ToDouble(ProductId));
                //    if (product != null) {
                //        var productModel = new ProductModel()
                //        {
                //            Name = product.title,
                //            EbayPrice = Convert.ToString(product.sellingStatus.currentPrice),
                //            Image = product.galleryURL.ToString(),
                //            EbayRedirect =product.viewItemURL,
                //            EbayAvailable=product.sellingStatus.sellingState,
                //            EbayshippingInfo=product.shippingInfo.shippingType
                //        };
                //    }
                //}
                //else 
                if (ProductType == "AMAZON")
                {
                    var amazonproductlisting = model.amazonSearchResult.amazonItems.Items.Where(x => x.itemAttributes.UPC == ProductId);
                    if (amazonproductlisting != null)
                    {
                        foreach (var product in amazonproductlisting)
                        {
                            var FeatureItems = new AttributeFeature();

                            var productModel = new ProductModel()
                            {
                                Name = product.itemAttributes.Title,
                                AmazonPrice = product.itemAttributes.attributeListPrice.FormattedPrice,
                                Image = product.itemMediumImage.URL,
                                AmazonRedirect = product.DetailPageURL,
                                Feature = FeatureItems.Feature,
                                AmaozonAvailable = product.offers.amazonOfferItems.amazonOfferListing.offerItemsAvailabilityAttributes.AvailabilityType,
                            };
                            List<string> SwatchImage = new List<string>();
                            foreach (var image in product.imageSets)
                            {
                                productModel.SwatchImage.Add(image.imageSetImages[0].swatchImage.URL);
                                productModel.SmallImage.Add(image.imageSetImages[0].smallImage.URL);
                                productModel.TinyImage.Add(image.imageSetImages[0].tinyImage.URL);
                                productModel.MediumImage.Add(image.imageSetImages[0].mediumImage.URL);
                                productModel.LargeImage.Add(image.imageSetImages[0].largeImage.URL);
                            }
                            foreach (var feature in product.itemAttributes.attributeFeature)
                            {
                                productModel.Features.Add(feature.Feature.ToString());
                            }
                            productList.Add(productModel);
                        }
                    }                   
                }

                var ebayProductListing = SearchEbay(ProductId.ToString(), true);

                foreach (var ebayproduct in ebayProductListing.searchResult.ebayProducts)
                {
                    var productModel = new ProductModel()
                    {
                        Name = ebayproduct.title,
                        EbayPrice = Convert.ToString(ebayproduct.sellingStatus.currentPrice),
                        Image = ebayproduct.galleryURL,
                        EbayRedirect = ebayproduct.viewItemURL,
                        EbayAvailable = ebayproduct.sellingStatus.sellingState,
                        EbayshippingInfo = ebayproduct.shippingInfo.shippingType
                    };
                    productList.Add(productModel);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(productList);
        }
    }
}