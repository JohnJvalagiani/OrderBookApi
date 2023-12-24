using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class ReadOrderDto
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
