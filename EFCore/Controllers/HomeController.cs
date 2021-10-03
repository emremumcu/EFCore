﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        [Route("/readme")]
        public IActionResult Readme() => View();
    }
}
