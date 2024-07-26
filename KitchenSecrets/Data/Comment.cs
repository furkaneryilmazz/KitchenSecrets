using System.ComponentModel.DataAnnotations;

namespace KitchenSecrets.Data
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string? Text { get; set; }
        public DateTime PublishedOn { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; } = null!;
        public int UserId { get; set; }
        public User? User { get; set; } = null!;
    }
}
