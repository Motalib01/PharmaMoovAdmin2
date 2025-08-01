using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.User;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PharmaMoov.Admin.DataLayer
{
    public class MainHttpClient : IMainHttpClient
    {
        private ConfigMaster MConfig { get; }
        private IConfiguration Configuration { get; }
        public AdminUserContext AdminUCtxt { get; set; }
        private IHttpContextAccessor _httpCtxtAcc;
        private ISession _session => _httpCtxtAcc.HttpContext.Session;
        private int flagTokenExpires = 0;

        public MainHttpClient(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf)
        {
            MConfig = _conf;
            _httpCtxtAcc = httpContextAccessor;

            if (_session != null)
            {
                AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
            }
        }

        public string PostHttpClientRequest(string requestEndPoint, object content)
        {
            if (flagTokenExpires == 0 && requestEndPoint != "Admin/AdminLogin" && requestEndPoint != "Admin/ForgotPassword")
            {
                if (!IsRefreshTokenExpired())
                {
                    return JsonConvert.SerializeObject(new APIResponse()
                    {
                        StatusCode = System.Net.HttpStatusCode.Unauthorized,
                        Status = "Error",
                        Message = "User needs to login"
                    });
                } //Token-Expired
            }

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(MConfig.WebApiBaseUrl);

                client.DefaultRequestHeaders.Clear();

                if (AdminUCtxt != null)
                {
                    //Authorization Token for POST
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + AdminUCtxt.Tokens.Token);
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", UCtxt.Token);
                }

                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = client.PostAsJsonAsync(requestEndPoint, content).Result;
                //Res.Content = new StringContent()
                return Res.Content.ReadAsStringAsync().Result;

            }

        }

        public string GetHttpClientRequest(string requestEndPoint)
        {
            if (flagTokenExpires == 0 && requestEndPoint != "Admin/AdminLogin")
            {
                if (!IsRefreshTokenExpired())
                {
                    return JsonConvert.SerializeObject(new APIResponse()
                    {
                        StatusCode = System.Net.HttpStatusCode.Unauthorized,
                        Status = "Error",
                        Message = "User needs to login"
                    });
                } //Token-Expired
            }

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(MConfig.WebApiBaseUrl);

                client.DefaultRequestHeaders.Clear();

                if (AdminUCtxt != null)
                {
                    //Authorization Token for GET
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + AdminUCtxt.Tokens.Token);
                }

                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = client.GetAsync(requestEndPoint).Result;
                return Res.Content.ReadAsStringAsync().Result;
            }
        }

        public bool IsRefreshTokenExpired()
        {
            if (AdminUCtxt != null && AdminUCtxt.Tokens.IsActive == true)
            {
                if (DateTime.Now > AdminUCtxt.Tokens.TokenExpiration)
                {
                    UserLoginTransaction LoginTrans = new UserLoginTransaction
                    {
                        UserId = AdminUCtxt.Tokens.UserId,
                        AccountType = AdminUCtxt.AdminInfo.AccountType,
                        Token = AdminUCtxt.Tokens.Token,
                        TokenExpiration = AdminUCtxt.Tokens.TokenExpiration,
                        RefreshToken = AdminUCtxt.Tokens.RefreshToken,
                        RefreshTokenExpiration = AdminUCtxt.Tokens.RefreshTokenExpiration,
                        Device = AdminUCtxt.Tokens.Device,
                        IsActive = AdminUCtxt.Tokens.IsActive
                    };

                    flagTokenExpires = 1;
                    string returnResult = this.PostHttpClientRequest("Admin/ReGenerateTokens", LoginTrans);

                    try
                    {
                        APIResponse ApiResp = JsonConvert.DeserializeObject<APIResponse>(returnResult);

                        if (ApiResp.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if (AdminUCtxt != null)
                            {
                                _session.Remove("adminUserContext");
                            }
                            AdminUCtxt = JsonConvert.DeserializeObject<AdminUserContext>(ApiResp.Payload.ToString());
                            _session.SetObjectAsJson("adminUserContext", AdminUCtxt);
                        }

                        if (AdminUCtxt.Tokens.IsActive == false) { return false; } else { return true; }
                    }
                    catch (Exception ex)
                    {
                        string errmsg = ex.Message;
                    }
                }
                return true;
            }
            return false;
        }

        public string PostFileHttpCientRequest(string _rEndPoint, HttpContent _content)
        {
            if (flagTokenExpires == 0 && _rEndPoint != "Admin/AdminLogin")
            {
                if (!IsRefreshTokenExpired()) { return "1"; } //Token-Expired
            }

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(MConfig.WebApiBaseUrl);

                client.DefaultRequestHeaders.Clear();

                if (AdminUCtxt != null)
                {
                    //Authorization Token for POST
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + AdminUCtxt.Tokens.Token);
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", UCtxt.Token);
                }

                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                //HttpResponseMessage Res = client.PostAsJsonAsync(_rEndPoint, _content).Result;
                HttpResponseMessage Res = client.PostAsync(_rEndPoint, _content).Result;
                //Res.Content = new StringContent()
                return Res.Content.ReadAsStringAsync().Result;

            }
        }

        public string GetAnnonHttpClientRequest(string requestEndPoint)
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(MConfig.WebApiBaseUrl);

                client.DefaultRequestHeaders.Clear();

                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = client.GetAsync(requestEndPoint).Result;
                return Res.Content.ReadAsStringAsync().Result;
            }
        }
       
    }
}
