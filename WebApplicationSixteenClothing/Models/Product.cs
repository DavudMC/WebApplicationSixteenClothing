using WebApplicationSixteenClothing.Models.Common;

namespace WebApplicationSixteenClothing.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
