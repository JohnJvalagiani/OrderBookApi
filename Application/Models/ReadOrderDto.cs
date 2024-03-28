using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record ReadOrderDto
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
