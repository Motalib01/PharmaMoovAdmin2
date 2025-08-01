using System;
using System.Collections.Generic;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PharmaMoov.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private IMainHttpClient OrderHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public OrderController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            OrderHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId != UserTypes.SUPERADMIN)
                {
                    //Access for Shop Admin, OP Manager and Order Picker
                    Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(OrderHttpClient.GetHttpClientRequest("Order/ShopsOrders/" + ShopID + "/false"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        IEnumerable<ShopOrderList> returnList = JsonConvert.DeserializeObject<IEnumerable<ShopOrderList>>(returnRes.Payload.ToString());

                        ViewBag.Current = "Order";
                        return View(returnList);
                    }

                    ViewBag.Current = "Order";
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Shop");
                }
            }
            else
            {
                return RedirectToAction("Index", "Shop");
            }
        }

        [HttpGet]
        public IActionResult OrderDetails(int rid)
        {
            if (AdminUCtxt != null && AdminUCtxt.AdminInfo.UserTypeId != UserTypes.SUPERADMIN)
            {
                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(OrderHttpClient.GetHttpClientRequest("Order/UserOrderDetails/" + rid));

                // If token is invalid, redirect to login
                //if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    OrderDetailsDTO returnDetails = JsonConvert.DeserializeObject<OrderDetailsDTO>(returnRes.Payload.ToString());
                    ViewBag.Current = "Order";
                    return View(returnDetails);
                }

                ViewBag.Current = "Order";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Shop");
            }
        }

        [HttpPost]
        public IActionResult ChangeOrderStatus([FromBody] ChangeOrderStatus _order)
        {
            if (AdminUCtxt != null)
            {
                //Manage Order status for the following user admin roles
                if (AdminUCtxt.AdminInfo.UserTypeId != UserTypes.SHOPADMIN || AdminUCtxt.AdminInfo.UserTypeId != UserTypes.ORDERPICKER || AdminUCtxt.AdminInfo.UserTypeId != UserTypes.OPERATIONMANAGER)
                {
                    Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
                    _order.UserId = AdminID;

                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(OrderHttpClient.PostHttpClientRequest("Order/ChangeOrderStatus/", _order));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.Current = "Order";
                        return Json(returnRes);
                    }

                    ViewBag.Current = "Order";
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Shop");
                }
            }
            else
            {
                return RedirectToAction("Index", "Shop");
            }
        }

        [HttpGet]
        public IActionResult History()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SHOPADMIN)
                {
                    Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(OrderHttpClient.GetHttpClientRequest("Order/ShopsOrders/" + ShopID + "/true"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        IEnumerable<ShopOrderList> returnList = JsonConvert.DeserializeObject<IEnumerable<ShopOrderList>>(returnRes.Payload.ToString());

                        ViewBag.Current = "OrderHistory";
                        return View(returnList);
                    }

                    ViewBag.Current = "OrderHistory";
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Shop");
                }
            }
            else
            {
                return RedirectToAction("Index", "Shop");
            }
        }

        [HttpPost]
        public IActionResult CreateJob([FromBody] JobModel _job)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SHOPADMIN)
                {
                    JobValidationModel validateJob = JsonConvert.DeserializeObject<JobValidationModel>(JsonConvert.DeserializeObject<APIResponse>
                                    (OrderHttpClient.PostHttpClientRequest("DeliveryJob/ValidateJobByOrder", _job)).Payload.ToString());

                    if (validateJob.Valid == true)
                    {
                        APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(OrderHttpClient.PostHttpClientRequest("DeliveryJob/CreateJob/", _job));
                        JobOutput newJob = JsonConvert.DeserializeObject<JobOutput>(returnRes.Payload.ToString());

                        return Json(returnRes);
                    }
                    else
                    {
                        return Json(validateJob.Valid);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Shop");
                }
            }
            else
            {
                return RedirectToAction("Index", "Shop");
            }
        }
    }
}
