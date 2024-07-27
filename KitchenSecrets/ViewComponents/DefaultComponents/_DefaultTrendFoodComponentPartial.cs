using KitchenSecrets.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KitchenSecrets.ViewComponents.DefaultComponents
{
    public class _DefaultTrendFoodComponentPartial:ViewComponent
    {
        private readonly DataContext _context;

        public _DefaultTrendFoodComponentPartial(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _context.Products.Where(x=> x.TrendFood).OrderByDescending(y=>y.ProductId).Take(3).ToListAsync();
            return View(products);
        }
    }
}
