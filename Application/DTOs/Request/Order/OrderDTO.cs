using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Order
{
    public class OrderDTO
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public bool? IsOrdered { get; set; }
    }
}
