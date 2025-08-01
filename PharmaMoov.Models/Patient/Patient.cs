using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Patient
{
    public class Patient : APIBaseModel
	{
		[Key]
		public int PatientRecordId { get; set; }
		public Guid PatientId { get; set; }
		public Guid UserId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string MobileNumber { get; set; }
		public string AddressName { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string Street { get; set; }
		public string POBox { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string City { get; set; }
		public string AddressNote { get; set; }

		[Column(TypeName = "decimal(10,8)")]
		public decimal Latitude { get; set; }
		[Column(TypeName = "decimal(11,8)")]
		public decimal Longitude { get; set; }
	}
}
