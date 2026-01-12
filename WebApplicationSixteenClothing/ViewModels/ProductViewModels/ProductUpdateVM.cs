using System.ComponentModel.DataAnnotations;

namespace WebApplicationSixteenClothing.ViewModels.ProductViewModels
{
    public class ProductUpdateVM
    {
        [Required, MaxLength(100), MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [Required, Range(0, 100000000)]
        public decimal Price { get; set; }
        [Required, MaxLength(1024), MinLength(3)]
        public string Description { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        [Required, Range(0, 5)]
        public decimal Rating { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
