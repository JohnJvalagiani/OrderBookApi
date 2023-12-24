using Entities;

namespace OrderBook.API.DTOs
{
    public class WriteOrderDto
    {
        public string UserId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
