using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record OrderBookModel
    {
        public IEnumerable<ReadOrderDto> BuyOrders { get; set; }
        public IEnumerable<ReadOrderDto> SellOrders { get; set; }
    }
}
