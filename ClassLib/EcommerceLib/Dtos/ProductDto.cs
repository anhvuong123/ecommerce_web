// Dtos/CategoryDto.cs
namespace EcommerceLib.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductImageUrl { get; set; }
        public decimal? ProductAverageRating { get; set; }
        public int? ProductRatingCount { get; set; }
        public CategoryDto Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<RatingDto> Ratings { get; set; } = new List<RatingDto>();
        public string? ProductNote { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
