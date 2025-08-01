using System;
using System.ComponentModel.DataAnnotations;

namespace PharmaMoov.Models.Review
{
    public class ShopReviewRating : APIBaseModel
    {
        [Key]
        public int ShopReviewID { get; set; }
        public Guid ShopId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ShopReview { get; set; }
        public int ShopRating { get; set; }
    }

    public class ShopReviewRatingDTO
    {
        public int ShopReviewID { get; set; }

        [Required(ErrorMessage = "This is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "This is required")]
        public Guid ShopId { get; set; }

        public string ShopReview { get; set; }

        [Required(ErrorMessage = "This is required")]
        public int ShopRating { get; set; }

        public DateTime ReviewDate { get; set; }
    }

    public class ShopRating
    {
        public Guid ShopId { get; set; }
        public int TotalRates { get; set; }
        public int TotalVotes { get; set; }
        public decimal FinalRates { get; set; }
    }

    public class ShopReviewList
    {
        public int ShopReviewID { get; set; }
        public string CustomerName { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public DateTime ReviewDate { get; set; }
    }

    public class ShopFavorite
    {
        public Guid ShopId { get; set; }
        public bool IsFavorite { get; set; }
    }
}
