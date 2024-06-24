using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrderEntities
{
	public class OrderDetails
	{
		[Key]
		public string OrderDetailId { get; set; }
		public string OrderId { get; set; }
		public Order Order { get; set; }
		public string PlateName { get; set; }
		public int? Quantity { get; set; }
	}
}
