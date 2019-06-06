using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace Price_Grabber.Helpers
{
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
        public string Amount { get; set; }
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
        
        public string Availability{ get; set; }

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
                var sign = "GET\n"+ endpoint  + "\n" + uri + "\n" + paramlist;
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
            catch(Exception ex)
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