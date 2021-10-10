using EFCore.AppLib.CodeFirst;
using EFCore.AppLib.CodeFirst.Entities;
using EFCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            return View();
        }

        [HttpGet]
        public IActionResult Iller()
        {
            AdresViewModel model = new AdresViewModel();
            model.Iller = _context.Iller.ToList();
            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        // Bind the id to the action in the route: [controller]/[action]/{id} as ilkodu
        [Route("[controller]/[action]/{ilkodu}")]  
        public IActionResult Ilceler(string ilkodu)
        {
            // https://www.thecodebuzz.com/query-linq-expression-could-not-be-translated/

            AdresViewModel model = new AdresViewModel();
            model.Iller = _context.Iller.Where(i=>i.IlKodu==ilkodu).ToList();
            model.Ilceler = _context.Ilceler
                .Include(i=>i.Il)                
                .Where(i=>i.IlKodu==ilkodu)
                .AsEnumerable() // Force the evaluation in the client side and to prevent exception: Either rewrite the query in a form that can be translated, or switch to client evaluation explicitly by inserting a call to either AsEnumerable(). EFCore doesn’t understand the order by expression when it contains StringComparer.CurrentCulture (since current culture is client property NOT server property), and doesn’t translate it to the server. It throws this exception! 
                .OrderBy(i=>i.IlceAdi, StringComparer.CurrentCulture).ToList();
            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        [Route("[controller]/[action]/{ilcekodu}")]
        public IActionResult Sbbler(string ilcekodu)
        {
            AdresViewModel model = new AdresViewModel();

            model.Sbbler = _context.SemtBucakBeldeler
                .Where(s=>s.Ilce.IlceKodu==ilcekodu)
                .AsEnumerable() // Force the evaluation in the client side and to prevent exception: Either rewrite the query in a form that can be translated, or switch to client evaluation explicitly by inserting a call to either AsEnumerable(). EFCore doesn’t understand the order by expression when it contains StringComparer.CurrentCulture (since current culture is client property NOT server property), and doesn’t translate it to the server. It throws this exception! 
                .OrderBy(s=>s.SemtBucakBeldeAdi, StringComparer.CurrentCulture)
                .ToList();

            model.Ilceler = _context.Ilceler
                .Where(i => i.IlceKodu==ilcekodu)
                .Include(i => i.Il)
                .AsEnumerable() // Force the evaluation in the client side and to prevent exception: Either rewrite the query in a form that can be translated, or switch to client evaluation explicitly by inserting a call to either AsEnumerable(). EFCore doesn’t understand the order by expression when it contains StringComparer.CurrentCulture (since current culture is client property NOT server property), and doesn’t translate it to the server. It throws this exception! 
                .OrderBy(i => i.IlceAdi, StringComparer.CurrentCulture)
                .ToList();

            model.Iller = _context.Iller.Where(i => i.IlKodu == model.Ilceler.FirstOrDefault().IlceKodu).ToList();

            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        [Route("[controller]/[action]/{sbbkodu}")]
        public IActionResult Mahalleler(string sbbkodu)
        {
            AdresViewModel model = new AdresViewModel();

            model.Mahalleler = _context.Mahalleler
                .Where(m => m.SbbKodu == sbbkodu)
                .Include(m=>m.SemtBucakBelde)
                .Include(m=>m.SemtBucakBelde.Ilce)
                .Include(m => m.SemtBucakBelde.Ilce.Il)
                .AsEnumerable() // Force the evaluation in the client side and to prevent exception: Either rewrite the query in a form that can be translated, or switch to client evaluation explicitly by inserting a call to either AsEnumerable(). EFCore doesn’t understand the order by expression when it contains StringComparer.CurrentCulture (since current culture is client property NOT server property), and doesn’t translate it to the server. It throws this exception! 
                .OrderBy(m=>m.MahalleAdi)
                .ToList();

            return View(viewName: "Index", model: model);
        }
    }
}
