using KitchenSecrets.Data;

namespace KitchenSecrets.Models
{
    public static class Repository
    {

        private static readonly List<Product> _products = new();
        private static readonly List<Category> _category = new();

        public static List<Product> Products { get { return _products; } }

        public static void CreateProduct(Product product)
        {
            _products.Add(product);
        }

        public static void DeleteProduct(Product entity)
        {
            var entities = _products.FirstOrDefault(p => p.ProductId == entity.ProductId);
            if (entities != null)
            {
                _products.Remove(entities);
            }
        }
    }
}
