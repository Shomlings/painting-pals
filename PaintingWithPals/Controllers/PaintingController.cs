using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaintingWithPals.Models;

namespace PaintingWithPals.Controllers
{
    public class PaintingController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public PaintingController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("/room/{code}")]
        public IActionResult Index([FromRoute]string code)
        {
            return View(model: code);
        }
    }
}
