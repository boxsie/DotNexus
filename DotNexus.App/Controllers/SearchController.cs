using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNexus.App.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        public IActionResult Index(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return RedirectToAction("index");

            searchTerm = searchTerm.Replace(",", "").Trim();

            var isNumber = int.TryParse(searchTerm, out var height);

            if (isNumber)
                return RedirectToRoute("block", new { blockId = height });
            
            return View();
        }
    }
}