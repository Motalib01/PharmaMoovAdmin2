using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmaMoov.Admin.DataLayer;
using PharmaMoov.Admin.Helpers;
using PharmaMoov.Models;
using PharmaMoov.Models.Admin;
using PharmaMoov.Models.Product;
using System.Collections.Generic;
using System;
using PharmaMoov.Models.ExternalProduct;
using Microsoft.Extensions.Configuration;
using PharmaMoov.Models.Shop;

namespace PharmaMoov.Admin.Controllers
{
	public class ExternalProductController : BaseController
	{
		private IMainHttpClient ProductHttpClient { get; }
		private readonly IConfiguration _configuration;
		public AdminUserContext AdminUCtxt { get; set; }
		private ConfigMaster MConf { get; }
		private readonly IHttpContextAccessor _httpCtxtAcc;
		private ISession _session => _httpCtxtAcc.HttpContext.Session;

		public ExternalProductController(IMainHttpClient _mhttpc, IHttpContextAccessor httpContextAccessor, ConfigMaster _conf, IConfiguration configuration)
		{
			ProductHttpClient = _mhttpc;
			_httpCtxtAcc = httpContextAccessor;
			MConf = _conf;
			AdminUCtxt = _session.GetObjectFromJson<AdminUserContext>("adminUserContext");
			_configuration = configuration;
		}
		public IActionResult Index()
		{
			APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>(ProductHttpClient.GetHttpClientRequest("Product/GetAllExternalProducts/" + "0"));

			if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

			IEnumerable<ExternalProductList> externalProductList = JsonConvert.DeserializeObject<IEnumerable<ExternalProductList>>(returnRes.Payload.ToString());
			string basePath = _configuration["WebAPI:Link"];
			ViewBag.Current = "ExternalProduct";
			ViewBag.BasePath = basePath;
			return View(externalProductList);
		}

		[HttpPost]
		public IActionResult AddProductFromExtern(int productRecordId)
		{
			if (ModelState.IsValid)
			{
				Guid ShopID = AdminUCtxt.AdminInfo.ShopId;
				var requestBody = new AddProductRequest
				{
					Shop = ShopID,
					ProductRecordId = productRecordId
				};
				APIResponse returnRes = JsonConvert.DeserializeObject<APIResponse>
					(ProductHttpClient.PostHttpClientRequest("Product/AddProductFromExtern", requestBody));

				// If token is invalid, redirect to login
				if (base.IsTokenInvalidUsingResponse(returnRes, "Unathorized access.")) { return RedirectToAction("Logout", "Home"); }

				
				ViewBag.ModalTitle = returnRes.Message;

				return Ok(returnRes.Message);
			}
			else
			{
				return Json(new APIResponse { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid Object!", ModelError = ModelState.Errors() });
			}
		}
	}
}
