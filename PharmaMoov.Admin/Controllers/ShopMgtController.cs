using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.Product;
using PharmaMoov.Models.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PharmaMoov.Admin.Controllers
{
    public class ShopMgtController : BaseController
    {
        private IMainHttpClient ShopMgtHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public ShopMgtController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            ShopMgtHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        public IActionResult Index()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopMgtHttpClient.GetHttpClientRequest("Shop/ShopList"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    IEnumerable<ShopList> returnList = JsonConvert.DeserializeObject<IEnumerable<ShopList>>(returnRes.Payload.ToString());

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.Current = "ShopMgt";
                        return View(returnList);
                    }

                    ViewBag.Current = "ShopMgt";
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "SAdmin");
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddShop()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopMgtHttpClient.GetHttpClientRequest("Categories/PopulateShopCategories/0/1"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    ViewBag.ShopCategories = JsonConvert.DeserializeObject<List<ShopCategoriesDTO>>(returnRes.Payload.ToString());
                    ViewBag.DefaultIcon = MConf.WebApiBaseUrl.Replace("api/", "") + "resources/Banners/Shop_Default_Logo.jpg";
                    ViewBag.Current = "ShopMgt";

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
        public IActionResult AddShop(ShopProfile _shop)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    ViewBag.ShopCategories = JsonConvert.DeserializeObject<List<ShopCategoriesDTO>>(JsonConvert.DeserializeObject<APIResponse>(ShopMgtHttpClient.GetHttpClientRequest("Categories/PopulateShopCategories/0/1")).Payload.ToString());

                    if (ModelState.IsValid)
                    {
                        APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopMgtHttpClient.PostHttpClientRequest("Shop/RegisterShop", _shop));

                        // If token is invalid, redirect to login
                        if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                        ViewBag.ModalMessage = returnRes.Message;
                        if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ViewBag.ShowModal = "true";
                            ViewBag.ModalTitle = "Pharmacie créée avec succès";
                        }
                        else
                        {
                            ViewBag.ShowModal = "false";
                            ViewBag.DefaultIcon = _shop.ShopIcon;
                            ViewBag.ModalTitle = "Pharmacie Registration Failed";
                        }

                        ViewBag.Current = "ShopMgt";
                        return View();
                    }
                    else
                    {
                        ViewBag.Current = "ShopMgt";
                        return View();
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

        public IActionResult EditShop(Guid sid)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    var responseData = ShopMgtHttpClient.GetHttpClientRequest("Shop/GetShopProfile/" + sid);
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(responseData);
                    ShopProfile editShop = JsonConvert.DeserializeObject<ShopProfile>(returnRes.Payload.ToString());

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    ViewBag.ShopCategories = JsonConvert.DeserializeObject<List<ShopCategoriesDTO>>(JsonConvert.DeserializeObject<APIResponse>
                        (ShopMgtHttpClient.GetHttpClientRequest("Categories/PopulateShopCategories/0/1")).Payload.ToString());

                    return View(editShop);
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
        public IActionResult EditShop(Shop _shop)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    if (ModelState.IsValid)
                    {
                        Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
                        Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
                        _shop.ShopId = ShopID;
                        _shop.CreatedBy = AdminID;
                        _shop.IsEnabledBy = AdminID;

                        APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopMgtHttpClient.PostHttpClientRequest("Shop/EditShopProfile", _shop));

                        // If token is invalid, redirect to login
                        if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                        ViewBag.ShopCategories = JsonConvert.DeserializeObject<List<ShopCategoriesDTO>>(JsonConvert.DeserializeObject<APIResponse>
                            (ShopMgtHttpClient.GetHttpClientRequest("Categories/PopulateShopCategories/0/1")).Payload.ToString());

                        ViewBag.ModalTitle = returnRes.Message;
                        if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ViewBag.ShowModal = "true";
                        }
                        else
                        {
                            ViewBag.ShowModal = "false";
                            ViewBag.DefaultIcon = _shop.ShopIcon;
                        }

                        return View();
                    }
                    else
                    {
                        return View();
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
        public IActionResult ValidateShopAddress(string _shop, string _phone)
        {
            if (AdminUCtxt != null)
            {
                //APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopMgtHttpClient.GetHttpClientRequest("DeliveryJob/ValidateShopAddress/" + _shop + "/" + _phone));

                //// If token is invalid, redirect to login
                //if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                //AddressValidationModel result = JsonConvert.DeserializeObject<AddressValidationModel>(returnRes.Payload.ToString());

                return Json(true);
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpGet]
        public IActionResult Documents(Guid sid)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopMgtHttpClient.GetHttpClientRequest("Shop/ShopDocuments/" + sid));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    IEnumerable<ShopDocument> returnList = JsonConvert.DeserializeObject<IEnumerable<ShopDocument>>(returnRes.Payload.ToString());

                    ViewBag.Current = "ShopMgt";

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

        [HttpPost]
        public IActionResult ChangeShopStatus([FromBody] ChangeRecordStatus _shop)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            _shop.AdminId = AdminID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopMgtHttpClient.PostHttpClientRequest("Shop/ChangeShopStatus", _shop));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
            return Json(returnRes);
        }

        [HttpGet]
        public IActionResult ShopRequest()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopMgtHttpClient.GetHttpClientRequest("Shop/GetRequestList"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    IEnumerable<ShopList> returnList = JsonConvert.DeserializeObject<IEnumerable<ShopList>>(returnRes.Payload.ToString());

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.Current = "ShopRequest";
                        return View(returnList);
                    }

                    ViewBag.Current = "ShopRequest";
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

        #region "Pharmacy Request Registration"
        [HttpPost]
        public IActionResult ChangeRegistrationStatus([FromBody] ChangeRegistrationStatus _shop)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            _shop.AdminId = AdminID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopMgtHttpClient.PostHttpClientRequest("Shop/ChangeRegistrationStatus", _shop));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
            return Json(returnRes);
        }

        [HttpGet]
        public IActionResult ViewPharmacyRequest(Guid sid)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    var responseData = ShopMgtHttpClient.GetHttpClientRequest("Shop/GetShopProfile/" + sid);
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(responseData);
                    ShopProfile editShop = JsonConvert.DeserializeObject<ShopProfile>(returnRes.Payload.ToString());

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    ViewBag.ShopCategories = JsonConvert.DeserializeObject<List<ShopCategoriesDTO>>(JsonConvert.DeserializeObject<APIResponse>
                        (ShopMgtHttpClient.GetHttpClientRequest("Categories/PopulateShopCategories/0/1")).Payload.ToString());

                    return View(editShop);
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

            //var responseData = ShopMgtHttpClient.GetHttpClientRequest("Shop/GetShopRegistrationRequest/" + _shopId);
            //APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(responseData);
            //ShopRequest editShop = JsonConvert.DeserializeObject<ShopRequest>(returnRes.Payload.ToString());

            //// If token is invalid, redirect to login
            //if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            //return View(editShop);
        }
        #endregion

        [HttpPost]
        public IActionResult ChangePopularStatus([FromBody] ChangeRecordStatus _shop)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            _shop.AdminId = AdminID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopMgtHttpClient.PostHttpClientRequest("Shop/ChangePopularStatus", _shop));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
            return Json(returnRes);
        }
    }
}
