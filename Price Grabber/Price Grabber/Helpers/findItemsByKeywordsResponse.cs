namespace Price_Grabber.Helpers
{

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.ebay.com/marketplace/search/v1/services", IsNullable = false)]
    public partial class findItemsByKeywordsResponse
    {

        private string ackField;

        private string versionField;

        private System.DateTime timestampField;

        private findItemsByKeywordsResponseSearchResult searchResultField;

        private findItemsByKeywordsResponsePaginationOutput paginationOutputField;

        private string itemSearchURLField;

        /// <remarks/>
        public string ack
        {
            get
            {
                return this.ackField;
            }
            set
            {
                this.ackField = value;
            }
        }

        /// <remarks/>
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        public System.DateTime timestamp
        {
            get
            {
                return this.timestampField;
            }
            set
            {
                this.timestampField = value;
            }
        }

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResult searchResult
        {
            get
            {
                return this.searchResultField;
            }
            set
            {
                this.searchResultField = value;
            }
        }

        /// <remarks/>
        public findItemsByKeywordsResponsePaginationOutput paginationOutput
        {
            get
            {
                return this.paginationOutputField;
            }
            set
            {
                this.paginationOutputField = value;
            }
        }

        /// <remarks/>
        public string itemSearchURL
        {
            get
            {
                return this.itemSearchURLField;
            }
            set
            {
                this.itemSearchURLField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResult
    {

        private findItemsByKeywordsResponseSearchResultItem[] itemField;

        private byte countField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("item")]
        public findItemsByKeywordsResponseSearchResultItem[] item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItem
    {

        private ulong itemIdField;

        private string titleField;

        private string globalIdField;

        private string subtitleField;

        private findItemsByKeywordsResponseSearchResultItemPrimaryCategory primaryCategoryField;

        private string galleryURLField;

        private string viewItemURLField;

        private findItemsByKeywordsResponseSearchResultItemProductId productIdField;

        private string[] paymentMethodField;

        private bool autoPayField;

        private uint postalCodeField;

        private bool postalCodeFieldSpecified;

        private string locationField;

        private string countryField;

        private findItemsByKeywordsResponseSearchResultItemShippingInfo shippingInfoField;

        private findItemsByKeywordsResponseSearchResultItemSellingStatus sellingStatusField;

        private findItemsByKeywordsResponseSearchResultItemListingInfo listingInfoField;

        private bool returnsAcceptedField;

        private string galleryPlusPictureURLField;

        private findItemsByKeywordsResponseSearchResultItemCondition conditionField;

        private bool isMultiVariationListingField;

        private findItemsByKeywordsResponseSearchResultItemDiscountPriceInfo discountPriceInfoField;

        private bool topRatedListingField;

        /// <remarks/>
        public ulong itemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }

        /// <remarks/>
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public string globalId
        {
            get
            {
                return this.globalIdField;
            }
            set
            {
                this.globalIdField = value;
            }
        }

        /// <remarks/>
        public string subtitle
        {
            get
            {
                return this.subtitleField;
            }
            set
            {
                this.subtitleField = value;
            }
        }

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResultItemPrimaryCategory primaryCategory
        {
            get
            {
                return this.primaryCategoryField;
            }
            set
            {
                this.primaryCategoryField = value;
            }
        }

        /// <remarks/>
        public string galleryURL
        {
            get
            {
                return this.galleryURLField;
            }
            set
            {
                this.galleryURLField = value;
            }
        }

        /// <remarks/>
        public string viewItemURL
        {
            get
            {
                return this.viewItemURLField;
            }
            set
            {
                this.viewItemURLField = value;
            }
        }

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResultItemProductId productId
        {
            get
            {
                return this.productIdField;
            }
            set
            {
                this.productIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("paymentMethod")]
        public string[] paymentMethod
        {
            get
            {
                return this.paymentMethodField;
            }
            set
            {
                this.paymentMethodField = value;
            }
        }

        /// <remarks/>
        public bool autoPay
        {
            get
            {
                return this.autoPayField;
            }
            set
            {
                this.autoPayField = value;
            }
        }

        /// <remarks/>
        public uint postalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postalCodeSpecified
        {
            get
            {
                return this.postalCodeFieldSpecified;
            }
            set
            {
                this.postalCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResultItemShippingInfo shippingInfo
        {
            get
            {
                return this.shippingInfoField;
            }
            set
            {
                this.shippingInfoField = value;
            }
        }

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResultItemSellingStatus sellingStatus
        {
            get
            {
                return this.sellingStatusField;
            }
            set
            {
                this.sellingStatusField = value;
            }
        }

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResultItemListingInfo listingInfo
        {
            get
            {
                return this.listingInfoField;
            }
            set
            {
                this.listingInfoField = value;
            }
        }

        /// <remarks/>
        public bool returnsAccepted
        {
            get
            {
                return this.returnsAcceptedField;
            }
            set
            {
                this.returnsAcceptedField = value;
            }
        }

        /// <remarks/>
        public string galleryPlusPictureURL
        {
            get
            {
                return this.galleryPlusPictureURLField;
            }
            set
            {
                this.galleryPlusPictureURLField = value;
            }
        }

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResultItemCondition condition
        {
            get
            {
                return this.conditionField;
            }
            set
            {
                this.conditionField = value;
            }
        }

        /// <remarks/>
        public bool isMultiVariationListing
        {
            get
            {
                return this.isMultiVariationListingField;
            }
            set
            {
                this.isMultiVariationListingField = value;
            }
        }

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResultItemDiscountPriceInfo discountPriceInfo
        {
            get
            {
                return this.discountPriceInfoField;
            }
            set
            {
                this.discountPriceInfoField = value;
            }
        }

        /// <remarks/>
        public bool topRatedListing
        {
            get
            {
                return this.topRatedListingField;
            }
            set
            {
                this.topRatedListingField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItemPrimaryCategory
    {

        private ushort categoryIdField;

        private string categoryNameField;

        /// <remarks/>
        public ushort categoryId
        {
            get
            {
                return this.categoryIdField;
            }
            set
            {
                this.categoryIdField = value;
            }
        }

        /// <remarks/>
        public string categoryName
        {
            get
            {
                return this.categoryNameField;
            }
            set
            {
                this.categoryNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItemProductId
    {

        private string typeField;

        private uint valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public uint Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItemShippingInfo
    {

        private findItemsByKeywordsResponseSearchResultItemShippingInfoShippingServiceCost shippingServiceCostField;

        private string shippingTypeField;

        private string[] shipToLocationsField;

        private bool expeditedShippingField;

        private bool oneDayShippingAvailableField;

        private byte handlingTimeField;

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResultItemShippingInfoShippingServiceCost shippingServiceCost
        {
            get
            {
                return this.shippingServiceCostField;
            }
            set
            {
                this.shippingServiceCostField = value;
            }
        }

        /// <remarks/>
        public string shippingType
        {
            get
            {
                return this.shippingTypeField;
            }
            set
            {
                this.shippingTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("shipToLocations")]
        public string[] shipToLocations
        {
            get
            {
                return this.shipToLocationsField;
            }
            set
            {
                this.shipToLocationsField = value;
            }
        }

        /// <remarks/>
        public bool expeditedShipping
        {
            get
            {
                return this.expeditedShippingField;
            }
            set
            {
                this.expeditedShippingField = value;
            }
        }

        /// <remarks/>
        public bool oneDayShippingAvailable
        {
            get
            {
                return this.oneDayShippingAvailableField;
            }
            set
            {
                this.oneDayShippingAvailableField = value;
            }
        }

        /// <remarks/>
        public byte handlingTime
        {
            get
            {
                return this.handlingTimeField;
            }
            set
            {
                this.handlingTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItemShippingInfoShippingServiceCost
    {

        private string currencyIdField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string currencyId
        {
            get
            {
                return this.currencyIdField;
            }
            set
            {
                this.currencyIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItemSellingStatus
    {

        private findItemsByKeywordsResponseSearchResultItemSellingStatusCurrentPrice currentPriceField;

        private findItemsByKeywordsResponseSearchResultItemSellingStatusConvertedCurrentPrice convertedCurrentPriceField;

        private string sellingStateField;

        private string timeLeftField;

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResultItemSellingStatusCurrentPrice currentPrice
        {
            get
            {
                return this.currentPriceField;
            }
            set
            {
                this.currentPriceField = value;
            }
        }

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResultItemSellingStatusConvertedCurrentPrice convertedCurrentPrice
        {
            get
            {
                return this.convertedCurrentPriceField;
            }
            set
            {
                this.convertedCurrentPriceField = value;
            }
        }

        /// <remarks/>
        public string sellingState
        {
            get
            {
                return this.sellingStateField;
            }
            set
            {
                this.sellingStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string timeLeft
        {
            get
            {
                return this.timeLeftField;
            }
            set
            {
                this.timeLeftField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItemSellingStatusCurrentPrice
    {

        private string currencyIdField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string currencyId
        {
            get
            {
                return this.currencyIdField;
            }
            set
            {
                this.currencyIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItemSellingStatusConvertedCurrentPrice
    {

        private string currencyIdField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string currencyId
        {
            get
            {
                return this.currencyIdField;
            }
            set
            {
                this.currencyIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItemListingInfo
    {

        private bool bestOfferEnabledField;

        private bool buyItNowAvailableField;

        private System.DateTime startTimeField;

        private System.DateTime endTimeField;

        private string listingTypeField;

        private bool giftField;

        /// <remarks/>
        public bool bestOfferEnabled
        {
            get
            {
                return this.bestOfferEnabledField;
            }
            set
            {
                this.bestOfferEnabledField = value;
            }
        }

        /// <remarks/>
        public bool buyItNowAvailable
        {
            get
            {
                return this.buyItNowAvailableField;
            }
            set
            {
                this.buyItNowAvailableField = value;
            }
        }

        /// <remarks/>
        public System.DateTime startTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime endTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }

        /// <remarks/>
        public string listingType
        {
            get
            {
                return this.listingTypeField;
            }
            set
            {
                this.listingTypeField = value;
            }
        }

        /// <remarks/>
        public bool gift
        {
            get
            {
                return this.giftField;
            }
            set
            {
                this.giftField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItemCondition
    {

        private ushort conditionIdField;

        private string conditionDisplayNameField;

        /// <remarks/>
        public ushort conditionId
        {
            get
            {
                return this.conditionIdField;
            }
            set
            {
                this.conditionIdField = value;
            }
        }

        /// <remarks/>
        public string conditionDisplayName
        {
            get
            {
                return this.conditionDisplayNameField;
            }
            set
            {
                this.conditionDisplayNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItemDiscountPriceInfo
    {

        private findItemsByKeywordsResponseSearchResultItemDiscountPriceInfoOriginalRetailPrice originalRetailPriceField;

        private string pricingTreatmentField;

        private bool soldOnEbayField;

        private bool soldOffEbayField;

        /// <remarks/>
        public findItemsByKeywordsResponseSearchResultItemDiscountPriceInfoOriginalRetailPrice originalRetailPrice
        {
            get
            {
                return this.originalRetailPriceField;
            }
            set
            {
                this.originalRetailPriceField = value;
            }
        }

        /// <remarks/>
        public string pricingTreatment
        {
            get
            {
                return this.pricingTreatmentField;
            }
            set
            {
                this.pricingTreatmentField = value;
            }
        }

        /// <remarks/>
        public bool soldOnEbay
        {
            get
            {
                return this.soldOnEbayField;
            }
            set
            {
                this.soldOnEbayField = value;
            }
        }

        /// <remarks/>
        public bool soldOffEbay
        {
            get
            {
                return this.soldOffEbayField;
            }
            set
            {
                this.soldOffEbayField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponseSearchResultItemDiscountPriceInfoOriginalRetailPrice
    {

        private string currencyIdField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string currencyId
        {
            get
            {
                return this.currencyIdField;
            }
            set
            {
                this.currencyIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ebay.com/marketplace/search/v1/services")]
    public partial class findItemsByKeywordsResponsePaginationOutput
    {

        private byte pageNumberField;

        private byte entriesPerPageField;

        private uint totalPagesField;

        private uint totalEntriesField;

        /// <remarks/>
        public byte pageNumber
        {
            get
            {
                return this.pageNumberField;
            }
            set
            {
                this.pageNumberField = value;
            }
        }

        /// <remarks/>
        public byte entriesPerPage
        {
            get
            {
                return this.entriesPerPageField;
            }
            set
            {
                this.entriesPerPageField = value;
            }
        }

        /// <remarks/>
        public uint totalPages
        {
            get
            {
                return this.totalPagesField;
            }
            set
            {
                this.totalPagesField = value;
            }
        }

        /// <remarks/>
        public uint totalEntries
        {
            get
            {
                return this.totalEntriesField;
            }
            set
            {
                this.totalEntriesField = value;
            }
        }
    }



}