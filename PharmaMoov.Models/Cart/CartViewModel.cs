using MangoPay.SDK.Entities.GET;
using PharmaMoov.Models.Orders;
using PharmaMoov.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Cart
{
    public class CartSyncDTO
    {
        public bool Sync { get; set; }
        public Guid ShopId { get; set; }
        public int PrescriptionRecordId { get; set; }
        public List<CartItemsDTO> CartItemsSync { get; set; }
    }

    public class CartItemsDTO
    {
        public int ProductRecordId { get; set; }
        [Column(TypeName = "decimal(4,1)")]
        public decimal ProductQuantity { get; set; }
    }

    public class UserCartItem
    {
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopStatus { get; set; }
        public int ProductRecordId { get; set; }
        public int PrescriptionRecordId { get; set; }
        public string ProductName { get; set; }
        [Column(TypeName = "decimal(4,1)")]
        public decimal ProductQuantity { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductPrice { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxValue { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxAmount { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal TotalAmount { get; set; }
        public string ProductIcon { get; set; }
        public string ShopIcon { get; set; }
        public decimal SalePrice { get; set; }
    }

    public class CheckoutCartItem
    {
        public Guid ShopId { get; set; }
        public Guid UserId { get; set; }
        public Guid PatientId { get; set; }
        public string OrderNote { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public int DeliveryAddressId { get; set; }
        public string CardId { get; set; }
        public OrderPaymentType PaymentType { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal DeliveryFee { get; set; }
        // public string PromoCode { get; set; }
        // public int PrescriptionRecordId { get; set; }
        public List<CartItemsDTO> CartItems { get; set; }
        public List<UserAddressBook> AddressList { get; set; }
        public List<OrderConfiguration> ConfigList { get; set; }
        public List<CardDTO> CardList { get; set; }
        public Card Cards { get; set; }
    }

    public class CartComputation
    {
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
    }

    public class CheckoutResponse
    {
        public NoVatOrder Order { get; set; }
        public List<NewOrderItems> OrderItems { get; set; }
        public PayInCardDirectDTO PaymentData { get; set; }
    }


    public class CardPayinCheckoutResponse
    {
        public NoVatOrder Order { get; set; }
        public List<NewOrderItems> OrderItems { get; set; }
        public PayInCardDirectDTO PaymentData { get; set; }
    }

    public class NewOrderItems
    {
        public int ProductRecordId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        [Column(TypeName = "decimal(4,1)")]
        public decimal ProductQuantity { get; set; }
        [Column(TypeName = "decimal(4,1)")]
        public decimal ProductPrice { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxValue { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxAmount { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal NoVatSubTotal { get; set; }
        public decimal SubTotal { get; set; }
        public string ProductIcon { get; set; }
    }
    public class CartSyncForHealthProfessional
    {
        public bool Sync { get; set; }
        public Guid ShopId { get; set; }
        public Guid UserId { get; set; }
        public Guid PatientId { get; set; }
        public int PrescriptionRecordId { get; set; }
        public List<CartItemsDTO> CartItemsSync { get; set; }
    }
    public class UserCartItemForHealthProfessional
    {
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopStatus { get; set; }
        public int PrescriptionRecordId { get; set; }
        public int PatientRecordId { get; set; }
        public Guid UserId { get; set; }
        public Guid PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ProductRecordId { get; set; }
        public string ProductName { get; set; }
        [Column(TypeName = "decimal(4,1)")]
        public decimal ProductQuantity { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductPrice { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxValue { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxAmount { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal TotalAmount { get; set; }
        public string ProductIcon { get; set; }
        public string ShopIcon { get; set; }
        public decimal SalePrice { get; set; }
    }
}