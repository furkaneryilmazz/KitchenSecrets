using System.ComponentModel.DataAnnotations;

namespace KitchenSecrets.Data
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Yemek Adı Zorunlu")]
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Material { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? Image { get; set; }
        public bool DealOfTheDay { get; set; }
        public bool TrendFood { get; set; }
        public bool IsActive { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; } = null!;

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public int? CategoryId { get; set; }
        public Category? Category { get; set; } = null!;
    }
}
