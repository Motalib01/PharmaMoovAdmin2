using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaMoov.Models.Product
{
    public class ProductCategoriesDTO
    {
        public Guid ShopId { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductCategoryDesc { get; set; }
        public string ProductCategoryImage { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public bool IsCategoryFeatured { get; set; }
    }

    public class FilterProducts
    {
        public int PageNumber { get; set; }
        public Guid ShopId { get; set; }
        public int ProductCategoryId { get; set; }
        public string SearchKey { get; set; }
        public int SortBy { get; set; }
        public int PageSize { get; set; }
    }

    public class FilteredProducts
    {
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public int ProductRecordId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductPrice { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal? ProductPricePerKG { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxValue { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxAmount { get; set; }
        public string ProductIcon { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public decimal SalePrice { get; set; }
        public bool IsSale { get; set; }
        public bool IsUnsold { get; set; }
        public bool IsFragile { get; set; }
        public decimal PriceHolder { get; set; }
    }

    public class ProductDetails
    {
        public int ProductRecordId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductPrice { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal? ProductPricePerKG { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxValue { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal ProductTaxAmount { get; set; }
        public string ProductIcon { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string ProductDesc { get; set; }
        public decimal ProductRating { get; set; }
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public decimal SalePrice { get; set; }
        public IEnumerable<FilteredProducts> RecomProducts { get; set; }
        public bool IsProductFeature { get; set; }
        public bool IsFragile { get; set; }
        public int ProductCategoryId { get; set; }
    }

    public class ProductList
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
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopIcon { get; set; }
    }

    public class PrescriptionProductList
    {
        public Guid ProductId { get; set; }
        public string productRecordId { get; set; }
        public string productIcon { get; set; }
        public string productName { get; set; }
        public string productPrice { get; set; }
        public string customPrice { get; set; }
        public string quantity { get; set; }
        public string subTotal { get; set; }
        public string action { get; set; }

    }

    public class ChangeProdStatus
    {
        public Guid AdminId { get; set; }
        public int ProductRecordId { get; set; }
        public ProductStatus ProductStatus { get; set; }
    }
    public class ProductAndPharmacyModel
    {
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public List<ProductListModel> ProductListModel { get; set; }
    }
    public class ProductListModel
    {
        public Guid ProductId { get; set; }
        public Guid ShopId { get; set; }
        public int ProductRecordId { get; set; }
        public string ProductIcon { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int ProductCategoryId { get; set; }
        public bool IsActive { get; set; }
        public bool IsProductFeature { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public decimal PriceHolder { get; set; }
    }
    public class ImportProductParamModel
    {
        public Guid AdminId { get; set; }
        public Guid ShopId { get; set; }
        public IFormFile File { get; set; }       
    }
    public class ImportProductErrorModel
    {
        public string Row { get; set; }
        public string ProductName { get; set; }
        public string Error { get; set; }
    }
    public class ProductErrorModel
    {
        public ProductErrorModel()
        {
            errorModel = new List<ImportProductErrorModel>();
        }
        public int TotalProduct { get; set; }
        public List<ImportProductErrorModel> errorModel { get; set; }
    }


}
