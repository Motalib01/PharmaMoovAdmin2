using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.Orders;
using PharmaMoov.Models.User;
using System;
using System.Collections.Generic;

namespace PharmaMoov.Admin.Controllers
{
    public class SAdminController : BaseController
    {
        private IMainHttpClient SAdminHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public SAdminController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            SAdminHttpClient = _mhttpc;
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
                PharmaMoov.Models.Admin.Admin loginAccount = new PharmaMoov.Models.Admin.Admin
                {
                    Email = _adminModel.Email,
                    Password = _adminModel.Password,
                    UserTypeId = UserTypes.SUPERADMIN
                    //AccountType = AccountTypes.SUPERADMINUSER
                };

                string returnResult = SAdminHttpClient.PostHttpClientRequest("Admin/AdminLogin", loginAccount);

                try
                {
                    APIResponse ApiResp = JsonConvert.DeserializeObject<APIResponse>(returnResult);

                    if (ApiResp != null)
                    {
                        if (ApiResp.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            AdminUCtxt = JsonConvert.DeserializeObject<AdminUserContext>(ApiResp.Payload.ToString());

                            if (AdminUCtxt != null)
                            {
                                _session.Remove("adminUserContext");
                            }
                            HttpContext.Session.SetObjectAsJson("adminUserContext", AdminUCtxt);

                            return RedirectToAction("Dashboard", "SAdmin");
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
            if (AdminUCtxt != null && AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
            {
                ViewBag.Current = "Dashboard";
                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(SAdminHttpClient.GetHttpClientRequest("Dashboard/GetSuperAdminDashboard"));

                // If token is invalid, redirect to login
                if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SuperAdminDashboardModel returnList = JsonConvert.DeserializeObject<SuperAdminDashboardModel>(returnRes.Payload.ToString());
                    return View(returnList);
                }
                else
                {
                    return View(new SuperAdminDashboardModel());
                }
            }
            else 
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpGet]
        public IActionResult Configs()
        {
            if (AdminUCtxt != null)
            {
                ViewBag.Current = "Configs";
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                { 
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(SAdminHttpClient.GetHttpClientRequest("Config/GetAllConfigurations"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<OrderConfiguration> returnList = JsonConvert.DeserializeObject<List<OrderConfiguration>>(returnRes.Payload.ToString());
                        return View(returnList);
                    }
                    else 
                    { 
                        return View(new List<OrderConfiguration>());
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
        public IActionResult Configs(List<OrderConfiguration> _configs)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    foreach (OrderConfiguration singleConf in _configs) 
                    {
                        singleConf.LastEditedBy = AdminUCtxt.AdminInfo.AdminId;
                    }
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(SAdminHttpClient.PostHttpClientRequest("Config/UpdateOrderConfig", _configs));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<OrderConfiguration> returnList = JsonConvert.DeserializeObject<List<OrderConfiguration>>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Paramétrage des configurations";
                        ViewBag.ModalMessage = "Import avec succès";
                        return View(returnList);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Paramétrage des configurations";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new List<OrderConfiguration>());
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
        public IActionResult HelpRequest()
        {
            if (AdminUCtxt != null)
            {
                ViewBag.Current = "HelpRequest";
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(SAdminHttpClient.GetHttpClientRequest("UserHelpRequest/GetUserRequestList/0"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<UserHelpRequestView> returnList = JsonConvert.DeserializeObject<List<UserHelpRequestView>>(returnRes.Payload.ToString());
                        return View(returnList);
                    }
                    else
                    {
                        return View(new List<UserHelpRequestView>());
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
        public IActionResult HelpRequestView(int id)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(SAdminHttpClient.GetHttpClientRequest("UserHelpRequest/GetUserRequestList/" + id.ToString()));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        UserHelpRequestView returnData = JsonConvert.DeserializeObject<UserHelpRequestView>(returnRes.Payload.ToString());
                        return View(returnData);
                    }
                    else
                    {
                        return View(new UserHelpRequestView());
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
        public IActionResult GeneralConcern()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(SAdminHttpClient.GetHttpClientRequest("UserHelpRequest/GetUserGeneralConcernList/0"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<UserGeneralConcernView> returnList = JsonConvert.DeserializeObject<List<UserGeneralConcernView>>(returnRes.Payload.ToString());
                        return View(returnList);
                    }
                    else
                    {
                        return View(new List<UserGeneralConcernView>());
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
        public IActionResult GeneralConcernView(int id)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(SAdminHttpClient.GetHttpClientRequest("UserHelpRequest/GetUserGeneralConcernList/" + id.ToString()));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        UserGeneralConcernView returnData = JsonConvert.DeserializeObject<UserGeneralConcernView>(returnRes.Payload.ToString());
                        return View(returnData);
                    }
                    else
                    {
                        return View(new UserGeneralConcernView());
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

        public IActionResult Payment()
        {
            ViewBag.Current = "Payment";
            return View();
        }

        [HttpPost]
        public IActionResult ChangeHelpRequestStatus([FromBody] ChangeHelpRequestStatus _request)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            _request.AdminId = AdminID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(SAdminHttpClient.PostHttpClientRequest("UserHelpRequest/ChangeHelpRequestStatus", _request));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
            return Json(returnRes);
        }
    }
}
