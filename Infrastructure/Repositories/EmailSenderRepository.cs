using Application.Interfaces;
using Domain.Models.EmailEntities;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class EmailSenderRepository : IEmailSenderRepository
	{

		private readonly MailSettings _mailSettings;
		private readonly AppDbContext _context;
		public EmailSenderRepository(IOptions<MailSettings> options, AppDbContext context)
		{
			_mailSettings = options.Value;
			_context = context;
		}



		public async Task SendEmailAsync(MailRequest mailRequest, string userId)
		{
			var user = await _context.Users.FindAsync(userId);
			
			var getUserOrders = await _context.Orders
				  .Where(o => o.UserId == user.Id)
				  .Include(o => o.OrderMaker)
				  .Include(o => o.GetOrderDetails)
		    	   .Include(o => o.Location)
				  .ToListAsync();

			var lastOrder = getUserOrders.OrderByDescending(o => o.OrderDate).FirstOrDefault();

			if (lastOrder == null)
			{
				throw new Exception("No orders found for the user.");
			}

			var email = new MimeMessage();
			email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
			email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
			email.Subject = mailRequest.Subject;
			var builder = new BodyBuilder();
			
			builder.HtmlBody = $@"
        <html>
        <body>
            <p>Hi {user.Name},</p>
            <p>Thank you for ordering with NOSHII! We have successfully received your order. Below are the details:</p>
            <p><strong>Order Number:</strong> #{lastOrder.OrderNumber}</p>
            <p><strong>Order Summary:</strong></p>
            <ul>
                {string.Join("", lastOrder.GetOrderDetails.Select(d => $"<li>{d.Quantity} x {d.PlateName}</li>"))}
            </ul>
            <p><strong>Total Amount:</strong> ${lastOrder.OrderTotal}</p>
            <p><strong>Delivery Address:</strong></p>
            <p><strong>Phone Number:</strong> {lastOrder.Location.PhoneNumber}</p>
			<p><strong>Street :</strong> {lastOrder.Location.Street}</p>
			<p><strong>Building:</strong> {lastOrder.Location.Building}</p>
			<p><strong>Floor:</strong> {lastOrder.Location.Floor}</p>
            <p><strong>Estimated Delivery Time:</strong> 50 minutes</p>
            <p>If you have any questions or need to make changes to your order, please contact us at noshiirestaurant@gmai.com.</p>
            <p>Thank you for choosing NOSHII!</p>
            <p>Best regards,</p>
            <p>NOSHII.</p>
            <p>+201123452806</p>
            <p><a href='[www.hamzabek.com]'>[noshii.com]</a></p>
        </body>
        </html>"; ;
			email.Body = builder.ToMessageBody();

			try 
			{
				using var smtp = new MailKit.Net.Smtp.SmtpClient();
				smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
				smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
				await smtp.SendAsync(email);
				smtp.Disconnect(true);
			}
			catch (Exception ex)
			{
				// Log the exception details
				Console.WriteLine($"An error occurred: {ex.Message}");
				throw;
			}

		}
	}
}
