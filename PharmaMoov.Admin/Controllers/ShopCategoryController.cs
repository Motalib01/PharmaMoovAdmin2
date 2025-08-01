using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.Product;
using PharmaMoov.Models.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PharmaMoov.Admin.Controllers
{
    public class ShopCategoryController : BaseController
    {
        private IMainHttpClient ShopCategoryHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public ShopCategoryController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            ShopCategoryHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopCategoryHttpClient.GetHttpClientRequest("Categories/PopulateShopCategories/0/0"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    IEnumerable<ShopCategoriesDTO> returnList = JsonConvert.DeserializeObject<IEnumerable<ShopCategoriesDTO>>(returnRes.Payload.ToString());

                    ViewBag.Current = "ShopCategory";

                    return View(returnList);
                }
                else
                {
                    return RedirectToAction("Index", "SAdmin");
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpGet]
        public IActionResult AddShopCategory()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    ViewBag.DefaultIcon = MConf.WebApiBaseUrl.Replace("api/", "") + "resources/Banners/Shop_Category_Default_Logo.jpg";

                    ViewBag.Current = "ShopCategory";

                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "SAdmin");
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpPost]
        public IActionResult AddShopCategory(ShopCategoriesDTO _category)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    if (ModelState.IsValid)
                    {
                        APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopCategoryHttpClient.PostHttpClientRequest("Categories/AddShopCategory", _category));

                        // If token is invalid, redirect to login
                        if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                        ViewBag.ModalTitle = returnRes.Message;
                        if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ViewBag.ShowModal = "true";
                        }
                        else
                        {
                            ViewBag.ShowModal = "false";
                            ViewBag.DefaultIcon = _category.ImageUrl;
                        }

                        ViewBag.Current = "ShopCategory";

                        return View();
                    }
                    else
                    {
                        return Json(new APIResponse { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid Object!", ModelError = ModelState.Errors() });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "SAdmin");
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpGet]
        public IActionResult EditShopCategory(int cid)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopCategoryHttpClient.GetHttpClientRequest("Categories/PopulateShopCategories/" + cid + "/0"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    ShopCategoriesDTO returnDetails = JsonConvert.DeserializeObject<ShopCategoriesDTO>(returnRes.Payload.ToString());

                    ViewBag.Current = "ShopCategory";

                    return View(returnDetails);
                }
                else
                {
                    return RedirectToAction("Index", "SAdmin");
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpPost]
        public IActionResult EditShopCategory(ShopCategoriesDTO _category)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    if (ModelState.IsValid)
                    {
                        APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopCategoryHttpClient.PostHttpClientRequest("Categories/EditShopCategory", _category));

                        // If token is invalid, redirect to login
                        if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                        ViewBag.ModalTitle = returnRes.Message;
                        if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ViewBag.ShowModal = "true";
                        }
                        else
                        {
                            ViewBag.ShowModal = "false";
                            ViewBag.DefaultIcon = _category.ImageUrl;
                        }

                        ViewBag.Current = "ShopCategory";

                        return View();
                    }
                    else
                    {
                        return Json(new APIResponse { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid Object!", ModelError = ModelState.Errors() });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "SAdmin");
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpPost]
        public IActionResult ChangeShopCategoryStatus([FromBody] ChangeRecordStatus _category)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            _category.AdminId = AdminID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopCategoryHttpClient.PostHttpClientRequest("Categories/ChangeShopCategoryStatus", _category));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
            return Json(returnRes);
        }
    }
}
