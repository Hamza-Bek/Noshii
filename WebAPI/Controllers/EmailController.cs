using Application.Interfaces;
using Domain.Models.EmailEntities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmailController : ControllerBase
	{
		private readonly IEmailSenderRepository _emailSenderRepository;
		public EmailController(IEmailSenderRepository emailSenderRepository)
		{
			_emailSenderRepository = emailSenderRepository;
		}

		[HttpPost("Send")]
		public async Task<IActionResult> Send(MailRequest request, string userId)
		{
			try
			{
				await _emailSenderRepository.SendEmailAsync(request, userId);
				return Ok();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

	}
}
