using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.Prescription;

namespace PharmaMoov.Admin.Controllers
{
    public class PrescriptionController : BaseController
    {
        private IMainHttpClient PrescriptionHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public PrescriptionController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            PrescriptionHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        [HttpGet]
        public IActionResult Index()
        {
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PrescriptionHttpClient.GetHttpClientRequest("Prescription/PopulatePrescriptions/0/0/" + ShopID));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            IEnumerable<PrescriptionDetail> returnList = JsonConvert.DeserializeObject<IEnumerable<PrescriptionDetail>>(returnRes.Payload.ToString());

            ViewBag.Current = "Prescriptions";

            return View(returnList);
        }

        [HttpGet]
        public IActionResult PrescriptionDetails(int pId)
        {
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PrescriptionHttpClient.GetHttpClientRequest("Prescription/GetPrescription/" + pId));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            PrescriptionDetailAndProducts returnList = JsonConvert.DeserializeObject<PrescriptionDetailAndProducts>(returnRes.Payload.ToString());

            ViewBag.Current = "Prescriptions";
            ViewBag.MyRouteId = pId;

            return View(returnList);
        }

        [HttpPost]
        public IActionResult PrescriptionDetails(List<PrescriptionProductsParamModel> prescriptionList)
        {
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PrescriptionHttpClient.PostHttpClientRequest("Prescription/AddPrescriptionProducts", prescriptionList));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            if (returnRes != null && returnRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Json(new { cStatus = returnRes.StatusCode, cMessage = returnRes.Message });
            }
            else if (returnRes.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return Json(new { cStatus = returnRes.StatusCode, cMessage = returnRes.Message });
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult GetProductsForPrescription(int pId)
        {
            Guid ShopID = AdminUCtxt.AdminInfo.ShopId;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PrescriptionHttpClient.GetHttpClientRequest("Product/GetProductsForPrescription/" + ShopID + "/" + pId));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
            
            ViewBag.MyRouteId = pId;
            
            if (returnRes != null && returnRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                
                return Json(new { data = returnRes.Payload });
            }

            return View();
        }

        [HttpGet]
        public IActionResult GetSelectedProductsForPrescription(int pId)
        {
            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PrescriptionHttpClient.GetHttpClientRequest("Prescription/GetPrescriptionDetails/" + pId));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

            if (returnRes != null && returnRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Json(new { data = returnRes.Payload });
            }


            return View();
        }
    }
}
