using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmaMoov.Models.Shop;

namespace PharmaMoov.Admin.Controllers
{
    public class UserMgtController : BaseController
    {
        private IMainHttpClient UserMgtHttpClient { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private ConfigMaster MConf { get; }
        private readonly IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;

        public UserMgtController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            UserMgtHttpClient = _mhttpc;
            _httpCtxtAcc = httpContextAccessor;
            MConf = _conf;
            AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Customers()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.GetHttpClientRequest("Customer/GetCustomers/0"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    ViewBag.Current = "Customers";

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<User> returnList = JsonConvert.DeserializeObject<List<User>>(returnRes.Payload.ToString());
                        return View(returnList);
                    }
                    else
                    {
                        return View(new List<User>());
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
        public IActionResult EditCustomer(int CustomerID)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.GetHttpClientRequest("Customer/GetCustomers/" + CustomerID.ToString()));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        User ReturnedPromo = JsonConvert.DeserializeObject<User>(returnRes.Payload.ToString());
                        if (string.IsNullOrEmpty(ReturnedPromo.ImageUrl))
                        {
                            ReturnedPromo.ImageUrl = MConf.WebApiBaseUrl.Replace("api/", "") + "resources/Icons/default-img.png";
                        }
                        ViewBag.ShowModal = 0;
                        ViewBag.ModalTitle = "Customer";
                        ViewBag.ModalMessage = returnRes.Message;
                        return View(ReturnedPromo);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Customer";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new User());
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
        public IActionResult EditCustomer(User CustomerProfile)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.PostHttpClientRequest("User/EditUserProfile/", CustomerProfile));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        User ReturnedPromo = JsonConvert.DeserializeObject<User>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Customer";
                        ViewBag.ModalMessage = "Profil du client mis à jour";
                        return View(ReturnedPromo);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Customer";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new User());
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
        public IActionResult ShopOwners()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.GetHttpClientRequest("Shop/ShopList/"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    ViewBag.Current = "Owners";

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<ShopList> returnList = JsonConvert.DeserializeObject<List<ShopList>>(returnRes.Payload.ToString());
                        return View(returnList);
                    }
                    else
                    {
                        return View(new List<User>());
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
        public IActionResult EditShopOwner(int ShopID)
        {
            if (AdminUCtxt != null && AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
            {
                ViewBag.Current = "ShopOwner";
                var responseData = UserMgtHttpClient.GetHttpClientRequest("Shop/GetPharmacyOwner/" + ShopID);
                APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(responseData);
                
                // If token is invalid, redirect to login
                if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    PharmacyOwner Shopdetail = JsonConvert.DeserializeObject<PharmacyOwner>(returnRes.Payload.ToString());
                    
                    if (string.IsNullOrEmpty(Shopdetail.ShopIcon))
                    {
                        Shopdetail.ShopIcon = MConf.WebApiBaseUrl.Replace("api/", "") + "resources/Icons/default-img.png";
                    }

                    ViewBag.ShowModal = 0;
                    ViewBag.ModalTitle = "Shop Owner";                   
                    ViewBag.ModalMessage = returnRes.Message;
                    return View(Shopdetail);
                }
                else
                {
                    ViewBag.ShowModal = 2;
                    ViewBag.ModalTitle = "Shop Owner";
                    ViewBag.ModalMessage = returnRes.Message;
                    ViewBag.ErrMsg = returnRes.Message;
                    return View(new PharmacyOwner());
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
            
        }

        [HttpPost]
        public IActionResult EditShopOwner(PharmacyOwner model)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                   
                    model.AdminID = AdminUCtxt.AdminInfo.AdminId;
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.PostHttpClientRequest("Shop/UpdatePharmacyOwner/", model));
                   
                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        PharmacyOwner ReturnedPromo = JsonConvert.DeserializeObject<PharmacyOwner>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Shop Owner";
                        ViewBag.ModalMessage = "Profil du propriétaire de la boutique mis à jour";
                        return View(ReturnedPromo);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Shop Owner";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new PharmacyOwner());
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
        public IActionResult Admin()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.GetHttpClientRequest("Admin/GetAdminList"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    ViewBag.Current = "Admin";

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<PharmaMoov.Models.Admin.Admin> returnList = JsonConvert.DeserializeObject<List<PharmaMoov.Models.Admin.Admin>>(returnRes.Payload.ToString());
                        return View(returnList);
                    }
                    else
                    {
                        return View(new List<PharmaMoov.Models.Admin.Admin>());
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
        public IActionResult AddAdmin()
        {
            ViewBag.DefaultIcon = MConf.WebApiBaseUrl.Replace("api/", "") + "resources/Icons/default-img.png";
            ViewBag.Current = "Admin";
            return View();
        }

        [HttpPost]
        public IActionResult AddAdmin(AdminProfile _admin)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
                    Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
                    _admin.ShopId = ShopID;
                    _admin.AdminId = AdminID;
                    _admin.AccountType = AdminUCtxt.AdminInfo.AccountType;

                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(UserMgtHttpClient.PostHttpClientRequest("Admin/RegisterAdmin", _admin));

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
                    return RedirectToAction("Index", "SAdmin");
                }
            }
            else
            {
                return RedirectToAction("Index", "SAdmin");
            }
        }

        [HttpGet]
        public IActionResult EditAdmin(int pid)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    Guid ShopID = Guid.Empty;

