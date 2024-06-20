using Domain.Models;
using Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Order
{
    public class CartDTO
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
 
    }
}
