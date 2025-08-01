using PharmaMoov.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PharmaMoov.Models.Admin
{
    public class AdminUserContext
    {
        public Admin AdminInfo { get; set; }
        public UserLoginTransaction Tokens { get; set; }
    }

    public class AdminProfile
    {
        public Guid AdminId { get; set; }
        public Guid ShopId { get; set; }

        [Required(ErrorMessage = "Prénom requis.")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AdminIcon { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{5,}$",
            ErrorMessage = "Le mot de passe doit contenir au moins 5-20 caractères contenant uniquement des lettres et des chiffres.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation de mot de passe ne correspondent pas")]
        public string ConfirmPassword { get; set; }

        public string ImageUrl { get; set; }
        public UserTypes UserTypeId { get; set; }
        public AccountTypes AccountType { get; set; }
        public bool? IsEnabled { get; set; }
    }
    public class EditAdminProfile
    {
        public Guid AdminId { get; set; }
        public Guid ShopId { get; set; }

        [Required(ErrorMessage = "Prénom requis.")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AdminIcon { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{5,}$",
            ErrorMessage = "Le mot de passe doit contenir au moins 5-20 caractères contenant uniquement des lettres et des chiffres.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation de mot de passe ne correspondent pas")]
        public string ConfirmPassword { get; set; }

        public string ImageUrl { get; set; }
        public UserTypes UserTypeId { get; set; }
        public AccountTypes AccountType { get; set; }
        public bool? IsEnabled { get; set; }
    }
    public class AdminLogin
    {
        [Required(ErrorMessage = "Addresse e-mail requis.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mot de passe requis.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{5,}$",
            ErrorMessage = "Le mot de passe doit contenir au moins 5-20 caractères contenant uniquement des lettres et des chiffres.")]
        public string Password { get; set; }

        public AccountTypes AccountType { get; set; }
    }

    public class AdminList
    {
        public int AdminRecordID { get; set; }
        public Guid AdminId { get; set; }
        public Guid ShopId { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public UserTypes UserType { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ChangeRecordStatus
    {
        public int RecordId { get; set; }
        public Guid AdminId { get; set; }
        public bool IsActive { get; set; }
    }   

    public class AdminForgotPassword
    {
        [Required(ErrorMessage = "Addresse e-mail requis.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class DashboardParamModel
    {
        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }

        [Required]
        public int SalesReportType { get; set; }

        [Required]
        public Guid ShopId { get; set; }
        public List<int> ProductRecordIdList { get; set; }
    }
    public class PharmacyAdminDashboardModel
    {
        public int CompletedOrder { get; set; }
        public int RefusedOrder { get; set; }
        public int TotalOrder { get; set; }
        public decimal TotalSalesInThePeriod { get; set; }
        public decimal AvgSalesInThePeriod { get; set; }
        public int TotalOrderInThePeriod { get; set; }
        public decimal TotalPurchaseInThePeriod { get; set; }
    }
    public class SuperAdminDashboardModel
    {
        public int TotalUser { get; set; }
        public int TotalShop { get; set; }
        public int TotalPendingRequest { get; set; }
        public int TotalOrder { get; set; }
    }
    public class ChangeAcceptOrDeclineRequestStatus
    {
        public int RecordId { get; set; }
        public Guid AdminId { get; set; }
        public bool IsLocked { get; set; }
    }
}
