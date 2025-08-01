namespace PharmaMoov.Models
{
    public enum DeliveryMethod
    {
        NotApplicable = 0,
        GreenDelivery,
        NormalDelivery
    }

    public enum DevicePlatforms
    {
        Web = 0,
        IOS = 1,
        Android = 2,
        Others = 3
    }

    public enum Genders
    {
        Female = 0,
        Male = 1,
        Others = 2
    }

    public enum VerificationTypes
    {
        RegisterAccount = 0,
        ForgotPassword = 1,
        ChangePassword = 2,
        ChangeEmail = 3,
        ChangePhoneNumber = 4
    }

    public enum UploadTypes
    {
        UploadBanner = 0,
        UploadIcon = 1,
        UploadBackgroundImage = 2,
        UploadProfileImage = 3,
        UploadDocument = 4,
        UploadPrescription = 5
    }

    public enum ERedirectTo
    {
        HOME = 1,
        ABOUTUS,
        CONTACTUS,
        CART,
        CHECKOUT,
        ACCOUNT
    }

    public enum APIResponseCode
    {
        UserIsNotYetVerified,
        UserHasNoAddress
    }

    public enum OrderProgressStatus
    {
        PENDING = 0, //Not used
        PLACED, 
        INPROGRESS,
        READYFORDELIVERY, //Set by Pharmacy admin 
        OUTFORDELIVERY, //Set by Delivery Man
        COMPLETED, //DELIVERED
        CANCELORDER,
        REJECTED, //REFUSED
        READYFORPICKUP,
        ACCEPTED
    }

    public enum OrderDeliveryType
    {
        ALL = 0,
        FORPICKUP = 1,
        FORDELIVERY
    }

    public enum OrderPaymentType
    {
        ONLINEPAYMENT = 1,
        CASHONDELIVERY,
        CARDONDELIVERY
    }

    public enum OrderPaymentStatus
    {
        PENDING = 0,
        PAID,
        CANCELLED,
        ERROR,
        REFUND
    }

    public enum OrderConfigType
    {
        MAXORDER = 1,
        MINORDER, //Pharma Min. amount order
        DELIVERYFEE,
        CASHPAYMENT,
        CARDPAYMENT,
        MAXDELIVERYDISTANCE,
        MAXPICKUPDISTANCE,
        COMMISSIONRATE, //Pharma Commission Rate in Percent
        NORMALDELIVERYFEEINPARIS,
        NORMALDELIVERYFEEOUTPARIS,
        GREENDELIVERYINPARIS,
        GREENDELIVERYOUTPARIS
    }

    public enum AccountTypes
    {
        APPUSER = 0, // customer(regular User) 0
        HEALTHPROFESSIONAL, // health prof 1
        COURIER // driver or deliveryman 2
    }

    public enum DeliveryPackageType
    {
        SMALL = 0,
        MEDIUM,
        LARGE
    }

    public enum ProductStatus
    {
        ACTIVE = 1,
        INACTIVE,
        OUTOFSTOCK
    }

    public enum UserTypes
    {
        SUPERADMIN = 1, // Super Admin 1
        SHOPADMIN, // Pharmacy Admin 2
        STAFF, // Staff 3
        ORDERPICKER, // Orderpicker 4
        OPERATIONMANAGER
    }

    public enum PromoType 
    {
        FIXED=1,
        PERCENTAGE
    }

    public enum RegistrationStatus
    {
        PENDING = 1,
        APPROVE,
        DECLINE
    }

    public enum HelpRequestStatus
    {
        PENDING = 1,
        RESOLVED
    }

    public enum ProductSortEnum
    {
        PriceHighToLow = 1,
        PriceLowToHigh,
        AscendingProduct,
        DescendingProduct
    }

    public enum PrescriptionStatus
    {
        PENDING = 1,
        APPROVED
    }
    public enum SalesReportTypeEnum
    {
        SalesByDate = 1,
        SalesByProduct
    }
    public enum PharmacySortEnum
    {        
        Ascending = 1,
        Descending
    }
    public enum DeliveryStatus
    {
        PENDING = 1,
        ACCEPT,
        CANCELLED,
        DELIVERED
    }
    public enum PrescriptionProductStatus
    {
        New = 1,
        Completed,
        Cancelled
    }
}
