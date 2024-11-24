namespace EcommerceLib.Dtos
{
    public class RatingDto
    {
        public int Id { get; set; }
            public int ProductId { get; set; }
            public string? UserId { get; set; }
            public int Score { get; set; }
            public string? Comment { get; set; }
            public DateTime? CreatedAt { get; set; }
    }
}
