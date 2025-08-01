using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PharmaMoov.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PharmaMoov.Models
{
    public enum EPaymentStatus
    {
        NotSpecified = 0,
        CREATED,
        SUCCEEDED,
        FAILED,
        REFUNDED,
        TRANSFERRED
    }

    public class MangoPayUserWallet : APIBaseModel
    {
        [Key]
        public int MangoPayUserWalletRecordID { get; set; }
        public string OwnerID { get; set; }
        public string Description { get; set; }
        public string Balance { get; set; }
        public string Currency { get; set; }
        public string FundsType { get; set; }
        public string WalletID { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ShopID { get; set; }
        public string Tag { get; set; }
    }

    public class Payment : APIBaseModel
    {
        [Key]
        public int Id { get; }
        public int OrderId { get; set; }
        public EPaymentStatus Status { get; set; }
        public string StatusMessage { get; set; }
        public string Tag { get; set; }
        public string AuthorId { get; set; }
        public string DebitedFundsCurrency { get; set; }
        public long DebitedFundsAmount { get; set; }
        public string FeesCurrency { get; set; }
        public long FeesAmount { get; set; }
        public string CreditedWalletId { get; set; }
        public string ReturnURL { get; set; }
        public string Culture { get; set; }
        public string CardType { get; set; }
        public string SecureMode { get; set; }
        public string CreditedUserId { get; set; }
        public string StatementDescriptor { get; set; }
        public string CreatePayload { get; set; }
        public string LastPayloadUpdate { get; set; }
        public DateTime? LastPayloadUpdateDate { get; set; }
        public string CardId { get; set; }
        [IgnoreDataMember]
        public virtual Order Order { get; set; }
    }

    public class PaymentDelivery: APIBaseModel
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string AuthorId { get; set; }
        public string CreditedUserId { get; set; }
        public CurrencyIso DebitedFundsCurrency { get; set; }
        public decimal DebitedFunds { get; set; }
        public CurrencyIso FeesCurrency { get; set; }
        public decimal Fees { get; set; }
        public string DebitedWalletId { get; set; }
        public string CreditedWalletId { get; set; }
        public EPaymentStatus Status { get; set; }

    }
    public class PaymentTransaction : APIBaseModel
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid UserId { get; set; }
        public Guid PatientId { get; set; }
        public EPaymentStatus PaymentStatus { get; set; }
        public string PaymentToken { get; set; }
        public string TransactionCode { get; set; }
    }

    public class PaymentResult
    {
        public int OrderId { get; set; }
        public string OrderReferenceID { get; set; }
        public EPaymentStatus OrderPaymentStatus { get; set; }
    }

    public class MangoPayShop : APIBaseModel
    {
        [Key]
        public int MagoPayShopRecordID { get; set; }
        public string ProofOfRegistration { get; set; }
        public string Statute { get; set; }
        public string LegalRepresentativeCountryOfResidence { get; set; }
        public string LegalRepresentativeNationality { get; set; }
        public DateTime? LegalRepresentativeBirthday { get; set; }
        public string LegalRepresentativeEmail { get; set; }
        public string LegalRepresentativeAddressObsolete { get; set; }
        public string LegalRepresentativeAddress { get; set; }
        public string LegalRepresentativeLastName { get; set; }
        public string LegalRepresentativeFirstName { get; set; }
        public string HeadquartersAddressObsolete { get; set; }
        public string HeadquartersAddress { get; set; }
        public string LegalPersonType { get; set; }
        public string CompanyNumber { get; set; }
        public string Name { get; set; }
        public string ShareholderDeclaration { get; set; }
        public string LegalRepresentativeProofOfIdentity { get; set; }
        public Guid PharmaShopId { get; set; }
        public string Id { get; set; }
        public string WalletID { get; set; }
        public KycLevel? KYCLevels { get; set; }
    }

    public class MangoPayUser : APIBaseModel
    {
        [Key]
        public int MangoPayUserRecordID { get; set; }
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("addressObsolete")]
        public string AddressObsolete { get; set; }
        [JsonProperty("birthday")]
        [System.Text.Json.Serialization.JsonConverter(typeof(MangoPay.SDK.Core.UnixDateTimeConverter))]
        public DateTime? Birthday { get; set; }
        [JsonProperty("birthplace")]
        public string Birthplace { get; set; }
        [JsonProperty("nationality")]
        [System.Text.Json.Serialization.JsonConverter(typeof(EnumerationConverter))]
        public string Nationality { get; set; }
        [JsonProperty("countryOfResidence")]
        [System.Text.Json.Serialization.JsonConverter(typeof(EnumerationConverter))]
        public string CountryOfResidence { get; set; }
        [JsonProperty("occupation")]
        public string Occupation { get; set; }
        [JsonProperty("incomeRange")]
        public int? IncomeRange { get; set; }
        [JsonProperty("proofOfIdentity")]
        public string ProofOfIdentity { get; set; }
        [JsonProperty("proofOfAddress")]
        public string ProofOfAddress { get; set; }
        [JsonProperty("pharmaMUserId")]
        public Guid PharmaMUserId { get; set; }
        public string KYCLevel { get; set; }
        [JsonProperty("walletID")]
        public string WalletID { get; set; }
    }

    public class CardRegistration : CardRegistrationDTO
    {
        [Key]
        public int RegistrationRecordID { get; set; }
        public Guid ApplicationUserID { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid CreatedBy { get; set; }
    }
    public class Card : APIBaseModel
    {
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Ce champs est requis.")]
        public string CardNumber { get; set; }
        [Required(ErrorMessage = "Ce champs est requis.")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Format incorrect")]
        public string Expiry { get; set; }
        [Required(ErrorMessage = "Ce champs est requis.")]
        public string Cvv { get; set; }
    }

}
