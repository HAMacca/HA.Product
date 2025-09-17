using HA.Product.Data;
using HA.Product.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HA.Product.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        public IActionResult Index()
        {
            var items = context.Items.ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                if (item.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(env.WebRootPath, "images");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(item.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        item.ImageFile.CopyTo(fileStream);
                    }

                    item.urlImage = "/images/" + uniqueFileName;
                }
                context.Items.Add(item);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        public IActionResult Edit(int id)
        {
            var item = context.Items.FirstOrDefault(x => x.ID == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Item updatedItem)
        {
            var item = context.Items.FirstOrDefault(x => x.ID == id);
            if (item == null) return NotFound();

            if (ModelState.IsValid)
            {
                item.Name = updatedItem.Name;
                item.Description = updatedItem.Description;
                item.Price = updatedItem.Price;

                if (updatedItem.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(env.WebRootPath, "images");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid() + Path.GetExtension(updatedItem.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        updatedItem.ImageFile.CopyTo(fileStream);
                    }

                    item.urlImage = "/images/" + uniqueFileName;
                }
                context.Items.Update(item);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(updatedItem);

        }

        public IActionResult Delete(int id)
        {
            var item = context.Items.FirstOrDefault(x => x.ID == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = context.Items.FirstOrDefault(x => x.ID == id);
            if (item == null) return NotFound();

            if (!string.IsNullOrEmpty(item.urlImage))
            {
                string filePath = Path.Combine(env.WebRootPath, item.urlImage.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            context.Items.Remove(item);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
