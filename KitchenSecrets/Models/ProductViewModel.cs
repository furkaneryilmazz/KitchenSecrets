using KitchenSecrets.Data;

namespace KitchenSecrets.Models
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; } = null!;
        public List<Category> Categories { get; set; } = null!;
        public string? SelectedCategory { get; set; }
    }
}
