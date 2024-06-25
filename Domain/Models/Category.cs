using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class Category
	{
        public required string CategoryId { get; set; }
        public required string CategoryTag { get; set; }
    }
}
