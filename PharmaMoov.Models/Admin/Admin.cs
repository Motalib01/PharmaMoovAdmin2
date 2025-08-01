using System;
using System.ComponentModel.DataAnnotations;

namespace PharmaMoov.Models.Admin
{
    public class Admin : APIBaseModel
    {
        [Key]
        public int AdminRecordID { get; set; }
        public Guid AdminId { get; set; }
        public Guid ShopId { get; set; }
        public UserTypes UserTypeId { get; set; }
        public AccountTypes AccountType { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsVerified { get; set; }
    }

    public class UserType : APIBaseModel
    {
        [Key]
        public int UserTypeID { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
    }
}
