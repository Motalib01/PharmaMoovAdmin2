using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.Review;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PharmaMoov.Admin.Controllers
{
    public class ReviewsController : BaseController
    {
        private IMainHttpClient ReviewHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public ReviewsController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            ReviewHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        public IActionResult Index()
        {
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ReviewHttpClient.GetHttpClientRequest("Reviews/ShopReviews/" + ShopID + "/0"));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                IEnumerable<ShopReviewList> returnList = JsonConvert.DeserializeObject<IEnumerable<ShopReviewList>>(returnRes.Payload.ToString());

                ViewBag.Current = "Reviews";
                return View(returnList);
            }

            ViewBag.Current = "Reviews";
            return View();
        }
    }
}
