using PharmaMoov.Models.Campaign;
using PharmaMoov.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Shop
{
    public class ShopProfile
    {
        public int ShopRecordID { get; set; }

        public Guid ShopId { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        public AccountTypes AccountType { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        public string ShopName { get; set; }

        public string ShopLegalName { get; set; }

        public string ShopTags { get; set; }

        public string Description { get; set; }

        public string ShopIcon { get; set; }

        public int ShopCategoryId { get; set; }

        //[Required(ErrorMessage = "Ceci est nécessaire ")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Email requis.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email invalide. Veuillez réessayer.")]
        public string Email { get; set; }

        
        [MinLength(10, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numéro de téléphone invalide")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "You must enter a valid mobile number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        [MinLength(10, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numéro de téléphone invalide")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "You must enter a valid mobile number")]
        public string TelephoneNumber { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        public string Address { get; set; }

        public string SuiteAddress { get; set; }

        public string HeadquarterAddress { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        public string City { get; set; }

        [Column(TypeName = "decimal(10,8)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(11,8)")]
        public decimal Longitude { get; set; }

        public string TradeLicenseNo { get; set; }

        public string VATNumber { get; set; }

        [Column(TypeName = "decimal(16,2)")]
        public decimal DeliveryCommission { get; set; }

        [Column(TypeName = "decimal(16,2)")]
        public decimal PickupCommission { get; set; }

        public string ShopBanner { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        public int KbisNumber { get; set; }

        public bool IsPopularPharmacy { get; set; }
    }

    public class ShopsWithOffers
    {
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Distance { get; set; }
    }

    public class ShopCategoriesDTO
    {
        public int ShopCategoryId { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        public string ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
    }

    public class RegularCustomerCatalogue 
    {
        public List<CampaignDTO> HomePageBanners { get; set; }
        public List<PopularPharmaciesDTO> PopularPharmacies { get; set; }
        public List<CampaignDTO> BannersForOffers { get; set; }
        public List<ProductList> FeaturedProducts { get; set; }
        public List<ProductCategoriesDTO> ProductCategories { get; set; }
        public List<ProductList> FeaturedProductCategoriesOne { get; set; }
        public List<ProductList> FeaturedProductCategoriesTwo { get; set; }
    }

    public class ShopCatalogue
    {
        public List<CampaignDTO> CampaignBanners { get; set; }
        public List<ShopsWithOffers> ShopWithOffers { get; set; }
        public List<ShopCategoriesDTO> ShopCategories { get; set; }
    }

    public class PopularPharmaciesDTO 
    {
        public int ShopRecordId { get; set; }
        public Guid ShopID { get; set; }
        public string ShopName { get; set; }
        public string ShopDescription { get; set; }
        public string ShopIcon { get; set; }
        public string ShopTelephoneNumber { get; set; }
        public string ShopAddress { get; set; }
        public string ShopStatus { get; set; }
    }

    public class FilterMain
    {
        [Required(ErrorMessage = "Ceci est nécessaire ")]
        [Column(TypeName = "decimal(10,8)")]
        public double Latitude { get; set; }
        [Required(ErrorMessage = "Ceci est nécessaire ")]
        [Column(TypeName = "decimal(11,8)")]
        public double Longitude { get; set; }
        [Required(ErrorMessage = "Ceci est nécessaire ")]
        public OrderDeliveryType DeliveryMethod { get; set; }

        public DayOfWeek? OpeningDay { get; set; }
        public string OpeningHour { get; set; }
        public string ClosingHour { get; set; }
    }

    public class FilterShops
    {
        public int PageNumber { get; set; }
        public Guid? ShopId { get; set; }
        public Guid? UserId { get; set; }
        public bool ShopsWithOffers { get; set; }
        public int ShopCatergoryId { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        [Column(TypeName = "decimal(10,8)")]
        public double Latitude { get; set; }
        [Required(ErrorMessage = "Ceci est nécessaire ")]
        [Column(TypeName = "decimal(11,8)")]
        public double Longitude { get; set; }
        [Required(ErrorMessage = "Ceci est nécessaire ")]
        public OrderDeliveryType DeliveryMethod { get; set; }

        public DayOfWeek? OpeningDay { get; set; }
        public string OpeningHour { get; set; }
        public string ClosingHour { get; set; }

        public string SearchKey { get; set; }
        public bool IsFavorite { get; set; }
    }

    public class FilteredShops
    {
        public Guid ShopId { get; set; }
        public int ShopCategoryID { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string Address { get; set; }
        public string PreparationTime { get; set; }
        public OrderDeliveryType DeliveryMethod { get; set; }
        public string DeliveryNote { get; set; }
        public string Description { get; set; }
        public bool? IsFavorite { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Distance { get; set; }
    }

    public class ShopOpeningHourDTO
    {
        public int ShopOpeningHourID { get; set; }
        public Guid ShopId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTimeAM { get; set; }
        public TimeSpan EndTimeAM { get; set; }
        public TimeSpan StartTimePM { get; set; }
        public TimeSpan EndTimePM { get; set; }
        public TimeSpan StartTimeEvening { get; set; }
        public TimeSpan EndTimeEvening { get; set; }
        public bool NowOpen { get; set; }
    }

    public class ShopHourList
    {
        public Guid ShopId { get; set; }
        public Guid AdminId { get; set; }
        public List<ShopOpeningHourDTO> OpeningHours { get; set; }
    }

    public class ShopDetails
    {
        public Guid ShopId { get; set; }
        public string ImageUrl { get; set; }
        public string ShopBanner { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string Address { get; set; }
        public string PreparationTime { get; set; }
        public OrderDeliveryType DeliveryMethod { get; set; }
        public string DeliveryNote { get; set; }
        public string Description { get; set; }
        public decimal AverageRating { get; set; }
        public string TotalReviews { get; set; }
        public string MobileNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public string ShopStatus { get; set; }
        public IEnumerable<ShopOpeningHourDTO> WorkingHours { get; set; }       
    }
   

    public class ShopRequestDTO
    {
        public int ShopRequestID { get; set; }

        [Required(ErrorMessage = "Company Name requis.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Prénom requis.")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Email requis.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email invalide. Veuillez réessayer.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vous devez renseigner un numéro de téléphone valide")]
        [MinLength(10, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numéro de téléphone invalide")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "You must enter a valid mobile number")]
        public string MobileNumber { get; set; }

        public string Address { get; set; }
        public string SuiteAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public int KbisNumber { get; set; }
        public string KbisDocument { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
    }

    public class ShopConfigs
    {
        public Guid ShopId { get; set; }
        public Guid AdminId { get; set; }
        public string PreparationTime { get; set; }
        public OrderDeliveryType? DeliveryMethod { get; set; }
    }

    public class ShopFAQdto
    {
        public int ShopFAQID { get; set; }

        [Required(ErrorMessage = "Ce champs est requis.")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Ce champs est requis.")]
        public string Answer { get; set; }
        public bool? IsActive { get; set; }
        public int Order { get; set; }
    }

    public class ShopDocumentDTO
    {
        public Guid ShopId { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
    }

    public class ShopList
    {
        public int ShopRecordID { get; set; }
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Owner { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Status { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
        public bool IsPopularPharmacy { get; set; }
        public string ContactNumber { get; set; }
        public string ShopIcon { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class ShopCommissionDateRange 
    {
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
    }

    public class ShopComissionDTO 
    {
        public int ShopRecordId { get; set; }
        public string ShopName { get; set; }
        public decimal TotalOrder { get; set; }
        public decimal TotalSales { get; set; }
        public decimal PharmaMoovCommission { get; set; }
        public decimal PharmaMoovAmount { get; set; }
        public decimal ShopAmount { get; set; }
    }

    public class SingleShopComissionInvoiceParameters 
    {
        public int ShopRecordId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }

    public class SingleShopComissionInvoice 
    {
        public int ShopRecordId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDateRangeFrom { get; set; }
        public DateTime InvoiceDateRangeTo { get; set; }
        public List<SingleShopComissionInvoiceTransaction> Transactions { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal NetAmount { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal VatAmount { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmountWithTax { get; set; }
    }

    public class SingleShopComissionInvoiceTransaction
    {
        public OrderDeliveryType OrderType { get; set; }
        public int TotalNumberOfOrder { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmountOfSales { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal CommissionRate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal CommissionAmount { get; set; }
        public int FixTax { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal GeneralTaxAmount { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmountWithtax { get; set; }
    }

    public class ChangeRegistrationStatus
    {
        public int ShopRecordID { get; set; }
        public Guid AdminId { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
    }
    public class ShopCategoriesListModel
    {
        public int ShopCategoryId { get; set; }
        public string Name { get; set; }       
        public string ImageUrl { get; set; }       
    }
    public class FilterShopCategoriesModel
    {
        public int _pageNo { get; set; }
        public string _searchKey { get; set; }
    }
    public class FilterShopAddress
    {
        public string SearchKey { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        [Column(TypeName = "decimal(10,8)")]
        public double Latitude { get; set; } 

        [Required(ErrorMessage = "Ceci est nécessaire ")]
        [Column(TypeName = "decimal(11,8)")]
        public double Longitude { get; set; }       
    }
    public class ShopAddressModel
    {
        public Guid ShopId { get; set; }       
        public string ShopName { get; set; }
        public string ShopIcon { get; set; }
        public string MobileNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string SuiteAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Distance { get; set; }
        public string ShopStatus { get; set; }

    }
    public class ShopListParamModel
    {
        public string SearchKey { get; set; }
        public int PageNo { get; set; }
        public int SortBy { get; set; }
    }
    public class ShopListModel
    {
        public int ShopRecordId { get; set; }
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopIcon { get; set; }
        public string MobileNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ShopStatus { get; set; }
    }

    public class FullShopCatalogue
    {
        public ShopDetails ShopDetails { get; set; }
        public List<ProductCategoriesDTO> ShopProductCategories { get; set; }
        public ShopProductList ShopProductList { get; set; }
    }

    public class ShopProductList
    {
        public IEnumerable<FilteredProducts> ProductList { get; set; }
        public int PageCount { get; set; }
    }
    public class PharmacyOwner
    {
        public int ShopRecordID { get; set; }

        [Required(ErrorMessage = "Email requis.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email invalide. Veuillez réessayer.")]
        public string Email { get; set; }

        [MinLength(10, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numéro de téléphone invalide")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "You must enter a valid mobile number")]
        public string MobileNumber { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string ShopIcon { get; set; }
        public bool IsEnabled { get; set; }
        public Guid AdminID { get; set; }
    }

}
