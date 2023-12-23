namespace OrderBook.API.Models.CommandModels
{
    public class UpdateOrderCommand
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
