using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Cart
{
    public class CartItem : APIBaseModel
    {
        [Key]
        public int CartItemId { get; set; }
        public Guid ShopId { get; set; }
        public Guid UserId { get; set; }
        public Guid PatientId { get; set; }
        public int ProductRecordId { get; set; }
        [Column(TypeName = "decimal(4,1)")]
        public decimal ProductQuantity { get; set; }
        public int PrescriptionRecordId { get; set; }
    }
}