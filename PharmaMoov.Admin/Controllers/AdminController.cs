using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models;

namespace PharmaMoov.Admin.Controllers
{
    public class AdminController : BaseController
    {
        private IMainHttpClient AdminHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public AdminController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            AdminHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        [HttpGet]
        public IActionResult Index()
        {
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(AdminHttpClient.GetHttpClientRequest("Admin/AllAdmins/" + ShopID + "/0"));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            IEnumerable<AdminList> returnList = JsonConvert.DeserializeObject<IEnumerable<AdminList>>(returnRes.Payload.ToString());
            returnList = returnList.Where(x => x.UserType != UserTypes.SUPERADMIN);

            ViewBag.Current = "Users";
            return View(returnList);
        }

        [HttpGet]
        public IActionResult AddAdmin()
        {
            ViewBag.DefaultIcon = MConf.WebApiBaseUrl.Replace("api/", "") + "resources/Icons/default-img.png";
            ViewBag.Current = "Users";
            return View();
        }

        [HttpPost]
        public IActionResult AddAdmin(AdminProfile _admin)
        {
            if (ModelState.IsValid)
            {
                Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
                Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
                _admin.ShopId = ShopID;
                _admin.AdminId = AdminID;
                _admin.AccountType = AdminUCtxt.AdminInfo.AccountType;
                //_admin.UserTypeId = UserTypes.SHOPADMIN;   //Default: Shop Admin can only add SHOPADMIN users

                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(AdminHttpClient.PostHttpClientRequest("Admin/RegisterAdmin", _admin));

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
                    ViewBag.DefaultIcon = _admin.AdminIcon;
                }
                ViewBag.Current = "Users";
                return View();
            }
            else
            {
                ViewBag.Current = "Users";
                return Json(new APIResponse { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid Object!", ModelError = ModelState.Errors() });
            }
        }

        [HttpGet]
        public IActionResult EditAdmin(int pid)
        {
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(AdminHttpClient.GetHttpClientRequest("Admin/AllAdmins/" + ShopID + "/" + pid));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            EditAdminProfile returnDetails = JsonConvert.DeserializeObject<EditAdminProfile>(returnRes.Payload.ToString());
            ViewBag.Current = "Users";
            return View(returnDetails);
        }

        [HttpPost]
        public IActionResult EditAdmin(EditAdminProfile _admin)
        {
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                _admin.AccountType = AdminUCtxt.AdminInfo.AccountType;

                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(AdminHttpClient.PostHttpClientRequest("Admin/EditAdminProfile", _admin));

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
                    ViewBag.DefaultIcon = _admin.AdminIcon;
                }
                ViewBag.Current = "Users";
                return View();
            }
            else
            {
                ViewBag.Current = "Users";
                return Json(new APIResponse { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid Object!", ModelError = ModelState.Errors() });
            }
        }

        [HttpPost]
        public IActionResult ChangeAdminStatus([FromBody] ChangeRecordStatus _admin)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            _admin.AdminId = AdminID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(AdminHttpClient.PostHttpClientRequest("Admin/ChangeAdminStatus", _admin));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
            ViewBag.Current = "Users";
            return Json(returnRes);
        }
    }
}
