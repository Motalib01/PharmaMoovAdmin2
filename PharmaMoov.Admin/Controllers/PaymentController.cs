using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;

namespace PharmaMoov.Admin.Controllers
{
    public class PaymentController : BaseController
    {
        private IMainHttpClient PaymentHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public PaymentController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            PaymentHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (AdminUCtxt != null)
            {
                ViewBag.Current = "Payment";

                Guid shopId = AdminUCtxt.AdminInfo.ShopId;

                PaymentListParamModel paymentParam = new PaymentListParamModel
                {
                    DateFrom = DateTime.Now.AddMonths(-2),
                    DateTo = DateTime.Now,
                    ShopId = shopId
                };

                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    paymentParam.ShopId = Guid.Empty; //Get all payments for super admin

                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PaymentHttpClient.PostHttpClientRequest("Payment/GetAllPayments", paymentParam));
                    if (returnRes.StatusCode == HttpStatusCode.OK && returnRes.Payload != null)
                    {
                        List<PaymentListModel> paymentList = JsonConvert.DeserializeObject<List<PaymentListModel>>(returnRes.Payload.ToString());
                        ViewBag.PaymentList = paymentList;
                        return View(paymentParam);
                    }
                    else
                    {
                        return View(paymentParam);
                    }
                }
                else
                {
                    if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SHOPADMIN || AdminUCtxt.AdminInfo.UserTypeId == UserTypes.OPERATIONMANAGER)
                    {
                        APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PaymentHttpClient.PostHttpClientRequest("Payment/GetAllPayments", paymentParam));
                        if (returnRes.StatusCode == HttpStatusCode.OK && returnRes.Payload != null)
                        {
                            List<PaymentListModel> paymentList = JsonConvert.DeserializeObject<List<PaymentListModel>>(returnRes.Payload.ToString());
                            ViewBag.PaymentList = paymentList;
                            return View(paymentParam);
                        }
                        else
                        {
                            return View(paymentParam);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Shop");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpPost]
        public IActionResult Index([FromForm] PaymentListParamModel _dateRange)
        {
            if (AdminUCtxt != null)
            {
                ViewBag.Current = "Payment";

                Guid shopId = AdminUCtxt.AdminInfo.ShopId;
                _dateRange.ShopId = shopId;

                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    _dateRange.ShopId = Guid.Empty; //Get all payments for super admin

                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PaymentHttpClient.PostHttpClientRequest("Payment/GetAllPayments", _dateRange));
                    if (returnRes.StatusCode == HttpStatusCode.OK && returnRes.Payload != null)
                    {
                        List<PaymentListModel> paymentList = JsonConvert.DeserializeObject<List<PaymentListModel>>(returnRes.Payload.ToString());
                        ViewBag.PaymentList = paymentList;
                        return View(_dateRange);
                    }
                    else
                    {
                        return View(_dateRange);
                    }
                }
                else
                {
                    if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SHOPADMIN || AdminUCtxt.AdminInfo.UserTypeId == UserTypes.OPERATIONMANAGER)
                    {
                        APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PaymentHttpClient.PostHttpClientRequest("Payment/GetAllPayments", _dateRange));
                        if (returnRes.StatusCode == HttpStatusCode.OK && returnRes.Payload != null)
                        {
                            List<PaymentListModel> paymentList = JsonConvert.DeserializeObject<List<PaymentListModel>>(returnRes.Payload.ToString());
                            ViewBag.PaymentList = paymentList;
                            return View(_dateRange);
                        }
                        else
                        {
                            return View(_dateRange);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Shop");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpGet]
        public IActionResult Invoice(int orderId)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN || AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SHOPADMIN)
                {
                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(PaymentHttpClient.GetHttpClientRequest("Payment/GetPaymentInvoice/" + orderId));

                    if (returnRes.StatusCode == HttpStatusCode.OK && returnRes.Payload != null)
                    {
                        PaymentInvoiceModel returnObject = JsonConvert.DeserializeObject<PaymentInvoiceModel>(returnRes.Payload.ToString());
                        return View(returnObject);
                    }
                    else
                    {
                        return View(new PaymentInvoiceModel());
                    }
                }
                else
                {
                    return RedirectToAction("Invoice", "Payment");
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }
    }
}
