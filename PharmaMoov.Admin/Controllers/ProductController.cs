using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace PharmaMoov.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private IMainHttpClient ProductHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public ProductController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            ProductHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        public IActionResult Index()
        {
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ProductHttpClient.GetHttpClientRequest("Product/GetAllProducts/" + ShopID + "/0"));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            IEnumerable<ProductList> returnList = JsonConvert.DeserializeObject<IEnumerable<ProductList>>(returnRes.Payload.ToString());

            ViewBag.Current = "Product";
            return View(returnList);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            Guid shopId = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ProductHttpClient.GetHttpClientRequest("Categories/PopulateProductCategories/" + shopId + "/0/1"));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            ViewBag.ProductCategories = JsonConvert.DeserializeObject<List<ProductCategoriesDTO>>(returnRes.Payload.ToString());
            ViewBag.DefaultIcon = MConf.WebApiBaseUrl.Replace("api/", "") + "resources/Banners/Product_Default_Logo.jpg";

            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product _product)
        {
            if (ModelState.IsValid)
            {
                Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
                Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
                _product.ShopId = ShopID;
                _product.CreatedBy = AdminID;
                _product.IsEnabledBy = AdminID;

                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ProductHttpClient.PostHttpClientRequest("Product/AddProduct", _product));

                // If token is invalid, redirect to login
                if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                ViewBag.ProductCategories = JsonConvert.DeserializeObject<List<ProductCategoriesDTO>>(JsonConvert.DeserializeObject<APIResponse>
                    (ProductHttpClient.GetHttpClientRequest("Categories/PopulateProductCategories/" + ShopID + "/0/1")).Payload.ToString());

                ViewBag.ModalTitle = returnRes.Message;
                if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewBag.ShowModal = "true";
                }
                else
                {
                    ViewBag.ShowModal = "false";
                    ViewBag.DefaultIcon = _product.ProductIcon;
                }

                return View();
            }
            else
            {
                return Json(new APIResponse { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid Object!", ModelError = ModelState.Errors() });
            }
        }

        [HttpGet]
        public IActionResult EditProduct(int pid)
        {
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;

            var responseData = ProductHttpClient.GetHttpClientRequest("Product/GetAllProducts/" + ShopID + "/" + pid);
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(responseData);
            Product returnDetails = JsonConvert.DeserializeObject<Product>(returnRes.Payload.ToString());

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            ViewBag.SuperAdmin = (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN) ? "True" : "False";
            ViewBag.ShopId = returnDetails.ShopId;
            ViewBag.ProductCategories = JsonConvert.DeserializeObject<List<ProductCategoriesDTO>>(JsonConvert.DeserializeObject<APIResponse>
                (ProductHttpClient.GetHttpClientRequest("Categories/PopulateProductCategories/" + ShopID + "/0/1")).Payload.ToString());

            return View(returnDetails);
        }

        [HttpPost]
        public IActionResult EditProduct(Product _product)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ShopId = _product.ShopId;
                Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
                Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
                _product.ShopId = ShopID;
                _product.CreatedBy = AdminID;
                _product.IsEnabledBy = AdminID;

                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ProductHttpClient.PostHttpClientRequest("Product/EditProduct", _product));

                // If token is invalid, redirect to login
                if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                ViewBag.ProductCategories = JsonConvert.DeserializeObject<List<ProductCategoriesDTO>>(JsonConvert.DeserializeObject<APIResponse>
                    (ProductHttpClient.GetHttpClientRequest("Categories/PopulateProductCategories/" + ShopID + "/0/1")).Payload.ToString());
                
                
                ViewBag.SuperAdmin = (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN) ? "True" : "False";
                ViewBag.ModalTitle = returnRes.Message;
                if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewBag.ShowModal = "true";                   
                }
                else
                {
                    ViewBag.ShowModal = "false";
                    ViewBag.DefaultIcon = _product.ProductIcon;
                }

                return View();
            }
            else
            {
                return Json(new APIResponse { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid Object!", ModelError = ModelState.Errors() });
            }
        }

        [HttpPost]
        public IActionResult ChangeProductStatus([FromBody] ChangeProdStatus _product)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            _product.AdminId = AdminID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ProductHttpClient.PostHttpClientRequest("Product/ChangeProductStatus", _product));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            return Json(returnRes);
        }

        public IActionResult ShopProducts(Guid sid)
        {
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ProductHttpClient.GetHttpClientRequest("Product/GetAllProducts/" + sid + "/0"));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            IEnumerable<ProductList> returnList = JsonConvert.DeserializeObject<IEnumerable<ProductList>>(returnRes.Payload.ToString());

            ViewBag.Current = "Product";
            return View(returnList);
        }

        public IActionResult ImportProducts()
        {          
            ViewBag.Percentage = 0;
            return View();
        }

        [HttpPost]
        public IActionResult ImportProducts([FromForm] ImportProductParamModel _import)
        {
            if (ModelState.IsValid)
            {
                if (_import.File == null)
                {
                    ViewBag.ModalTitle = "Please upload file";
                    ViewBag.ShowModal = "false";
                    return View();
                }

                _import.AdminId = AdminUCtxt.AdminInfo.AdminId;
                _import.ShopId = AdminUCtxt.AdminInfo.ShopId;
               
                byte[] data;
                using (var br = new BinaryReader(_import.File.OpenReadStream()))
                {
                    data = br.ReadBytes((int)_import.File.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                MultipartFormDataContent multipartFormData = new MultipartFormDataContent();
                multipartFormData.Add(bytes, "file", _import.File.FileName);
                multipartFormData.Add(new StringContent(_import.AdminId.ToString()), "AdminId");
                multipartFormData.Add(new StringContent(_import.ShopId.ToString()), "ShopId");

                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ProductHttpClient.PostFileHttpCientRequest("Product/ImportProduct", multipartFormData));

                // If token is invalid, redirect to login
                if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                ViewBag.ModalTitle = returnRes.Message;
                if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var returnList = JsonConvert.DeserializeObject<ProductErrorModel>(returnRes.Payload.ToString());
                    if(returnList != null)
                    {
                        ViewBag.TotalProduct = returnList.TotalProduct;
                       
                        if(returnList.errorModel.Count == 0) //All product imported successfully
                        {
                            ViewBag.Percentage = 100;
                            ViewBag.ErrorProduct = returnList.TotalProduct;
                            ViewBag.ShowModal = "true";
                        }
                        else
                        {
                            if(returnList.TotalProduct == returnList.errorModel.Count) // No product imported 
                            {
                                ViewBag.Percentage = 0;
                                ViewBag.ErrorProduct = 0;
                            }
                            else // Partially product imported 
                            {
                                decimal calculation = (Convert.ToDecimal(returnList.errorModel.Count) / Convert.ToDecimal(returnList.TotalProduct)) * 100;
                                ViewBag.Percentage = Math.Ceiling(calculation);
                                ViewBag.ErrorProduct = returnList.errorModel.Count;
                            }
                            ViewBag.ShowModal = "false";
                        }                        
                        ViewBag.ProductErrorList = returnList.errorModel;
                        
                    }
                    else
                    {
                        ViewBag.TotalProduct = 0;
                        ViewBag.ErrorProduct = 0;
                        ViewBag.Percentage = 0;
                        ViewBag.ShowModal = "false";
                    }                    
                }
                else
                {
                    ViewBag.TotalProduct = 0;
                    ViewBag.ErrorProduct = 0;
                    ViewBag.Percentage = 0;
                    ViewBag.ShowModal = "false";
                }

                return View();
            }
            else
            {
                return Json(new APIResponse { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid Object!", ModelError = ModelState.Errors() });
            }
        }
    }
}
