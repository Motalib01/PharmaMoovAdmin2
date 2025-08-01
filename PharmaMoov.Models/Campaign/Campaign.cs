using System;
using System.ComponentModel.DataAnnotations;

namespace PharmaMoov.Models.Campaign
{
    public class Campaign : APIBaseModel
    {
        [Key]
        public int CampaignRecordID { get; set; }
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "IsProductOfferBanner cannot be null")]
        public bool IsProductOfferBanner { get; set; }
    }
}
