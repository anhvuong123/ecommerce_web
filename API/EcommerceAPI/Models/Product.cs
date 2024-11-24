namespace EcommerceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public string? ImageUrl { get; set; }
        public string? Note { get; set; }

        public decimal? AverageRating { get; set; } // Avg Rating
        public int? RatingCount { get; set; } // Number of ratings voted

        // Danh sách các đánh giá
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}