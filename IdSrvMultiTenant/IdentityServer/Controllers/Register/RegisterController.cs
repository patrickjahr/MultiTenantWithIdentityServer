using System;
using System.Threading.Tasks;
using Core.Services;
using IdentityServer.Models;
using IdentityServer.Services;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IdentityServer.Controllers.Register
{
    [SecurityHeaders]
    public class RegisterController : Controller
    {
        private readonly IRegistrationService _registrationService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegisterController(
            IRegistrationService registrationService,
            ITenantService tenantService,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _registrationService = registrationService;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(model.Tenant.IsNullOrEmpty() && GetTenantName().IsNullOrEmpty())
                ModelState.AddModelError(nameof(model.Tenant), $"Tenantname not available");
                
            if (ModelState.IsValid)
            {
                if (model.Tenant.IsNullOrEmpty())
                    model.Tenant = GetTenantName();
                
                var result = await _registrationService.CreateTenantAndUserAsync(model.Email, model.Password, model.Tenant);

                if (result.ResultType == RegistrationResultType.Success)
                {
                    Console.WriteLine($"User Create with tenant {model.Tenant} and has the following result: {JsonConvert.SerializeObject(result)}");
                    //TODO: Move to settings as template
                    return Redirect(_hostingEnvironment.IsDevelopment()
                        ? $"http://{model.Tenant}.gameslist.local:4200/login"
                        : $"https://{model.Tenant}.gameslist.local.com/login");
                }

                if (result.ResultType == RegistrationResultType.TenantNameNotAvailable)
                {
                    ModelState.AddModelError(nameof(model.Tenant), $"Tenantname {model.Tenant} not available");
                }

                if (result.IdentityResult?.Errors != null)
                {
                    foreach (var error in result.IdentityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        private string GetTenantName()
        {
            var host = _httpContextAccessor.HttpContext.Request.Host.Host;
            var splitHost = host.Split('.');

            return splitHost.Length < 2 ? string.Empty : splitHost[0];
        }
    }
}