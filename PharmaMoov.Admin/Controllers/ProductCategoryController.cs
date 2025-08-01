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

namespace PharmaMoov.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private IMainHttpClient ProdCategoryHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public ProductCategoryController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            ProdCategoryHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        public IActionResult Index()
        {
            Guid shopId = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ProdCategoryHttpClient.GetHttpClientRequest("Categories/PopulateProductCategories/" + shopId + "/0/0"));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            IEnumerable<ProductCategoriesDTO> returnList = JsonConvert.DeserializeObject<IEnumerable<ProductCategoriesDTO>>(returnRes.Payload.ToString());

            ViewBag.Current = "ProductCategory";
            ViewBag.DefaultIcon = MConf.WebApiBaseUrl.Replace("api/", "") + "resources/Banners/Product_Default_Logo.jpg";

            return View(returnList);
        }

        [HttpPost]
        public IActionResult AddProductCategory([FromBody] ProductCategoriesDTO _category)
        {
            _category.ShopId = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ProdCategoryHttpClient.PostHttpClientRequest("Categories/AddProductCategory", _category));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            return Json(returnRes);
            //if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    return Json(returnRes);
            //}

            //return View();
        }

        [HttpPost]
        public IActionResult EditProductCategory([FromBody] ProductCategoriesDTO _category)
        {
            _category.ShopId = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ProdCategoryHttpClient.PostHttpClientRequest("Categories/EditProductCategory", _category));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            return Json(returnRes);
        }
    }
}
