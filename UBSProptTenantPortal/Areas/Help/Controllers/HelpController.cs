using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UBSProptTenantPortal.Models;

namespace UBSProptTenantPortal.Areas.Help
{
    [Area("Help")]
    public class HelpController : BaseController
    {
        private readonly ILogger<HelpController> _logger;
        public HelpController(ILogger<HelpController> logger)
        {
            _logger = logger;
        }

        public IActionResult Help()
        {
            var model = new HelpViewModel
            {
                MobileNumber1 = "026354444 (with in UAE)",
                MobileNumber2 = "+9716354444 (outside UAE)",
                Email = "FABProps@bankfab.com",
            };
            return View(model);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}

