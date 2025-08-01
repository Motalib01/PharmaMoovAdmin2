using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PharmaMoov.Models
{
    public enum EDeliveryType
    {
        PICKUP,
        DELIVERY
    }
    public enum EDeliveryStatus
    {
        PENDING,
        FINISHED
    }

    public class JobModel
    {
        public int OrderId { get; set; }
        public string PickupComment { get; set; }
        public string DeliveryComment { get; set; }
        /// <summary>
        /// France only Which transport type you want for your Job. Mandatory if package_type is empty.
        /// bike, cargobike, cargobikexl, motorbike, motorbikexl, car, van
        /// </summary>
        public string TransportType { get; set; }
        /// <summary>
        /// Which package size you want to send. Mandatory if transport_type is empty. Spain and UK only
        /// xsmall, small, medium, large, xlarge
        /// </summary>
        public string PackageType { get; set; }
        public string PackageDescription { get; set; }
    }

    public class JobModelForMobile
    {
        public Guid ShopId { get; set; }
        public Guid CustomerId { get; set; }
        public int DeliveryAddressId { get; set; }
    }

    #region Stuart model
    public class JobInput
    {
        public string assignment_code { get; }
        public string transport_type { get; }
        public List<PickupInput> pickups { get; set; }
        public List<DropoffInput> dropoffs { get; set; }
        public JobInput(string iCode, string iTransportType)
        {
            assignment_code = iCode;
            transport_type = iTransportType;
            pickups = new List<PickupInput>();
            dropoffs = new List<DropoffInput>();
        }
    }

    public class PickupInput
    {
        public string address { get; set; }
        public string comment { get; set; }
        public ContactInput contact { get; set; }
        public PickupInput()
        {
            address = string.Empty;
            comment = string.Empty;
            contact = new ContactInput();
        }
    }

    public class DropoffInput
    {
        public string package_type { get; set; }
        public string package_description { get; set; }
        public string address { get; set; }
        public string comment { get; set; }
        public ContactInput contact { get; set; }
        public DropoffInput()
        {
            package_type = string.Empty;
            package_description = string.Empty;
            address = string.Empty;
            comment = string.Empty;
            contact = new ContactInput();
        }
    }

    public class ContactInput
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string company { get; set; }
        public ContactInput()
        {
            firstname = string.Empty;
            lastname = string.Empty;
            phone = string.Empty;
            email = string.Empty;
            company = string.Empty;
        }
    }
    #endregion

    #region response models
    public class TokenModel : ErrorModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
        [JsonProperty("created_at")]
        public int CreatedAt { get; set; }
    }

    public class AddressValidationModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class ErrorModel
    {
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class PricingModel : ErrorModel
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class JobValidationModel : ErrorModel
    {
        [JsonProperty("valid")]
        public bool Valid { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 


    public class Contact
    {
        [JsonProperty("firstname")]
        public string FirstName { get; set; }
        [JsonProperty("lastname")]
        public string LastName { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class PickupOutput
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("address")]
        public AddressOutput Address { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
        [JsonProperty("access_codes")]
        public List<object> AccessCode { get; set; }
    }

    public class AddressOutput
    {
        [JsonProperty("street")]
        public string Street { get; set; }
        [JsonProperty("postcode")]
        public string PostCode { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("zone")]
        public string Zone { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }
    }

    public class ShopContactOutput
    {
        [JsonProperty("firstname")]
        public string FirstName { get; set; }
        [JsonProperty("lastname")]
        public string LastName { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class DropoffOutput
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("address")]
        public AddressOutput Address { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("contact")]
        public ShopContactOutput Contact { get; set; }
        [JsonProperty("access_codes")]
        public List<object> AccessCodes { get; set; }
    }

    public class CancellationOutput
    {
        [JsonProperty("canceled_by")]
        public object CanceledBy { get; set; }
        [JsonProperty("reason_key")]
        public object ReasonKey { get; set; }
        [JsonProperty("comment")]
        public object Comment { get; set; }
    }

    public class EtaOutput
    {
        [JsonProperty("pickup")]
        public object Pickup { get; set; }
        [JsonProperty("dropoff")]
        public object Dropoff { get; set; }
    }

    public class ProofOutput
    {
        [JsonProperty("signature_url")]
        public object SignatureUrl { get; set; }
    }

    public class DeliveryOutput
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("picked_at")]
        public object PickedAt { get; set; }
        [JsonProperty("delivered_at")]
        public object DeliveredAt { get; set; }
        [JsonProperty("tracking_url")]
        public string TrackingUrl { get; set; }
        [JsonProperty("client_reference")]
        public object ClientReference { get; set; }
        [JsonProperty("package_description")]
        public string PackageDescription { get; set; }
        [JsonProperty("package_type")]
        public object PackageType { get; set; }
        [JsonProperty("fleet_ids")]
        public List<int> FleetIds { get; set; }
        [JsonProperty("pickup")]
        public PickupOutput Pickup { get; set; }
        [JsonProperty("dropoff")]
        public DropoffOutput Dropoff { get; set; }
        [JsonProperty("cancellation")]
        public CancellationOutput Cancellation { get; set; }
        [JsonProperty("eta")]
        public EtaOutput Eta { get; set; }
        [JsonProperty("proof")]
        public ProofOutput Proof { get; set; }
        [JsonProperty("package_image_url")]
        public object PackageImageUrl { get; set; }
    }

    public class PricingOutput
    {
        [JsonProperty("PriceTaxIncluded")]
        public double price_tax_included { get; set; }
        [JsonProperty("price_tax_excluded")]
        public double PriceTaxExcluded { get; set; }
        [JsonProperty("tax_amount")]
        public double TaxAmount { get; set; }
        [JsonProperty("invoice_url")]
        public object InvoiceUrl { get; set; }
        [JsonProperty("tax_percentage")]
        public double TaxPercentage { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class JobOutput : ErrorModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("package_type")]
        public string PackageType { get; set; }
        [JsonProperty("transport_type")]
        public string TransportType { get; set; }
        [JsonProperty("assignment_code")]
        public string AssignmentCode { get; set; }
        [JsonProperty("pickup_at")]
        public string PickupAt { get; set; }
        [JsonProperty("dropoff_at")]
        public string DropoffAt { get; set; }
        [JsonProperty("ended_at")]
        public string EndedAt { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("distance")]
        public double Distance { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("traveled_time")]
        public int TraveledTime { get; set; }
        [JsonProperty("traveled_distance")]
        public int TraveledDistance { get; set; }
        [JsonProperty("deliveries")]
        public List<DeliveryOutput> Deliveries { get; set; }
        [JsonProperty("Driver")]
        public object driver { get; set; }
        [JsonProperty("pricing")]
        public PricingOutput Pricing { get; set; }
        [JsonProperty("rating")]
        public object Rating { get; set; }
    }




    #endregion

    #region webhook

    public class WebhookResponse
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public WebhookData Data { get; set; }
    }

    public class WebhookData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("comment")]
        public object Comment { get; set; }
        [JsonProperty("pickupAt")]
        public object PickupAt { get; set; }

        [JsonProperty("dropoffAt")]
        public string DropoffAt { get; set; }
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("endedAt")]
        public string EndedAt { get; set; }
        [JsonProperty("originComment")]
        public string OriginComment { get; set; }
        [JsonProperty("destinationComment")]
        public string DestinationComment { get; set; }
        [JsonProperty("jobReference")]
        public string JobReference { get; set; }
        [JsonProperty("transportType")]
        public WebhookCode TransportType { get; set; }
        [JsonProperty("packageType")]
        public WebhookCode PackageType { get; set; }
        [JsonProperty("currentDelivery")]
        public WebhookCurrentDelivery CurrentDelivery { get; set; }
        [JsonProperty("deliveries")]
        public WebhookDeliveries Deliveries { get; set; }
    }

    public class WebhookCode
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class WebhookCurrentDelivery
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("trackingUrl")]
        public string TrackingUrl { get; set; }
        [JsonProperty("clientReference")]
        public string ClientReference { get; set; }
        [JsonProperty("etaToDestination")]
        public string EtaToDestination { get; set; }
        [JsonProperty("etaToOrigin")]
        public string EtaToOrigin { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("transportType")]
        public WebhookCode TransportType { get; set; }
        [JsonProperty("packageType")]
        public WebhookCode PackageType { get; set; }
        [JsonProperty("cancellation")]
        public WebhookCancellation Cancellation { get; set; }
        [JsonProperty("driver")]
        public WebhookDriver Driver { get; set; }
    }

    public class WebhookCancellation
    {
        [JsonProperty("canceledBy")]
        public string CanceledBy { get; set; }
        [JsonProperty("reasonKey")]
        public string ReasonKey { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }

    public class WebhookDriver
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }
        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("firstname")]
        public string Firstname { get; set; }
        [JsonProperty("lastname")]
        public string Lastname { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("picture_path_imgix")]
        public string Picture_path_imgix { get; set; }
        [JsonProperty("transportType")]
        public WebhookCode TransportType { get; set; }
    }

    public class WebhookDeliveries
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("clientReference")]
        public string ClientReference { get; set; }
    }

    #endregion
}
