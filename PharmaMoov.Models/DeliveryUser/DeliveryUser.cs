using MangoPay.SDK.Core;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.DeliveryUser
{
    public class DeliveryUserLocation : APIBaseModel
    {
        [Key]
        public int DeliveryUserLocationId { get; set; }
        public Guid DeliveryUserId { get; set; }

        [Column(TypeName = "decimal(10,8)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(11,8)")]
        public decimal Longitude { get; set; }
        public bool ReceiveOrder { get; set; }
    }
    public class DeliveryUserOrder : APIBaseModel
    {
        [Key]
        public int DeliveryUserOrderId { get; set; }
        public int OrderID { get; set; }
        public Guid DeliveryUserId { get; set; }       
        public DeliveryStatus DeliveryStatus { get; set; }
    }

    public class MangoPayCouriers : APIBaseModel
    {
        [Key]
        public int MangoPayCourierRecordID { get; set; }
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
        [System.Text.Json.Serialization.JsonConverter(typeof(UnixDateTimeConverter))]
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

    public class MangoPayCourierWallet : APIBaseModel
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
}
