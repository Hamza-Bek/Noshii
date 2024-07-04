using Domain.Models.EmailEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IEmailSenderRepository
	{
		Task SendEmailAsync(MailRequest mailRequest, string userId);
		
	}
}
