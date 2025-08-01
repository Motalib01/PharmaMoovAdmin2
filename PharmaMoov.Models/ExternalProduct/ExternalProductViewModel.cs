using System;
using System.Collections.Generic;
using System.Text;

namespace PharmaMoov.Models.ExternalProduct
{
	public class ExternalProductList
	{
		public Guid ProductId { get; set; }
		public int ProductRecordId { get; set; }
		public string ProductIcon { get; set; }
		public string ProductName { get; set; }
		public decimal ProductPrice { get; set; }
		public decimal SalePrice { get; set; }
		public int ProductCategoryId { get; set; }
		public string ProductCategory { get; set; }
		public ProductStatus ProductStatus { get; set; }
		public bool ExistsInDatabase { get; set; }
	}
}
