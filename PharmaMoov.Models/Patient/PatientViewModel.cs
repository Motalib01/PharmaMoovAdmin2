using PharmaMoov.Models.Orders;
using PharmaMoov.Models.Prescription;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Patient
{
    public class PatientDTO
    {
		public int PatientRecordId { get; set; }
		public Guid PatientId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		[MinLength(10, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres")]
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Numéro de téléphone invalide")]
		[DataType(DataType.PhoneNumber, ErrorMessage = "You must enter a valid mobile number")]
		public string MobileNumber { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string AddressName { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string Street { get; set; }
		public string POBox { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string City { get; set; }
		public string AddressNote { get; set; }

		public decimal Latitude { get; set; }

		public decimal Longitude { get; set; }
		public bool IsActive { get; set; }

		[NotMapped]
		public string Address { get; set; }
	}
	public class PatientProfileModel
    {
		[Required(ErrorMessage = "Ce champs est requis.")]
		public int PatientRecordId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		[MinLength(10, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres")]
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Numéro de téléphone invalide")]
		[DataType(DataType.PhoneNumber, ErrorMessage = "You must enter a valid mobile number")]
		public string MobileNumber { get; set; }
		public bool IsActive { get; set; }
	}
	public class PatientAddressModel
    {
		[Required(ErrorMessage = "Ce champs est requis.")]
		public int PatientRecordId { get; set; }

		public string AddressName { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string Street { get; set; }
		public string POBox { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string City { get; set; }
		public string AddressNote { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public decimal Latitude { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public decimal Longitude { get; set; }
	}
	public class FilterPatientModel
    {
        public int _pageNo { get; set; }
		public string _searchKey { get; set; }
	}
	public class PatientListModel
    {
		public int PatientRecordId { get; set; }
		public Guid PatientId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsEnabled { get; set; }

		public string CompleteAddress { get; set; }
		public string MobileNumber { get; set; }
	}

	public class PatientListResult
	{
		public List<PatientListModel> PatientList { get; set; }
		public int PageCount { get; set; }
	}

	public class PatientDetailsResult
	{
		public PatientDTO PatientDetail { get; set; }
		public List<UserOrderList> PatientOrderList { get; set; }
		public List<PrescriptionList> PrescriptionList { get; set; }
	}
}
