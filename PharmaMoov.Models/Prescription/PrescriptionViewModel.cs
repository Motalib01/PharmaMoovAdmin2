using PharmaMoov.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Prescription
{
    public class PrescriptionDetail
    {
		public int PrescriptionRecordId { get; set; }
		public Guid ShopId { get; set; }
		public Guid UserId { get; set; }
		public Guid PatientId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string MedicineDescription { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string DoctorName { get; set; }

		public string CustomerName { get; set; }
		public string CustomerEmail { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string PrescriptionIcon { get; set; }
		public DateTime DateCreated { get; set; }
		public bool IsActive { get; set; }
		public PrescriptionStatus PrescriptionStatus { get; set; }
	}

	public class PrescriptionDetailAndProducts
	{
		public PrescriptionDetail PrescriptionDetail { get; set; }
		public List<ProductList> PrescriptionProducts { get; set; }
	}
	public class PrescriptionProductsParamModel
    {	
		//[Required(ErrorMessage = "Ce champs est requis.")]
		public int PrescriptionRecordId { get; set; }
		public int ProductRecordId { get; set; }
		public decimal ProductCustomPrice { get; set; }
		public decimal ProductQuantity { get; set; }
		//public List<PrescriptionProductsListModel> prescriptionList { get; set; }
	}
	public class PrescriptionProductsListModel
    {
		[Required(ErrorMessage = "Ce champs est requis.")]
		public int ProductRecordId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public decimal ProductQuantity { get; set; }
	}
	public class InvoicePrescriptionListModel
    {
		public int PrescriptionRecordId { get; set; }		
		public PrescriptionStatus PrescriptionStatus { get; set; }			
		public DateTime DateAdded { get; set; }		
		public Guid ShopId { get; set; }
		public string ShopName { get; set; }
		public string ShopIcon { get; set; }
	}
	public class PrescriptionProductListParamModel
    {
		[Required(ErrorMessage = "Ce champs est requis.")]
		public int PrescriptionRecordId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public Guid ShopId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public Guid UserId { get; set; }
	}
	//public class PrescriptionProductListModel
	//{
	//	public Guid ShopId { get; set; }
	//	public string ShopName { get; set; }
	//	public string ShopAddress { get; set; }
	//	public string ShopStatus { get; set; }
	//	public int ProductRecordId { get; set; }
	//	public int PrescriptionRecordId { get; set; }
	//	public string ProductName { get; set; }
	//	[Column(TypeName = "decimal(4,1)")]
	//	public decimal ProductQuantity { get; set; }
	//	[Column(TypeName = "decimal(16,2)")]
	//	public decimal ProductPrice { get; set; }
	//	[Column(TypeName = "decimal(16,2)")]
	//	public decimal ProductTaxValue { get; set; }
	//	[Column(TypeName = "decimal(16,2)")]
	//	public decimal ProductTaxAmount { get; set; }
	//	[Column(TypeName = "decimal(16,2)")]
	//	public decimal TotalAmount { get; set; }
	//	public string ProductIcon { get; set; }
	//	public string ShopIcon { get; set; }
	//	public decimal SalePrice { get; set; }
	//}
	public class InvoiceParamModel
    {
		[Required(ErrorMessage = "Ce champs est requis.")]
		public int PrescriptionRecordId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public Guid UserId { get; set; }
	}
	public class PrescriptionParamModel
	{
		[Required(ErrorMessage = "Ce champs est requis.")]
		public Guid ShopId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public Guid UserId { get; set; }
	}

	public class PrescriptionList
	{
		public int PrescriptionRecordId { get; set; }
		public Guid ShopId { get; set; }
		public Guid UserId { get; set; }
		public Guid PatientId { get; set; }
		public string MedicineDescription { get; set; }
		public string DoctorName { get; set; }
		public string PrescriptionIcon { get; set; }
		public string ShopIcon { get; set; }
		public string ShopName { get; set; }
		public DateTime? CreatedDate { get; set; }
		public PrescriptionStatus PrescriptionStatus { get; set; }
	}
}
