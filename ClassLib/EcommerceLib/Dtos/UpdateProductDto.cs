namespace EcommerceLib.Dtos
{
    public class UpdateProductDto
    {
        public string ProductName { get; set; }  // Tương ứng với [Name]
        public string? ProductDescription { get; set; }  // Tương ứng với [Description]
        public decimal? ProductPrice { get; set; }  // Tương ứng với [Price]
        public int CategoryId { get; set; }  // Tương ứng với [CategoryId]
        public string? ProductNote { get; set; }  // Tương ứng với [Note]
        public decimal? ProductAverageRating { get; set; }  // Tương ứng với [AverageRating]
        public int? ProductRatingCount { get; set; }  // Tương ứng với [RatingCount]
        public string? ProductImageUrl { get; set; }  // Tương ứng với [ImageUrl]
    }
}
