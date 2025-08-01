using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PharmaMoov.Models
{
    public class APIResponse
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }
        private object _payload;
        public APIResponseCode? ResponseCode { get; set; }
        public object Payload
        {
            set
            {
                if (value == null)
                {
                    this._payload = value;
                }
                else if (value.GetType() == typeof(PharmaMoov.Models.User.UserContext))
                {
                    PharmaMoov.Models.User.UserContext userctxt = value as PharmaMoov.Models.User.UserContext;
                    if (userctxt != null)
                    {
                        userctxt.UserInfo.Password = "";
                        this._payload = userctxt as object;
                    }
                }
                else if (value.GetType() == typeof(PharmaMoov.Models.User.User))
                {
                    PharmaMoov.Models.User.User userInfo = value as PharmaMoov.Models.User.User;
                    if (userInfo != null)
                    {
                        userInfo.Password = "";
                        this._payload = userInfo as object;
                    }
                    this._payload = value;
                }
                else if (value.GetType() == typeof(PharmaMoov.Models.Admin.AdminUserContext))
                {
                    PharmaMoov.Models.Admin.AdminUserContext adminContext = value as PharmaMoov.Models.Admin.AdminUserContext;
                    if (adminContext != null)
                    {
                        adminContext.AdminInfo.Password = "";
                        this._payload = adminContext as object;
                    }
                    this._payload = value;
                }
                else
                {
                    this._payload = value;
                }
            }
            get { return this._payload; }
        }
        public IEnumerable<KeyValuePair<string, string[]>> ModelError { get; set; }
    }

    public static class ModelStateHelper
    {
        public static IEnumerable<KeyValuePair<string, string[]>> Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                    .Where(m => m.Value.Any());
            }

            return null;
        }
    }
}
