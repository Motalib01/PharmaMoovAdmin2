using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.Promo;
using PharmaMoov.Models.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PharmaMoov.Admin.Controllers
{
    public class FAQsController : BaseController
    {
        private IMainHttpClient FAQsHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; } 
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public FAQsController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor)
        {
            FAQsHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor; 
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        public IActionResult Index()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    ViewBag.Current = "FAQs";

                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (FAQsHttpClient.GetHttpClientRequest("FAQs/GetFAQsList/0"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<ShopFAQdto> returnList = JsonConvert.DeserializeObject<List<ShopFAQdto>>(returnRes.Payload.ToString());
                        return View(returnList);
                    }
                    else
                    {
                        return View(new List<ShopFAQdto>());
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

        public IActionResult Add() 
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
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
        
        public IActionResult EditFAQ(int id) 
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(FAQsHttpClient.GetHttpClientRequest("FAQs/GetFAQsList/" + id.ToString()));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ShopFAQdto ReturnedFAQ = JsonConvert.DeserializeObject<ShopFAQdto>(returnRes.Payload.ToString());
                        return View(ReturnedFAQ);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Promo";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new ShopFAQdto());
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
        public IActionResult EditFAQ(ShopFAQdto _shopFAQ)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(FAQsHttpClient.PostHttpClientRequest("FAQs/EditFAQuestion", _shopFAQ));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ShopFAQdto returnShop = JsonConvert.DeserializeObject<ShopFAQdto>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "FAQ";
                        ViewBag.ModalMessage = "FAQ mise à jour";
                        return View(returnShop);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "FAQ";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new ShopFAQdto());
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
        public IActionResult Add(ShopFAQdto _shopFAQ)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(FAQsHttpClient.PostHttpClientRequest("FAQs/AddFAQuestion", _shopFAQ));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ShopFAQdto returnShop = JsonConvert.DeserializeObject<ShopFAQdto>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "FAQ";
                        ViewBag.ModalMessage = "FAQ créée";
                        return View(returnShop);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "FAQ";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new PromoDTO());
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

        public IActionResult Terms()
        {
            if (AdminUCtxt != null)
            {
                ViewBag.Current = "Terms";
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (FAQsHttpClient.GetAnnonHttpClientRequest("FAQs/GetTermsAndConditions"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TermsAndCondition ReturnData = new TermsAndCondition();
                        if (returnRes.Payload != null) 
                        {
                            ReturnData = JsonConvert.DeserializeObject<TermsAndCondition>(returnRes.Payload.ToString());
                        } 
                        return View(ReturnData);
                    }
                    else
                    {
                        return View(new TermsAndCondition());
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

        public IActionResult ShopTerms()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (FAQsHttpClient.GetAnnonHttpClientRequest("FAQs/ShopGetTermsAndConditions"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ShopTermsAndCondition ReturnData = new ShopTermsAndCondition();
                        if (returnRes.Payload != null)
                        {
                            ReturnData = JsonConvert.DeserializeObject<ShopTermsAndCondition>(returnRes.Payload.ToString());
                        }
                        return View(ReturnData);
                    }
                    else
                    {
                        return View(new ShopTermsAndCondition());
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
        public IActionResult Terms(TermsAndCondition _termsAndCondition)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                        APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>
                        (FAQsHttpClient.PostHttpClientRequest("FAQs/SaveTermsAndConditions", _termsAndCondition));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TermsAndCondition returnShop = JsonConvert.DeserializeObject<TermsAndCondition>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Termes et conditions ";
                        ViewBag.ModalMessage = "Termes et conditions établie!";
                        return View(returnShop);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "FAQ";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new TermsAndCondition());
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
        public IActionResult ShopTerms(ShopTermsAndCondition _termsAndCondition)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>
                    (FAQsHttpClient.PostHttpClientRequest("FAQs/ShopSaveTermsAndConditions", _termsAndCondition));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ShopTermsAndCondition returnShop = JsonConvert.DeserializeObject<ShopTermsAndCondition>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Shop Termes et conditions ";
                        ViewBag.ModalMessage = "Shop Termes et conditions établie!";
                        return View(returnShop);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Shop Termes et conditions ";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new ShopTermsAndCondition());
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

        public IActionResult Privacy()
        {
            if (AdminUCtxt != null)
            {
                ViewBag.Current = "Privacy";
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (FAQsHttpClient.GetAnnonHttpClientRequest("FAQs/GetPrivacyPolicy"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        PrivacyPolicy ReturnData = new PrivacyPolicy();
                        if (returnRes.Payload != null)
                        {
                            ReturnData = JsonConvert.DeserializeObject<PrivacyPolicy>(returnRes.Payload.ToString());
                        }
                        return View(ReturnData);
                    }
                    else
                    {
                        return View(new PrivacyPolicy());
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
        public IActionResult Privacy(PrivacyPolicy _privacyPolicy)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>
                    (FAQsHttpClient.PostHttpClientRequest("FAQs/SavePrivacyPolicy", _privacyPolicy));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        PrivacyPolicy returnShop = JsonConvert.DeserializeObject<PrivacyPolicy>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Politique de confidentialité";
                        ViewBag.ModalMessage = "Politique de confidentialité établie!";
                        return View(returnShop);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Politique de confidentialité";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new PrivacyPolicy());
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
        public IActionResult ChangeFAQStatus([FromBody] ChangeRecordStatus _admin)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            _admin.AdminId = AdminID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(FAQsHttpClient.PostHttpClientRequest("FAQs/ChangeFAQStatus", _admin));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
            return Json(returnRes);
        }

        // public web page view for FAQ
        public IActionResult FAQList() 
        {
            APIResponse returnRes = JsonConvert
                .DeserializeObject<APIResponse>
                (FAQsHttpClient.GetAnnonHttpClientRequest("FAQs/GetFAQs/"));

            if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<ShopFAQdto> returnList = JsonConvert.DeserializeObject<List<ShopFAQdto>>(returnRes.Payload.ToString());
                return View(returnList);
            }
            else
            {
                return View(new List<ShopFAQdto>());
            }
        }

        // public web page view for Terms and conditions
        public IActionResult TermsAndConditions() 
        {
            APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (FAQsHttpClient.GetAnnonHttpClientRequest("FAQs/GetTermsAndConditions"));

            if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TermsAndCondition ReturnData = new TermsAndCondition();
                if (returnRes.Payload != null)
                {
                    ReturnData = JsonConvert.DeserializeObject<TermsAndCondition>(returnRes.Payload.ToString());
                }
                return View(ReturnData);
            }
            else
            {
                return View(new TermsAndCondition());
            }
        }

        // public web page view for Terms and conditions
        public IActionResult ShopTermsAndConditions()
        {
            APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (FAQsHttpClient.GetAnnonHttpClientRequest("FAQs/GetTermsAndConditions"));

            if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TermsAndCondition ReturnData = new TermsAndCondition();
                if (returnRes.Payload != null)
                {
                    ReturnData = JsonConvert.DeserializeObject<TermsAndCondition>(returnRes.Payload.ToString());
                }
                return View(ReturnData);
            }
            else
            {
                return View(new TermsAndCondition());
            }
        }

        // public web page view for privacy policy
        public IActionResult PrivacyPolicy()
        {
            APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (FAQsHttpClient.GetAnnonHttpClientRequest("FAQs/GetPrivacyPolicy"));

            if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                PrivacyPolicy ReturnData = new PrivacyPolicy();
                if (returnRes.Payload != null)
                {
                    ReturnData = JsonConvert.DeserializeObject<PrivacyPolicy>(returnRes.Payload.ToString());
                }
                return View(ReturnData);
            }
            else
            {
                return View(new PrivacyPolicy());
            }
        }
    }
}
