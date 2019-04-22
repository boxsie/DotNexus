using System;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.App.Models;
using DotNexus.Core.Nexus;
using DotNexus.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNexus.App.Controllers
{
    public class ConnectionController : Controller
    {
        private readonly INodeManager _nodeManager;

        public ConnectionController(INodeManager nodeManager)
        {
            _nodeManager = nodeManager;
        }
        
        public async Task<IActionResult> Connect()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("index", "node");

            var savedNodes = await _nodeManager.GetAllEndpointsAsync();

            var vm = new HomeConnectViewModel
            {
                NexusNodeEndpointIds = savedNodes.Select(x => x.Name).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Connect(string nodeId)
        {
            var endpoint = await _nodeManager.GetEndpointAsync(nodeId);

            var result = await _nodeManager.LoginAsync(HttpContext, endpoint);

            if (result.Succeeded)
                return RedirectToAction("index", "node");

            return View();
        }

        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("index", "node");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateConnectionViewModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("index", "node");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _nodeManager.CreateAsync(new NexusNodeEndpoint
                {
                    Name = model.Name,
                    Url = model.Url,
                    Username = model.Username,
                    Password = model.Password,
                    ApiSessions = model.ApiSessions,
                    IndexHeight = model.IndexHeight
                });
            }
            catch (Exception)
            {
                return View(model);
            }

            return RedirectToAction("connect", "connection");
        }
    }
}