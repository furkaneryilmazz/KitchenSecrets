using KitchenSecrets.Data;
using Microsoft.AspNetCore.Mvc;

namespace KitchenSecrets.ViewComponents.DefaultComponents
{
    public class _DefaultSliderComponentPartial : ViewComponent
    {
        private readonly DataContext _context;

        public _DefaultSliderComponentPartial(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
