using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record OrderBook
    {
        public List<Order> BuyOrders { get; set; }
        public List<Order> SellOrders { get; set; }
    }
}
