using System;

namespace PharmaMoov.Models.Campaign
{
    public class CampaignDTO
    {
        public int CampaignRecordID { get; set; }
        public Guid? ShopId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string RelatedShopName { get; set; }
        public bool IsProductOfferBanner { get; set; }
    }
}
