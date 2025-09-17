using HA.Product.Data;
using HA.Product.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HA.Product.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;

        public HomeController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var items = context.Items.ToList();
            return View(items);
        }

        public IActionResult Detail(int id)
        {
            var item = context.Items.FirstOrDefault(x => x.ID == id);
            if (item == null) return NotFound();

            return View(item);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
