using PharmaMoov.Models.Cart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Orders
{
    public class UserOrderList
    {
        public int OrderID { get; set; }
        public string OrderReferenceID { get; set; }
        public OrderProgressStatus OrderProgressStatus { get; set; }
        public string ShopName { get; set; }
        public int NoOfProducts { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderGrossAmount { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string ScheduleTiming { get; set; }
        public Guid ShopId { get; set; }
        public string ShopIcon { get; set; }
        public string CompleteDeliveryAddress { get; set; }
        public int DeliveryAddressId { get; set; }
        public Guid UserId { get; set; }
        public Guid PatientId { get; set; }
    }

    public class UserOrderDetails
    {
        public int OrderID { get; set; }
        public Guid UserID { get; set; }
        public Guid PatientId { get; set; }
        public string OrderReferenceID { get; set; }
        public OrderProgressStatus OrderProgressStatus { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public DateTime DateAdded { get; set; }
       // public DateTime ScheduleDate { get; set; }
        public string ScheduleTiming { get; set; }
        public int DeliveryAddressId { get; set; }
        public string AddressName { get; set; }
        public string CompleteAddress { get; set; }
        public string MobileNumber { get; set; }
        public decimal OrderSubTotalAmount { get; set; }
        public decimal OrderVatAmount { get; set; }
       // public decimal OrderPromoAmount { get; set; }
        public decimal OrderDeliveryFee { get; set; }
        public decimal OrderGrossAmount { get; set; }
        public string OrderNote { get; set; }
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopIcon { get; set; }
        public bool IsAddressDisabled { get; set; }
    }

    public class OrderDetailsDTO
    {
        public UserOrderDetails OrderDetails { get; set; }
        public List<NewOrderItems> OrderItems { get; set; }
    }

    public class ShopOrderList
    {
        public int OrderID { get; set; }
        public string OrderReferenceID { get; set; }
        public string CustomerName { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderGrossAmount { get; set; }
        public OrderDeliveryType DeliveryTypeId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public OrderProgressStatus OrderProgressStatus { get; set; }
    }

    public class ChangeOrderStatus
    {
        public Guid UserId { get; set; }
        public int OrderId { get; set; }
        public OrderProgressStatus ProgressStatus { get; set; }
    }

    public class OrderListNotification
    {
        public Guid ShopId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public int OrderID { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
