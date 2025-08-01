
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PharmaMoov.Models.Admin;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using Microsoft.AspNetCore.Http;
using PharmaMoov.Models;
using Newtonsoft.Json;
using PharmaMoov.Models.Shop;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.Net;
using System.Net.Http;
using Microsoft.Net.Http.Headers;
using PharmaMoov.Models.User;

namespace PharmaMoov.Admin.Controllers
{
    public class ShopController : BaseController
    {
        private IMainHttpClient ShopHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public ShopController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            ShopHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AdminLogin _adminModel)
        {
            if (ModelState.IsValid)
            {
                AdminLogin loginAccount = new AdminLogin
                {
                    Email = _adminModel.Email,
                    Password = _adminModel.Password,
                    //AccountType = AccountTypes.SHOPADMINUSER
                };

                string returnResult = ShopHttpClient.PostHttpClientRequest("Admin/AdminLogin", loginAccount);

                try
                {
                    APIResponse ApiResp = JsonConvert.DeserializeObject<APIResponse>(returnResult);

                    if (ApiResp != null)
                    {
                        if (ApiResp.StatusCode == HttpStatusCode.OK)
                        {
                            AdminUCtxt = JsonConvert.DeserializeObject<AdminUserContext>(ApiResp.Payload.ToString());

                            if (AdminUCtxt != null)
                            {
                                _session.Remove("adminUserContext");
                            }

                            HttpContext.Session.SetObjectAsJson("adminUserContext", AdminUCtxt);

                            if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.ORDERPICKER)
                            {
                                return RedirectToAction("Index", "Order");
                            }
                            else
                            {
                                return RedirectToAction("Dashboard", "Shop");
                            }
                        }

                        else if (ApiResp.StatusCode == System.Net.HttpStatusCode.Redirect)
                        {
                            AdminUCtxt = JsonConvert.DeserializeObject<AdminUserContext>(ApiResp.Payload.ToString());
                            HttpContext.Session.SetObjectAsJson("adminUserContext", AdminUCtxt);

                            return RedirectToAction("ChangePassword", "Home");
                        }

                        else
                        {
                            TempData["ErrorMessage"] = ApiResp.Status + " : " + ApiResp.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    ModelState.AddModelError("Email", errmsg);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Identifiants invalides";
            }

            return View(_adminModel);
        }

        public IActionResult Dashboard()
        {
            if (AdminUCtxt != null && AdminUCtxt.AdminInfo.UserTypeId != UserTypes.SUPERADMIN)
            {
                ViewBag.Current = "Dashboard";
                DashboardParamModel dashboardParam = new DashboardParamModel
                {
                    DateFrom = DateTime.Now.AddMonths(-2),
                    DateTo = DateTime.Now,
                    ShopId = AdminUCtxt.AdminInfo.ShopId,
                    SalesReportType = 1
                };

                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.PostHttpClientRequest("Dashboard/GetPharmacyAdminDashboard", dashboardParam));

                // If token is invalid, redirect to login
                if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                if (returnRes.StatusCode == HttpStatusCode.OK)
                {
                    PharmacyAdminDashboardModel returnList = JsonConvert.DeserializeObject<PharmacyAdminDashboardModel>(returnRes.Payload.ToString());
                    return View(returnList);
                }
                else
                {
                    return View(new SuperAdminDashboardModel());
                }
            }
            else
            {
                return RedirectToAction("Index", "Shop"); //Shop Login
            }
        }

        public IActionResult EditProfile()
        {
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;

            var responseData = ShopHttpClient.GetHttpClientRequest("Shop/GetShopProfile/" + ShopID);
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(responseData);
            ShopProfile editShop = JsonConvert.DeserializeObject<ShopProfile>(returnRes.Payload.ToString());

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            ViewBag.ShopCategories = JsonConvert.DeserializeObject<List<ShopCategoriesDTO>>(JsonConvert.DeserializeObject<APIResponse>
                (ShopHttpClient.GetHttpClientRequest("Categories/PopulateShopCategories/0/1")).Payload.ToString());

            ViewBag.Current = "Profile";

            return View(editShop);
        }

        [HttpPost]
        public IActionResult EditProfile(Shop _shop)
        {
            if (ModelState.IsValid)
            {
                Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
                Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
                _shop.ShopId = ShopID;
                _shop.CreatedBy = AdminID;
                _shop.IsEnabledBy = AdminID;

                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.PostHttpClientRequest("Shop/EditShopProfile", _shop));

                // If token is invalid, redirect to login
                if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                ViewBag.ShopCategories = JsonConvert.DeserializeObject<List<ShopCategoriesDTO>>(JsonConvert.DeserializeObject<APIResponse>
                    (ShopHttpClient.GetHttpClientRequest("Categories/PopulateShopCategories/0/1")).Payload.ToString());

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

                ViewBag.Current = "Profile";
                return View();
            }
            else
            {
                ViewBag.Current = "Profile";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ShopHours()
        {
            DateTime startAM = DateTime.ParseExact("06:00", "HH:mm", null);
            DateTime endAM = DateTime.ParseExact("12:00", "HH:mm", null);

            DateTime startPM = DateTime.ParseExact("12:00", "HH:mm", null);
            DateTime endPM = DateTime.ParseExact("18:00", "HH:mm", null);

            //Night Hours
            DateTime startEve = DateTime.ParseExact("18:00", "HH:mm", null);
            DateTime endEve = DateTime.ParseExact("23:59", "HH:mm", null);

            int interval = 30;
            Dictionary<TimeSpan, TimeSpan> timeSlotsAM = new Dictionary<TimeSpan, TimeSpan>();
            Dictionary<TimeSpan, TimeSpan> timeSlotsPM = new Dictionary<TimeSpan, TimeSpan>();
            Dictionary<TimeSpan, TimeSpan> timeSlotsEve = new Dictionary<TimeSpan, TimeSpan>();

            for (DateTime i = startAM; i <= endAM; i = i.AddMinutes(interval))
            {
                TimeSpan timespan = new TimeSpan(i.Hour, i.Minute, 00);
                timeSlotsAM.Add(timespan, timespan);
            }

            for (DateTime i = startPM; i <= endPM; i = i.AddMinutes(interval))
            {
                TimeSpan timespan = new TimeSpan(i.Hour, i.Minute, 00);
                timeSlotsPM.Add(timespan, timespan);
            }

            //Night Hours
            for (DateTime i = startEve; i <= endEve; i = i.AddMinutes(interval))
            {
                TimeSpan timespan = new TimeSpan(i.Hour, i.Minute, 00);
                timeSlotsEve.Add(timespan, timespan);
            }

            ViewBag.TimeSlotsAM = timeSlotsAM;
            ViewBag.TimeSlotsPM = timeSlotsPM;
            ViewBag.TimeSlotsEve = timeSlotsEve;

            Guid shopId = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.GetHttpClientRequest("Shop/ShopOpeningHours/" + shopId));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            IEnumerable<ShopOpeningHourDTO> returnList = JsonConvert.DeserializeObject<IEnumerable<ShopOpeningHourDTO>>(returnRes.Payload.ToString());

            ViewBag.Current = "ShopHours";
            return View(returnList);
        }

        [HttpPost]
        public IActionResult SetShopHours([FromBody] ShopHourList _shop)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
            _shop.AdminId = AdminID;
            _shop.ShopId = ShopID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.PostHttpClientRequest("Shop/EditShopOpeningHours", _shop));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            ViewBag.Current = "ShopHours";
            return Json(returnRes);
        }

        [HttpGet]
        public IActionResult PreparationTime()
        {
            Guid shopId = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.GetHttpClientRequest("Shop/GetShopProfile/" + shopId));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            Shop returnDetails = JsonConvert.DeserializeObject<Shop>(returnRes.Payload.ToString());

            ViewBag.Current = "PreparationTime";
            return View(returnDetails);
        }

        [HttpPost]
        public IActionResult SetPreparationTime([FromBody] ShopConfigs _shop)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
            _shop.AdminId = AdminID;
            _shop.ShopId = ShopID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.PostHttpClientRequest("Shop/SetShopConfigurations", _shop));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            ViewBag.Current = "PreparationTime";
            return Json(returnRes);
        }

        [HttpGet]
        public IActionResult DeliveryMethod()
        {
            Guid shopId = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.GetHttpClientRequest("Shop/GetShopProfile/" + shopId));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            Shop returnDetails = JsonConvert.DeserializeObject<Shop>(returnRes.Payload.ToString());

            ViewBag.Current = "DeliveryMethod";
            return View(returnDetails);
        }

        [HttpPost]
        public IActionResult SetDeliveryMethod([FromBody] ShopConfigs _shop)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
            _shop.AdminId = AdminID;
            _shop.ShopId = ShopID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.PostHttpClientRequest("Shop/SetShopConfigurations", _shop));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            ViewBag.Current = "DeliveryMethod";
            return Json(returnRes);
        }

        [HttpGet]
        public IActionResult Documents()
        {
            Guid shopId = AdminUCtxt.AdminInfo.ShopId;

            if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
            {
                shopId = Guid.Empty;
                ViewBag.IsSuperAdmin = true;
            }

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.GetHttpClientRequest("Shop/ShopDocuments/" + shopId));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            IEnumerable<ShopDocument> returnList = JsonConvert.DeserializeObject<IEnumerable<ShopDocument>>(returnRes.Payload.ToString());

            ViewBag.Current = "Documents";
            return View(returnList);
        }

        [HttpPost]
        public IActionResult AddShopDocument([FromBody] ShopDocumentDTO _shop)
        {
            _shop.ShopId = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.PostHttpClientRequest("Shop/AddShopDocument", _shop));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViewBag.Current = "Documents";
                return Json(returnRes);
            }

            ViewBag.Current = "Documents";
            return View();
        }

        [HttpPost]
        public IActionResult DeleteShopDocument([FromBody] int _shop)
        {
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.PostHttpClientRequest("Shop/DeleteShopDocument", _shop));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViewBag.Current = "Documents";
                return Json(returnRes);
            }

            ViewBag.Current = "Documents";
            return View();
        }

        private static HttpClient Client { get; } = new HttpClient();
        [HttpGet]
        public async Task<FileStreamResult> DownloadFile(string path, string name)
        {
            var stream = await Client.GetStreamAsync(path);

            return new FileStreamResult(stream, new MediaTypeHeaderValue("application/pdf"))
            {
                FileDownloadName = name,
                EnableRangeProcessing = true
            };
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewBag.Current = "Contact";
            return View();
        }

        [HttpPost]
        public IActionResult Contact(NewUserGeneralConcern _shop)
        {
            if (ModelState.IsValid)
            {
                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.PostHttpClientRequest("UserHelpRequest/AddUserGeneralConcern", _shop));

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
                }

                ViewBag.Current = "Contact";

                return View();
            }
            else
            {
                ViewBag.Current = "Contact";

                return View();
            }
        }

        [HttpGet]
        public IActionResult ShopCommisionInvoice() 
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    ShopCommissionDateRange scDR = new ShopCommissionDateRange {
                        dateFrom = DateTime.Now.AddMonths(-2),
                        dateTo = DateTime.Now
                    };

                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.PostHttpClientRequest("Shop/ShopCommisionInvoice", scDR));
                    if (returnRes.StatusCode == HttpStatusCode.OK && returnRes.Payload != null)
                    {
                        List<ShopComissionDTO> shopListing = JsonConvert.DeserializeObject<List<ShopComissionDTO>>(returnRes.Payload.ToString());
                        ViewBag.Current = "ShopCommisionInvoice";
                        ViewBag.shopList = shopListing;
                        return View(scDR);
                    }
                    else 
                    {
                        ViewBag.Current = "ShopCommisionInvoice";
                        return View(scDR);
                    }
                }
                else
                {
                    if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SHOPADMIN)
                    {
                        return RedirectToAction("Index", "Shop");
                    }
                    else
                    {
                        return RedirectToAction("Index", "SAdmin");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpPost]
        public IActionResult ShopCommisionInvoice([FromForm] ShopCommissionDateRange _range)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.PostHttpClientRequest("Shop/ShopCommisionInvoice", _range));
                    if (returnRes.StatusCode == HttpStatusCode.OK && returnRes.Payload != null)
                    {
                        List<ShopComissionDTO> shopListing = JsonConvert.DeserializeObject<List<ShopComissionDTO>>(returnRes.Payload.ToString());
                        ViewBag.Current = "ShopCommisionInvoice";
                        ViewBag.shopList = shopListing;
                        return View(_range);
                    }
                    else
                    {
                        ViewBag.Current = "ShopCommisionInvoice";
                        return View(_range);
                    }
                }
                else
                {
                    if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SHOPADMIN)
                    {
                        return RedirectToAction("Index", "Shop");
                    }
                    else
                    {
                        return RedirectToAction("Index", "SAdmin");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpGet]
        public IActionResult ComissionInvoiceView(int shopId, string dFrom,string dTo) 
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    SingleShopComissionInvoiceParameters printParams = new SingleShopComissionInvoiceParameters
                    {
                        DateFrom = DateTime.Parse(dFrom),
                        DateTo = DateTime.Parse(dTo),
                        ShopRecordId = shopId
                    };
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.PostHttpClientRequest("Shop/ComissionInvoiceView", printParams));

                    if (returnRes.StatusCode == HttpStatusCode.OK && returnRes.Payload != null)
                    {
                        SingleShopComissionInvoice returnObject = JsonConvert.DeserializeObject<SingleShopComissionInvoice>(returnRes.Payload.ToString());
                        ViewBag.Current = "ShopCommisionInvoice";
                        return View(returnObject);
                    }
                    else
                    {
                        ViewBag.Current = "ShopCommisionInvoice";
                        return View(new SingleShopComissionInvoice());
                    }
                }
                else
                {
                    if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SHOPADMIN)
                    {
                        return RedirectToAction("Index", "Shop");
                    }
                    else
                    {
                        return RedirectToAction("Index", "SAdmin");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpGet]
        public IActionResult GetOrdersNotification()
        {
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ShopHttpClient.GetHttpClientRequest("Dashboard/GetOrdersNotification/" + ShopID));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            return Json(new { data = returnRes.Payload });
        }
    }
}
