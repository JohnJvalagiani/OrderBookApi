using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomExceptions
{
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(int orderId)
            : base($"Order with ID '{orderId}' was not found.")
        {
            OrderId = orderId;
        }

        public int OrderId { get; } 
    }
}
