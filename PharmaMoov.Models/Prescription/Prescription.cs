using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Prescription
{
    public class Prescription : APIBaseModel
    {
		[Key]
		public int PrescriptionRecordId { get; set; }
		public Guid PrescriptionId { get; set; }
		public Guid UserId { get; set; }
		public Guid ShopId { get; set; }
		public Guid PatientId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string MedicineDescription { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string DoctorName { get; set; }
		
		[Required(ErrorMessage = "Ce champs est requis.")]
		public string PrescriptionIcon { get; set; }

		public PrescriptionStatus PrescriptionStatus { get; set; }
	}
	public class PrescriptionProduct : APIBaseModel
    {
		[Key]
		public int PrescriptionProductRecordId { get; set; }
		public int PrescriptionRecordId { get; set; }
		public int ProductRecordId { get; set; }

		[Column(TypeName = "decimal(4,1)")]
		public decimal ProductQuantity { get; set; }

		[Column(TypeName = "decimal(16,2)")]
		public decimal ProductPrice { get; set; }

		[Column(TypeName = "decimal(16,2)")]
		public decimal ProductTaxValue { get; set; }

		[Column(TypeName = "decimal(16,2)")]
		public decimal ProductTaxAmount { get; set; }
		public string ProductUnit { get; set; }

		[Column(TypeName = "decimal(16,2)")]
		public decimal SubTotal { get; set; }
		public PrescriptionProductStatus PrescriptionProductStatus { get; set; }
	}
}
