using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;
using core_localization.Models;
using core_localization;

namespace core_localization.Controllers
{
    public class HomeController : Controller
    {
        public readonly IStringLocalizer<HomeController> _localizer;
        public readonly IStringLocalizer<SharedResources> _sharedLocalize;
        public readonly ILogger<HomeController> _logger;
        public HomeController(IStringLocalizer<HomeController> localizer,
            IStringLocalizer<SharedResources> sharedLocalizer,
            ILogger<HomeController> logger)
        {
            _localizer = localizer;
            _sharedLocalize = sharedLocalizer;
            _logger = logger;
        }
        public IActionResult Index()
        {
            ViewData["hello home"] = _localizer["Hello Home"];
            ViewData["hello"] = _sharedLocalize["Hello"];
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["hello home"] = _localizer["Hello Home"] + " in Contact";
            ViewData["hello"] = _sharedLocalize["Hello"] + " in Contact";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
