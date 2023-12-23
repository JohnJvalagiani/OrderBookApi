using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OrderBook
    {
        public List<Order> BuyOrders { get; set; }
        public List<Order> SellOrders { get; set; }
    }
}
