using KitchenSecrets.Data;
using KitchenSecrets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KitchenSecrets.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string category)
        {
            var products = await _context.Products.ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;
                products = products.Where(s => s.Name.ToLower().Contains(searchString)).ToList();
            }
            if (!String.IsNullOrEmpty(category) && category != "0")
            {
                products = products.Where(s => s.CategoryId == int.Parse(category)).ToList();
            }
            //ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");

            var model = new ProductViewModel
            {
                Products = products,
                Categories = await _context.Categories.ToListAsync(),
                SelectedCategory = category
            };

            return View(model);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var products = await _context.Products.Include(y => y.Category).FirstOrDefaultAsync(x => x.ProductId == id);
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            var allowenExtensions = new[] { ".jpg", ".png", ".jpeg" };
            if (imageFile != null)
            {
                var extensions = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowenExtensions.Contains(extensions))
                {
                    ModelState.AddModelError("", "Geçerli bir resim giriniz!");
                }
                else
                {
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extensions}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                    try
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        product.Image = randomFileName;
                    }
                    catch
                    {

                        ModelState.AddModelError("", "Resim yüklenirken bir hata oluştu!");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Resim zorunludur!");
            }

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(product);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (entity == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(entity);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile)
        {
            var allowenExtensions = new[] { ".jpg", ".png", ".jpeg" };
            if (imageFile != null)
            {
                var extensions = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowenExtensions.Contains(extensions))
                {
                    ModelState.AddModelError("", "Geçerli bir resim giriniz!");
                }
                else
                {
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extensions}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                    try
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        model.Image = randomFileName;
                    }
                    catch
                    {

                        ModelState.AddModelError("", "Resim yüklenirken bir hata oluştu!");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Resim zorunludur!");
            }

            var entity = _context.Products.FirstOrDefault(p => p.ProductId == model.ProductId);
            if (entity == null)
            {
                return NotFound();
            }
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Title = model.Title;
                entity.Material = model.Material;
                entity.Description = model.Description;
                entity.ShortDescription = model.ShortDescription;
                entity.Image = model.Image;
                entity.DealOfTheDay = model.DealOfTheDay;
                entity.TrendFood = model.TrendFood;
                entity.CategoryId = model.CategoryId;
            }
            if (ModelState.IsValid)
            {
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (entity == null)
            {
                return NotFound();
            }

            return View("DeleteConfirm", entity);
        }
        [HttpPost]
        public IActionResult Delete(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            var entity = _context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (entity == null)
            {
                return NotFound();
            }
            _context.Products.Remove(entity);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
