using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PharmaMoov.Models.User
{
    public class UserHelpRequest : APIBaseModel
    {
        [Key]
        public int UserHelpRequestRecordID { get; set; }
        public Guid UserId { get; set; }
        public int OrderId { get; set; } // Commande #
        public string Description { get; set; }
        public HelpRequestStatus RequestStatus { get; set; }
    }

    public class UserHelpRequestView 
    {
        public int UserHelpRequestRecordID { get; set; }
        public Guid UserId { get; set; }
        public int OrderId { get; set; } // Commande #
        public string Description { get; set; }
        public string UserFullName { get; set; }
        public string UserMobileNumber { get; set; }
        public string UserEmail { get; set; }
        public DateTime? DateCreated { get; set; }
        public HelpRequestStatus RequestStatus { get; set; }
    }

    public class OrderNumber
    {
        public int OrderId { get; set; }
        public string OrderReferenceID { get; set; }
    }

    public class NewUserHelpRequest
    {
        [Required(ErrorMessage = "Ce champs est requis.")]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Ce champs est requis.")]
        public string Description { get; set; }
    }

    public class UserGeneralConcern : APIBaseModel
    {
        [Key]
        public int UserConcernRecordID { get; set; }
        public Guid UserId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
    }

    public class UserGeneralConcernView
    {
        public int UserConcernRecordID { get; set; }
        public Guid UserId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string UserFullName { get; set; }
        public string UserMobileNumber { get; set; }
        public string UserEmail { get; set; }
        public DateTime? DateCreated { get; set; }
    }

    public class NewUserGeneralConcern
    {
        [Required(ErrorMessage = "This is required.")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "This is required.")]
        public string Description { get; set; }
    }

    public class ChangeHelpRequestStatus
    {
        public int RecordId { get; set; }
        public Guid AdminId { get; set; }
        public HelpRequestStatus HelpRequestStatus { get; set; }
    }

    public class GeneralInquiry
    {
        [Required(ErrorMessage = "This is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This is required.")]
        public string InquiryMessage { get; set; }
    }

    public class ReCaptchaValidationResult
    {
        public bool Success { get; set; }
        public string HostName { get; set; }
        [JsonProperty("challenge_ts")]
        public string TimeStamp { get; set; }
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }

    public class CareersForm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public string CoverLetter { get; set; }
        public IFormFile AttachmentFile { get; set; }
    }
}
