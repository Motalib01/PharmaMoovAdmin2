using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.User
{
    public class UserContext
    {
        public User UserInfo { get; set; }
        public UserLoginTransaction Tokens { get; set; }
    }

    public class FullUserRegForm
    {
        [Required(ErrorMessage = "Prénom requis.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Nom de famille requis.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Nom d'utilisateur requis.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email requis.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email invalide. Veuillez réessayer.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Numéro de téléphone requis.")]
        [MaxLength(10, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres")]
        [RegularExpression(@"^0[67][0-9]{8}$", ErrorMessage = "Numéro de téléphone invalide")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "You must enter a valid mobile number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Mot de passe requis.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{5,}$",
            ErrorMessage = "Le mot de passe doit contenir au moins 5-20 caractères contenant uniquement des lettres et des chiffres.")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Mot de passe requis.")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Le mot de passe ne correspond pas.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "You must specify an account type Regular user/Customer = 0, HEALTHPROFESSIONAL = 1, COURIER = 2")]
        public AccountTypes AccountType { get; set; }

        //[Required(ErrorMessage = "You must specify a registration Device Platform Web = 0, iOS = 1, Android = 2, Others = 3")]
        public DevicePlatforms DevicePlatform { get; set; }

        [Required(ErrorMessage = "This is required")]
        public bool AcceptedTermsAndConditions { get; set; }

        public DeliveryMethod MethodDelivery { get; set; }

        // Alpha Numeric, input by  Health Professional for user type 
        // for user type 1 must be present and not null
        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public string ProfessionalID { get; set; } 
        
        // Alpha Numeric, input by Delivery Man/ Driver
        // for user type 2 must be present and not null
        public string KBIS { get; set; }
        
        // Alpha Numeric, input by Delivery Man/ Driver
        // for user type 2 must be present and not null
        public string UserFieldID { get; set; }

        // Numeric, input by regular customer 
        // for user type 0 must be present and not null
        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public string HealthNumber { get; set; }

    }

    public class UserRegistrationViaEmail
    {
        [Required(ErrorMessage = "Prénom requis.")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Email requis.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email invalide. Veuillez réessayer.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mot de passe requis.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{5,}$",
            ErrorMessage = "Le mot de passe doit contenir au moins 5-20 caractères contenant uniquement des lettres et des chiffres.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "You must specify an account type: appuser = 0, admin = 1, shop = 2")]
        public AccountTypes AccountType { get; set; }

        [Required(ErrorMessage = "You must specify a registration Device Platform Web = 0, iOS = 1, Android = 2, Others = 3")]
        public DevicePlatforms DevicePlatform { get; set; }

        [Required(ErrorMessage = "This is required")]
        public bool AcceptedTermsAndConditions { get; set; }
    }

    public class VerifyMobileOrEmail
    {
        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Vous devez renseigner un numéro de téléphone valide")]
        [MinLength(10, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numéro de téléphone invalide")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "You must enter a valid mobile number")]
        public string MobileNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email invalide. Veuillez réessayer.")]
        public string Email { get; set; }

        public DevicePlatforms DevicePlatform { get; set; }
    }

    public class IsEmailOrMobileNumberValid
    {
        public bool IsEmailValid { get; set; }
        public bool IsMobileNumberValid { get; set; }

        public IsEmailOrMobileNumberValid()
        {
            IsEmailValid = false;
            IsMobileNumberValid = false;
        }
    }

    public class UserRegistrationViaMobileNumber
    {
        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Vous devez renseigner un numéro de téléphone valide")]
        [MinLength(10, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numéro de téléphone invalide")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "You must enter a valid mobile number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "You must specify a registration Device Platform Web = 0, iOS = 1, Android = 2, Others = 3")]
        public DevicePlatforms DevicePlatform { get; set; }
    }

    public class UserVerifyCode
    {
        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public VerificationTypes VerificationType { get; set; }

        public string VerificationCode { get; set; }

        public Guid? UserId { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public string Email { get; set; }

        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "You must specify a registration Device Platform Web = 0, iOS = 1, Android = 2, Others = 3")]
        public DevicePlatforms DevicePlatform { get; set; }
    }

    public class LoginCredentials
    {
        [Required(ErrorMessage = "Email requis.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email invalide. Veuillez réessayer.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mot de passe requis.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Nom d'utilisateur requis.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must specify a registration Device Platform Web = 0, IOS = 1, Android = 2, Others = 3")]
        public DevicePlatforms Device { get; set; }

        public AccountTypes AccountType { get; set; }
    }

    public class LoginEmailUsername
    {
        [Required(ErrorMessage = "Mot de passe requis.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email requis.")]
        public string LoginName { get; set; }

        //[Required(ErrorMessage = "You must specify a registration Device Platform Web = 0, IOS = 1, Android = 2, Others = 3")]
        public DevicePlatforms Device { get; set; }
    }

    public class LogoutCredentials
    {
        public Guid UserId { get; set; }
        public DevicePlatforms DevicePlatform { get; set; }
        public string DeviceFCMToken { get; set; }
        public string AuthToken { get; set; }
    }

    public class UserChangePassword
    {
        [Required(ErrorMessage = "UserId requis.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Mot de passe actuel requis.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Nouveau mot de passe requis.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{5,}$",
            ErrorMessage = "Le mot de passe doit contenir au moins 5-20 caractères contenant uniquement des lettres et des chiffres.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et la confirmation de mot de passe ne correspondent pas")]
        public string ConfirmPassword { get; set; }

        public UserTypes UserTypeId { get; set; }
    }

    public class UserProfile
    {
        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email requis.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email invalide. Veuillez réessayer.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vous devez renseigner un numéro de téléphone valide")]
        [MinLength(10, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numéro de téléphone invalide")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "You must enter a valid mobile number")]
        public string MobileNumber { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "You must specify a registration Device Platform Web = 0, IOS = 1, Android = 2, Others = 3")]
        public DevicePlatforms Device { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public string Username { get; set; }

        public string HealthNumber { get; set; }
        public string ProfessionalID { get; set; }
    }

    public class UserResetPassword
    {
        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Mot de passe requis.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$",
            ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères contenant uniquement des lettres et des chiffres.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "You must specify a registration Device Platform Web = 0, iOS = 1, Android = 2, Others = 3")]
        public DevicePlatforms RegistrationPlatform { get; set; }
    }

    public class UserAddressBook
    {
        public int UserAddressID { get; set; }
        public string AddressName { get; set; }
        public string CompleteAddress { get; set; }
        public bool IsCurrentAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string PostalCode { get; set; }
    }

    public class UserAddress
    {
        public int UserAddressID { get; set; }

        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public string AddressName { get; set; }

        [Required(ErrorMessage = "Ceci est nécessaire.")]
        public string Street { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string Building { get; set; }

        public string AddressNote { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public bool IsCurrentAddress { get; set; }

        public bool? IsEnabled { get; set; }
        public string PostalCode { get; set; }
    }

    public class UserAddressToDel
    {
        public int UserAddressID { get; set; }
        public Guid UserId { get; set; }
    }
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Ce champs est requis.")]
        public string VerificationCode { get; set; }

        [Required(ErrorMessage = "Nouveau mot de passe requis.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{5,}$",
          ErrorMessage = "Le mot de passe doit contenir au moins 5-20 caractères contenant uniquement des lettres et des chiffres.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et la confirmation de mot de passe ne correspondent pas")]
        public string ConfirmPassword { get; set; }
    }

    public class MangoPayUsers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; }
        public string Nationality { get; set; }
        public string CountryOfResidence { get; set; }
        public string Email { get; set; }
        public string Capacity { get; set; }
        public string Tag { get; set; }
        public MangoPayUsersAddress Address { get; set; }
    }

    public class MangoPayUsersAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
