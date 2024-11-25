namespace EcommerceLib.Dtos
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal? ProductPrice { get; set; }
        public int CategoryId { get; set; }
        public string? ProductNote { get; set; }
        public string? ProductImageUrl { get; set; }
    }
}
