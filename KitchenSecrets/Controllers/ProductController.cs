using KitchenSecrets.Data;
using KitchenSecrets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace KitchenSecrets.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(string searchString, string category)
        {
            var products = await _context.Products.Include(a=>a.User).Where(x=> x.IsActive).ToListAsync();
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

        public async Task<IActionResult> ProductUserIndex()
        {
            var userIdString = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            if(int.TryParse(userIdString, out int userId))
            {
                var products = await _context.Products.Where(p => p.UserId == userId).ToListAsync();
                return View(products);
            }
            return RedirectToAction("Login", "Login");
        }



        public async Task<IActionResult> Detail(int? id)
        {
            var products = await _context.Products.Include(y => y.Category).Include(a => a.Comments).Include(z => z.Comments).ThenInclude(x=>x.User).FirstOrDefaultAsync(x => x.ProductId == id);
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            // Kullanıcının giriş yapıp yapmadığını kontrol et
            var userIdString = HttpContext.Session.GetString("UserId");
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Login");
            }

            product.UserId = userId;

            // Resim uzantılarını kontrol et
            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
            if (imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Geçerli bir resim formatı seçiniz!");
                }
                else
                {
                    var randomFileName = $"{Guid.NewGuid()}{extension}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                    try
                    {
                        // Resmi dosya sistemine kaydet
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

            // Kategori verilerini ViewBag'e ekle ve View'ı döndür
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(product);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Index");
            }

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
            if (HttpContext.Session.GetString("UserId") ==null)
            {
                return RedirectToAction("Index");
            }

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
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Index");
            }

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
