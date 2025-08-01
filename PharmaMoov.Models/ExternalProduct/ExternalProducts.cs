using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace PharmaMoov.Models.ExternalProduct
{
	public class ExternalProduct
	{
		[Key]
		public int ProductRecordId { get; set; }
		public Guid ProductId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public int ProductCategoryId { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string ProductName { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string ProductDesc { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		public string ProductIcon { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		[RegularExpression("^[0-9]{1,11}(?:.[0-9]{1,3})?$", ErrorMessage = "Ce champs ne prend en compte que des valeurs numériques")]
		[Column(TypeName = "decimal(16,2)")]
		public decimal ProductPrice { get; set; }

		public string ProductUnit { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		[RegularExpression("^[0-9]{1,11}(?:.[0-9]{1,3})?$", ErrorMessage = "Ce champs ne prend en compte que des valeurs numériques")]
		[Column(TypeName = "decimal(16,2)")]
		public decimal? ProductPricePerKG { get; set; }

		[Required(ErrorMessage = "Ce champs est requis.")]
		[RegularExpression("^[0-9]{1,11}(?:.[0-9]{1,3})?$", ErrorMessage = "Ce champs ne prend en compte que des valeurs numériques")]
		[Column(TypeName = "decimal(16,2)")]
		public decimal ProductTaxValue { get; set; }

		public string ProductSKUCode { get; set; }
		public string ProductSAPCode { get; set; }

		public bool IsSale { get; set; }
		public bool IsUnsold { get; set; }
		public bool IsFragile { get; set; }

		public ProductStatus ProductStatus { get; set; }

		public bool IsProductFeature { get; set; }

		[Column(TypeName = "decimal(16,2)")]
		public decimal SalePrice { get; set; }

		[NotMapped]
		public decimal PriceHolder { get; set; }
		[NotMapped]
		public bool ExistsInDatabase { get; set; }
	}
	public class AddProductRequest
	{
		public Guid Shop { get; set; }
		public int ProductRecordId { get; set; }
	}
}
