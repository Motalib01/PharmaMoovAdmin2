using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Shop
{
    public class Shop : APIBaseModel
    {
        [Key]
        public int ShopRecordID { get; set; }
        public Guid ShopId { get; set; }
        public int ShopCategoryId { get; set; }
        public AccountTypes AccountType { get; set; }
        public string ShopName { get; set; }
        public string ShopLegalName { get; set; }
        public string ShopTags { get; set; }
        public string OwnerName { get; set; }
        public string Description { get; set; }
        public string ShopIcon { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string TelephoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string TradeLicenseNo { get; set; }
        public string VATNumber { get; set; }
        public string Address { get; set; }
        public string SuiteAddress { get; set; }
        public string HeadquarterAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        [Column(TypeName = "decimal(10,8)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(11,8)")]
        public decimal Longitude { get; set; }
        public bool? HasOffers { get; set; }
        public OrderDeliveryType DeliveryMethod { get; set; }
        public string PreparationTimeForDelivery { get; set; }
        public string PreparationTime { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal DeliveryCommission { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal PickupCommission { get; set; }
        public string ShopBanner { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public int KbisNumber { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
        [Required(ErrorMessage = "IsPopularPharmacy cannot be null")]
        public bool IsPopularPharmacy { get; set; }
    }

    public class ShopCategory : APIBaseModel
    {
        [Key]
        public int ShopCategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int OrderSequence { get; set; }
    }

    public class ShopOpeningHour : APIBaseModel
    {
        [Key]
        public int ShopOpeningHourID { get; set; }
        public Guid ShopId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public string StartTimeAM { get; set; }
        public string EndTimeAM { get; set; }
        public string StartTimePM { get; set; }
        public string EndTimePM { get; set; }
        public string StartTimeEvening { get; set; }
        public string EndTimeEvening { get; set; }
        public bool NowOpen { get; set; }
        public OrderDeliveryType DeliveryType { get; set; }
    }

    public class ShopDocument : APIBaseModel
    {
        [Key]
        public int ShopDocumentID { get; set; }
        public Guid ShopId { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
    }

    public class UserShopFavorite : APIBaseModel
    {
        [Key]
        public int FavoriteRecordID { get; set; }
        public Guid ShopId { get; set; }
        public Guid UserId { get; set; }
    }

    public class ShopRequest : APIBaseModel
    {
        [Key]
        public int ShopRequestID { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string SuiteAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public int KbisNumber { get; set; }
        public string KbisDocument { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
    }

    public class ShopFAQ : APIBaseModel
    {
        [Key]
        public int ShopFAQID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Order { get; set; }
    }

    public class TermsAndCondition : APIBaseModel 
    {
        [Key]
        public int TermsAndConditionID { get; set; }
        public string TermsAndConditionBody { get; set; }
    }

    public class PrivacyPolicy : APIBaseModel 
    {
        [Key]
        public int PrivacyPolicyID { get; set; }
        public string PrivacyPolicyBody { get; set; }
    }

    public class ShopTermsAndCondition : APIBaseModel 
    {
        [Key]
        public int ShopTermsAndConditionID { get; set; }
        public string ShopTermsAndConditionBody { get; set; }
    }
}
