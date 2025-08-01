using System;
using System.ComponentModel.DataAnnotations;

namespace PharmaMoov.Models.DeliveryUser
{
    public class UpdateLocationParamModel
    {       
        [Required(ErrorMessage = "Ce champs est requis.")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "Ce champs est requis.")]
        public decimal Longitude { get; set; }
    }    
    public class OrderListModel
    {
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int OrderID { get; set; }
        public string OrderReferenceID { get; set; }
        public OrderProgressStatus OrderProgressStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Guid UserId { get; set; }
        public Guid PatientId { get; set; }
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Distance { get; set; }
        public int DeliveryAddressId { get; set; }
    }
    public class OrderDetailModel
    {
        public int OrderID { get; set; }
        public string OrderReferenceID { get; set; }        
        public DateTime DeliveryDate { get; set; }
        public int OrderDeliveryType { get; set; }
        public OrderProgressStatus OrderProgressStatus { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public OrderPaymentType PaymentMethod { get; set; }
        public Guid UserId { get; set; }
        public Guid PatientId { get; set; }
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string AddressName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Building { get; set; }
        public string PostalCode { get; set; }
        public string AddressNote { get; set; }
        public int DeliveryAddressId { get; set; }
    }
    public class AcceptOrderParamModel
    {
        [Required(ErrorMessage = "Ce champs est requis.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Ce champs est requis.")]
        public Guid DeliveryUserId { get; set; }
    }
    public class UpdateOrderStatusParamModel
    {
        [Required(ErrorMessage = "Ce champs est requis.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Ce champs est requis.")]
        public Guid DeliveryUserId { get; set; }

        [Required(ErrorMessage = "Ce champs est requis.")]
        public OrderProgressStatus OrderStatus { get; set; }
    }
    public class UpdateReceiveOrderParamModel
    {
        [Required(ErrorMessage = "Ce champs est requis.")]
        public Guid DeliveryUserId { get; set; }
        public bool ReceiveOrder { get; set; }
    }
    public class OrderListParamModel
    {
        [Required(ErrorMessage = "Ce champs est requis.")]
        public Guid DeliveryUserId { get; set; }

        [Required(ErrorMessage = "Ce champs est requis.")]
        public DeliveryStatus DeliveryStatus { get; set; }
        public int PageNo { get; set; }
    }
    public class DeliveryUserNotificationModel
    {
        public Guid DeliveryUserId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Distance { get; set; }
        public DevicePlatforms  DevicePlatforms { get; set; }
        public string DeviceFCMToken { get; set; }
    }


}
