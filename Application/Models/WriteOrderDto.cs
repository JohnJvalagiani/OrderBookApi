using Domain;

namespace Application.Models
{
    public record WriteOrderDto
    {
        public required string UserId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
