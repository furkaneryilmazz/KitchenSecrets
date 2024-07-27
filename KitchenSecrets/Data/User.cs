using System.ComponentModel.DataAnnotations;

namespace KitchenSecrets.Data
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
        public string? Image { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
