using EFCore.AppLib.CodeFirst;
using EFCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Controllers
{
    public class DataController : Controller
    {
        private readonly CodeFirstDbContext _context;

        public DataController(CodeFirstDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            AdresViewModel model = new AdresViewModel();

            model.Iller = _context.Iller.ToList();

            return View(model);
        }
    }
}
