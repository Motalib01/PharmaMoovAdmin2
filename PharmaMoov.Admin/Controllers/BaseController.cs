using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Models.Attachment;
using PharmaMoov.Models;
using PharmaMoov.Models.Shop;

namespace PharmaMoov.Admin.Controllers
{
    public class BaseController : Controller
    {
        public string UploadFile(IFormFile file, UploadTypes ut,IMainHttpClient mainHttpClient)
        {
            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream()))
            {
                data = br.ReadBytes((int)file.OpenReadStream().Length);
            }
            ByteArrayContent bytes = new ByteArrayContent(data);
            MultipartFormDataContent multiC = new MultipartFormDataContent();
            multiC.Add(bytes, "file", file.FileName);
            string res = mainHttpClient.PostFileHttpCientRequest("Attachment/" + ut.ToString(), multiC);

            return JsonConvert.DeserializeObject<Attachment>(
                JsonConvert.DeserializeObject<APIResponse>(
                    res
                ).Payload.ToString()
            ).AttachmentExternalUrl;
        }

        public bool IsTokenInvalidUsingResponse(APIResponse ApiResp, string CustomMessage = "Error")
        {
            if (ApiResp != null && ApiResp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Set error message
                TempData["ErrorMessage"] = CustomMessage + " : " + ApiResp.Message;
                return true;
            }

            return false;
        }

        public bool IsTokenInvalidUsingHttpClient(IMainHttpClient MainHttpClient, string CustomMessage = "Error")
        {
            if (!(MainHttpClient as MainHttpClient).IsRefreshTokenExpired())
            {
                // Set error message
                TempData["ErrorMessage"] = CustomMessage == "Error" ? "Session has expired. Please login." : (CustomMessage + " : Session has expired. Please login.");
                return true;
            }

            return false;
        }
    }
}