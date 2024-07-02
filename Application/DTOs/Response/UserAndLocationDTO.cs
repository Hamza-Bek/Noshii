using Application.DTOs.Request.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class UserAndLocationDTO
    {
        public string UserName { get; set; }
        public LocationDTO Location { get; set; }
    }
}
