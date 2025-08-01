using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.Campaign;
using PharmaMoov.Models.Orders;
using PharmaMoov.Models.Promo;
using PharmaMoov.Models.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PharmaMoov.Admin.Controllers
{
    public class PromoController : BaseController
    {
        private IMainHttpClient PromoHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public PromoController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            PromoHttpClient = _mhttpc;
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
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (PromoHttpClient.GetHttpClientRequest("Promo/GetPromoList/0"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<PromoDTO> returnList = JsonConvert.DeserializeObject<List<PromoDTO>>(returnRes.Payload.ToString());
                        return View(returnList);
                    }
                    else
                    {
                        return View(new List<PromoDTO>());
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
        public IActionResult AddPromo()
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

        [HttpPost]
        public IActionResult AddPromo(PromoDTO _promo)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                { 
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PromoHttpClient.PostHttpClientRequest("Promo/AddPromo", _promo));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        PromoDTO ReturnedPromo = JsonConvert.DeserializeObject<PromoDTO>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Promo";
                        ViewBag.ModalMessage = "Promo créée";
                        return View(ReturnedPromo);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Promo";
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

        [HttpGet]
        public IActionResult EditPromo(int id) 
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PromoHttpClient.GetHttpClientRequest("Promo/GetPromoList/" + id.ToString()));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        PromoDTO ReturnedPromo = JsonConvert.DeserializeObject<PromoDTO>(returnRes.Payload.ToString());
                        return View(ReturnedPromo);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Promo";
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

        [HttpPost]
        public IActionResult EditPromo(PromoDTO _promo)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PromoHttpClient.PostHttpClientRequest("Promo/EditPromo/", _promo));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        PromoDTO ReturnedPromo = JsonConvert.DeserializeObject<PromoDTO>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Promo";
                        ViewBag.ModalMessage = "Promo Updated!";
                        return View(ReturnedPromo);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Promo";
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

        [HttpGet]
        public IActionResult AddBanner(Guid cId)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PromoHttpClient.GetHttpClientRequest("Shop/ShopList")); 
                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
                    IEnumerable<ShopList> returnShopList = JsonConvert.DeserializeObject<IEnumerable<ShopList>>(returnRes.Payload.ToString());
                    ViewBag.ShopList = returnShopList;

                    Campaign aCampaign = new Campaign(); 
                    if (cId.ToString() != "00000000-0000-0000-0000-000000000000") 
                    {
                        APIResponse respResult = JsonConvert.DeserializeObject<APIResponse>(PromoHttpClient.GetHttpClientRequest("Promo/GetCampaignByShopId/"+cId.ToString()));
                        if (respResult.Payload != null)
                        {
                            aCampaign = JsonConvert.DeserializeObject<Campaign>(respResult.Payload.ToString());
                        }
                        else 
                        {
                            aCampaign.ShopId = cId;
                            aCampaign.Name = "Banner For " + returnShopList.Where(c => c.ShopId == cId).FirstOrDefault().Name;
                            aCampaign.Description = "Banner Description For " + returnShopList.Where(c => c.ShopId == cId).FirstOrDefault().Name;
                        }
                    }
                    return View(aCampaign);
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
        public IActionResult AddBanner(Campaign campaign)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                   
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PromoHttpClient.GetHttpClientRequest("Shop/ShopList"));
                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    IEnumerable<ShopList> returnShopList = JsonConvert.DeserializeObject<IEnumerable<ShopList>>(returnRes.Payload.ToString());
                    ViewBag.ShopList = returnShopList;

                    if (string.IsNullOrEmpty(campaign.ImageUrl))
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Banner";
                        ViewBag.ModalMessage = "Please upload banner";
                        return View(new Campaign());
                    }

                    APIResponse newCampaignReturn = JsonConvert.DeserializeObject<APIResponse>(PromoHttpClient.PostHttpClientRequest("Promo/AddBanner", campaign));

                    if (newCampaignReturn.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Campaign returnCamp = JsonConvert.DeserializeObject<Campaign>(newCampaignReturn.Payload.ToString());
                        
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Banner";
                        ViewBag.ModalMessage = newCampaignReturn.Message;
                        return View(returnCamp);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Banner";
                        ViewBag.ModalMessage = newCampaignReturn.Message;
                        ViewBag.ErrMsg = newCampaignReturn.Message;
                        return View(new Campaign());
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
        public IActionResult EditBanner(int id)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse newCampaignReturn = JsonConvert.DeserializeObject<APIResponse>(PromoHttpClient.GetHttpClientRequest("Promo/GetCampaignByBannerId/" + id.ToString()));
                    
                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(newCampaignReturn, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (newCampaignReturn.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //Populate Pharmacy Dropdownlist
                        APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PromoHttpClient.GetHttpClientRequest("Shop/ShopList"));

                        IEnumerable<ShopList> returnShopList = JsonConvert.DeserializeObject<IEnumerable<ShopList>>(returnRes.Payload.ToString());
                        ViewBag.ShopList = returnShopList;

                        Campaign returnCamp = JsonConvert.DeserializeObject<Campaign>(newCampaignReturn.Payload.ToString());
                        return View(returnCamp);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Banner";
                        ViewBag.ModalMessage = newCampaignReturn.Message;
                        ViewBag.ErrMsg = newCampaignReturn.Message;
                        return View(new Campaign());
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
        public IActionResult ChangePromoStatus([FromBody] ChangeRecordStatus _admin)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            _admin.AdminId = AdminID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PromoHttpClient.PostHttpClientRequest("Customer/ChangeCustomerStatus", _admin));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
            return Json(returnRes);
        }

        [HttpGet]
        public IActionResult ShopBanners()
        {
            if (AdminUCtxt != null)
            {
                ViewBag.Current = "Banner";
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (PromoHttpClient.GetHttpClientRequest("Promo/GetShopBanners"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<CampaignDTO> returnList = JsonConvert.DeserializeObject<List<CampaignDTO>>(returnRes.Payload.ToString());
                        return View(returnList);
                    }
                    else
                    {
                        return View(new List<CampaignDTO>());
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
    }
}

