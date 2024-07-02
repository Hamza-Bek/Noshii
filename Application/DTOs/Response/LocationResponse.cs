using Application.DTOs.Request.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public record LocationResponse(bool Flag = false, string Message = null!, string userName = null!, LocationDTO location = null!);
}