                    APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(UserMgtHttpClient.GetHttpClientRequest("Admin/AllAdmins/" + ShopID + "/" + pid));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    EditAdminProfile returnDetails = JsonConvert.DeserializeObject<EditAdminProfile>(returnRes.Payload.ToString());
                    if(string.IsNullOrEmpty(returnDetails.ImageUrl))
                    {
                        returnDetails.ImageUrl = MConf.WebApiBaseUrl.Replace("api/", "") + "resources/Icons/default-img.png";
                    }
                    return View(returnDetails);
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
        public IActionResult EditAdmin(EditAdminProfile _admin)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.PostHttpClientRequest("Admin/EditAdminProfile", _admin));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        EditAdminProfile ReturnedPromo = JsonConvert.DeserializeObject<EditAdminProfile>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Admin Details";
                        ViewBag.ModalMessage = "Profil d'administrateur mis à jour";
                        return View(ReturnedPromo);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Admin Details";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new AdminProfile());
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

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Couriers()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.GetHttpClientRequest("Courier/GetCouriers/0"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    ViewBag.Current = "Courriers";

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<User> returnList = JsonConvert.DeserializeObject<List<User>>(returnRes.Payload.ToString());
                        return View(returnList);
                    }
                    else
                    {
                        return View(new List<User>());
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
        public IActionResult HealthProfessionals()
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.GetHttpClientRequest("HealthProfessional/GetHealthProfessionals/0"));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    ViewBag.Current = "HealthProfessionals";

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<User> returnList = JsonConvert.DeserializeObject<List<User>>(returnRes.Payload.ToString());
                        return View(returnList);
                    }
                    else
                    {
                        return View(new List<User>());
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
        public IActionResult EditCourier(int CourierID)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.GetHttpClientRequest("Courier/GetCouriers/" + CourierID.ToString()));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        User ReturnedPromo = JsonConvert.DeserializeObject<User>(returnRes.Payload.ToString());
                        if (string.IsNullOrEmpty(ReturnedPromo.ImageUrl))
                        {
                            ReturnedPromo.ImageUrl = MConf.WebApiBaseUrl.Replace("api/", "") + "resources/Icons/default-img.png";
                        }
                        ViewBag.ShowModal = 0;
                        ViewBag.ModalTitle = "Courier";
                        ViewBag.ModalMessage = returnRes.Message;
                        return View(ReturnedPromo);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Courier";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new User());
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
        public IActionResult EditCourier(User CourierProfile)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.PostHttpClientRequest("User/EditUserProfile/", CourierProfile));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        User ReturnedPromo = JsonConvert.DeserializeObject<User>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Courier";
                        ViewBag.ModalMessage = "Updated Courier Profile";
                        return View(ReturnedPromo);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Courier";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new User());
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
        public IActionResult EditHealthProfessional(int HealthProfID)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.GetHttpClientRequest("HealthProfessional/GetHealthProfessionals/" + HealthProfID.ToString()));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        User ReturnedPromo = JsonConvert.DeserializeObject<User>(returnRes.Payload.ToString());
                        if (string.IsNullOrEmpty(ReturnedPromo.ImageUrl))
                        {
                            ReturnedPromo.ImageUrl = MConf.WebApiBaseUrl.Replace("api/", "") + "resources/Icons/default-img.png";
                        }
                        ViewBag.ShowModal = 0;
                        ViewBag.ModalTitle = "Health Professional";
                        ViewBag.ModalMessage = returnRes.Message;
                        return View(ReturnedPromo);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Health Professional";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new User());
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
        public IActionResult EditHealthProfessional(User HealthProfProfile)
        {
            if (AdminUCtxt != null)
            {
                if (AdminUCtxt.AdminInfo.UserTypeId == UserTypes.SUPERADMIN)
                {
                    APIResponse returnRes = JsonConvert
                        .DeserializeObject<APIResponse>
                        (UserMgtHttpClient.PostHttpClientRequest("User/EditUserProfile/", HealthProfProfile));

                    // If token is invalid, redirect to login
                    if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

                    if (returnRes.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        User ReturnedPromo = JsonConvert.DeserializeObject<User>(returnRes.Payload.ToString());
                        ViewBag.ShowModal = 1;
                        ViewBag.ModalTitle = "Health Professional";
                        ViewBag.ModalMessage = "Mettre à jour le profil du professionnel de la santé";
                        return View(ReturnedPromo);
                    }
                    else
                    {
                        ViewBag.ShowModal = 2;
                        ViewBag.ModalTitle = "Health Professional";
                        ViewBag.ModalMessage = returnRes.Message;
                        ViewBag.ErrMsg = returnRes.Message;
                        return View(new User());
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

        #region Section: For Customer, Courier and Health Prof under Super Admin
        [HttpPost]
        public IActionResult ChangeUserStatus([FromBody] ChangeRecordStatus _admin)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            _admin.AdminId = AdminID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(UserMgtHttpClient.PostHttpClientRequest("User/ChangeUserStatus", _admin));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
            return Json(returnRes);
        }

        #endregion

        [HttpPost]
        public IActionResult ChangeAcceptOrDeclineRequest([FromBody] ChangeAcceptOrDeclineRequestStatus _admin)
        {
            Guid AdminID = AdminUCtxt.AdminInfo.AdminId;
            _admin.AdminId = AdminID;

            APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(UserMgtHttpClient.PostHttpClientRequest("User/ChangeAcceptOrDeclineRequest", _admin));

            // If token is invalid, redirect to login
            if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }
            return Json(returnRes);
        }
    }
}
