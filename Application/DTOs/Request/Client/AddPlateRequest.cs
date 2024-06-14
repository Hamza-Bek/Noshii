using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Client
{
    public class AddPlateRequest
    {
        public string PlateId { get; set; }
        public string UserId { get; set; }
    }
}
