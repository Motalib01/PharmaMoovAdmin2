using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PharmaMoov.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using PharmaMoov.Models.Admin;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using Microsoft.AspNetCore.Http;
using PharmaMoov.Models;
using Newtonsoft.Json;
using PharmaMoov.Models.User;

namespace PharmaMoov.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private IMainHttpClient HomeHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public HomeController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            HomeHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        public IActionResult Logout()
        {
            UserTypes logoutFrom = 0;

            if (AdminUCtxt != null)
            {
                logoutFrom = AdminUCtxt.AdminInfo.UserTypeId;

                _session.Clear();
                _session.Remove("adminUserContext");
            }

            if (logoutFrom == UserTypes.SUPERADMIN)
            {
                return RedirectToAction("Index", "SAdmin");
            }
            else
            {
                return RedirectToAction("Index", "Shop");
            }
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(UserChangePassword _admin)
        {
            if (ModelState.IsValid)
            {
                _admin.UserId = AdminUCtxt.AdminInfo.AdminId;
                _admin.UserTypeId = AdminUCtxt.AdminInfo.UserTypeId; //for Super and Pharmacy Admin

                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(HomeHttpClient.PostHttpClientRequest("Account/ChangePassword", _admin));

                // If token is invalid, redirect to login
                if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                    {
                        return RedirectToAction("Dashboard", "SAdmin");
                    }
                    else if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.ORDERPICKER)
                    {
                        return RedirectToAction("Index", "Order"); //Order Management
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "Shop");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = returnRes.Status + " : " + returnRes.Message;
                }

                return View();
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid Object Model";
            }

            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(AdminForgotPassword _admin)
        {
            if (ModelState.IsValid)
            {
                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(HomeHttpClient.PostHttpClientRequest("Admin/ForgotPassword", _admin));

                // If token is invalid, redirect to login
                if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //AccountTypes accountType = JsonConvert.DeserializeObject<AccountTypes>(returnRes.Payload.ToString());
                    UserTypes userTypes = JsonConvert.DeserializeObject<UserTypes>(returnRes.Payload.ToString());
                    return RedirectToAction("PasswordReset", new { aT = (int)userTypes });
                }
                else
                {
                    TempData["ErrorMessage"] = returnRes.Status + " : " + returnRes.Message;
                }

                return View();
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid Object Model";
            }

            return View();
        }

        public IActionResult PasswordReset()
        {
            var accType = Request.Query["aT"];
            if (accType.Count > 0)
            {
                ViewBag.AccountType = accType;
            }

            return View();
        }


        //BASE
        [HttpPost]
        public IActionResult UploadIcon()
        {
            return Ok(new { publicUrl = UploadFile(Request.Form.Files[0], UploadTypes.UploadIcon, HomeHttpClient) });
        }

        [HttpPost]
        public IActionResult UploadDocument()
        {
            return Ok(new { publicUrl = UploadFile(Request.Form.Files[0], UploadTypes.UploadDocument, HomeHttpClient) });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult AdminChangePassword()
        {
            ViewBag.SuperAdmin = (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN) ? "True" : "False";
            return View();
        }

        [HttpPost]
        public IActionResult AdminChangePassword(UserChangePassword _admin)
        {
            if (ModelState.IsValid)
            {
                _admin.UserId = AdminUCtxt.AdminInfo.AdminId;
                _admin.UserTypeId = AdminUCtxt.AdminInfo.UserTypeId; //for Super and Pharmacy Admin

                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(HomeHttpClient.PostHttpClientRequest("Account/ChangePassword", _admin));

                // If token is invalid, redirect to login
                if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                ViewBag.ModalTitle = returnRes.Message;
                ViewBag.SuperAdmin = (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN) ? "True" : "False";
                if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                    //{
                    //    return RedirectToAction("Dashboard", "SAdmin");
                    //}                   
                    //else
                    //{
                    //    return RedirectToAction("Dashboard", "Shop");
                    //}
                    ViewBag.ShowModal = "true";
                    //TempData["SuccessMessage"] = returnRes.Message;
                }
                else
                {
                    ViewBag.ShowModal = "false";
                    //TempData["ErrorMessage"] = returnRes.Status + " : " + returnRes.Message;
                }

                return View();
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid Object Model";
            }

            return View();
        }
    }
}
