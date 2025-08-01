using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Promo
{
    public class Promo : APIBaseModel
    {
        [Key]
        public int PromoRecordID { get; set; }
        public string PromoName { get; set; }
        public string PromoCode { get; set; }
        public string PromoDescription { get; set; }
        public PromoType PType { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal PromoValue { get; set; }
        public string ValidityPeriod { get; set; }
        public DateTime ValidityDate { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }

    public class PromoDTO
    {
        public int PromoRecordID { get; set; }
        public string PromoName { get; set; }
        public string PromoCode { get; set; }
        public string PromoDescription { get; set; }
        public decimal PromoValue { get; set; }
        public PromoType PType { get; set; }
        public string ValidityPeriod { get; set; }
        public DateTime ValidityDate { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
