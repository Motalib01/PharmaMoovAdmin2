using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PharmaMoov.Models
{
    public class PaymentListParamModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Guid ShopId { get; set; }
    }
    public class PaymentListModel
    {
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int Status { get; set; }
    }
    public class PaymentInvoiceModel
    {
        public Guid UserId { get; set; }
        public Guid PatientId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public int DeliveryAddressId { get; set; }
        public string Address { get; set; }
        public string OrderReferenceID { get; set; }
        public int OrderType { get; set; }
        public DateTime PlacedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int OrderStatus { get; set; }
       // public decimal AmountDue { get; set; }
        public decimal OrderSubTotalAmount { get; set; }
        public decimal OrderVatAmount { get; set; }
        public decimal OrderDeliveryFee { get; set; }
        public decimal OrderGrossAmount { get; set; }
        public List<OrderItemListModel> OrderItemListModel { get; set; }

    }
    public class OrderItemListModel
    {
        public string ProductName { get; set; }
        public decimal ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ProductTaxValue { get; set; }
    }
}
