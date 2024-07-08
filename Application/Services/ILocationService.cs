using Application.DTOs.Request.Order;
using Application.DTOs.Response;
using Domain.Models.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface ILocationService
	{
		Task <IEnumerable<LocationDTO>> GetLocation(string userId);
		Task<LocationResponse> AddLocation(LocationDTO model);
		Task<LocationResponse> UpdateLocation(LocationDTO model);
	}
}
