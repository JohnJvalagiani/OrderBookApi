namespace OrderBook.API.Models.CommandModels
{
    public class PlaceSellOrderCommand
    {
        public string UserId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
