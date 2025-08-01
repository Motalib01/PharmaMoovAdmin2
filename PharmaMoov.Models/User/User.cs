using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.User
{
    public class User : APIBaseModel
    {
        [Key]
        public int UserRecordID { get; set; }
        public Guid UserId { get; set; }
        public AccountTypes AccountType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Genders Gender { get; set; }
        public string ImageUrl { get; set; }
        public DevicePlatforms RegistrationPlatform { get; set; }
        public string RegistrationCode { get; set; }
        public string ProfessionalID { get; set; }
        public string KBIS { get; set; }
        public string UserFieldID { get; set; }
        public string HealthNumber { get; set; }
        public DeliveryMethod MethodDelivery { get; set; }
        public string ForgotPasswordCode { get; set; }
        public bool IsDecline { get; set; }
    }

    public class UserDevice : APIBaseModel
    {
        [Key]
        public int UserDeviceRecordID { get; set; }
        public Guid UserId { get; set; }
        public string DeviceFCMToken { get; set; }
        public DevicePlatforms DeviceType { get; set; }
    }

    public class UserLoginTransaction
    {
        [Key]
        public int UserLoginRecordID { get; set; }
        public Guid UserId { get; set; }
        public AccountTypes AccountType { get; set; }
        public DevicePlatforms Device { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserVerificationCode : APIBaseModel
    {
        [Key]
        public int UserCodeRecordID { get; set; }
        public Guid UserId { get; set; }
        public DevicePlatforms Device { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public VerificationTypes VerificationType { get; set; }
        public string VerificationCode { get; set; }
    }

    public class UserAddresses : APIBaseModel
    {
        [Key]
        public int UserAddressID { get; set; }
        public Guid UserId { get; set; }
        public string AddressName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Building { get; set; }
        public string AddressNote { get; set; }
        [Column(TypeName = "decimal(10,8)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(11,8)")]
        public decimal Longitude { get; set; }
        public bool IsCurrentAddress { get; set; }

        [NotMapped]
        public string CompleteAddress { get; set; }
        public string PostalCode { get; set; }
    }
}
