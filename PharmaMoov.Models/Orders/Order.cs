using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Orders
{
    public class Order : APIBaseModel
    {
        [Key]
        public int OrderID { get; set; }
        public string OrderReferenceID { get; set; }
        public string OrderInvoiceNo { get; set; }
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public Guid UserId { get; set; }
        public Guid PatientId { get; set; }
        public string CustomerName { get; set; }
        public string OrderNote { get; set; }
        public int DeliveryAddressId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDay { get; set; }
        public string DeliveryTime { get; set; }
        public string PromoCode { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderSubTotalAmount { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderVatAmount { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderPromoAmount { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderDeliveryFee { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderGrossAmount { get; set; }
        public OrderProgressStatus OrderProgressStatus { get; set; }
        public OrderDeliveryType OrderDeliveryType { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public OrderPaymentType OrderPaymentType { get; set; }
        public OrderPaymentStatus OrderPaymentStatus { get; set; }
        public DeliveryPackageType PackageType { get; set; }
        public int? DeliveryJobId { get; set; }
        public virtual DeliveryJob DeliveryJob { get; set; }
        public int? PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
    }

    public class NoVatOrder : APIBaseModel
    {
        [Key]
        public int OrderID { get; set; }
        public string OrderReferenceID { get; set; }
        public string OrderInvoiceNo { get; set; }
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public Guid UserId { get; set; }
        public Guid PatientId { get; set; }
        public string CustomerName { get; set; }
        public string OrderNote { get; set; }
        public int DeliveryAddressId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDay { get; set; }
        public string DeliveryTime { get; set; }
        public string PromoCode { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderSubTotalAmount { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderVatAmount { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderPromoAmount { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderDeliveryFee { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal OrderGrossAmount { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal NoVatOrderGrossAmount { get; set; }
        public OrderProgressStatus OrderProgressStatus { get; set; }
        public OrderDeliveryType OrderDeliveryType { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public OrderPaymentType OrderPaymentType { get; set; }
        public OrderPaymentStatus OrderPaymentStatus { get; set; }
        public DeliveryPackageType PackageType { get; set; }
        public int? DeliveryJobId { get; set; }
        public virtual DeliveryJob DeliveryJob { get; set; }
        public int? PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
    }

    public class OrderItem : APIBaseModel
    {
        [Key]
        public int OrderItemRecordID { get; set; }
        public int OrderID { get; set; }
        public int ProductRecordId { get; set; }
        [Column(TypeName = "decimal(4,1)")]
        public decimal ProductQuantity { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductPrice { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxValue { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxAmount { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal? ProductPricePerKG { get; set; }
        public string ProductUnit { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal SubTotal { get; set; }
    }

    public class OrderConfiguration : APIBaseModel
    {
        [Key]
        public int OrderConfigId { get; set; }
        public OrderConfigType ConfigType { get; set; }
        public int ConfigIntValue { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ConfigDecValue { get; set; }
        public string ConfigStrValue { get; set; }
    }
}
