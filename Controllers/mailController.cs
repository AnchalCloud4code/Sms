using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGridEmailSender.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SendGridEmailSender.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class mailController : ControllerBase
	{
		private readonly ISendGridClient _sendGridClient;
		private readonly IConfiguration _configuration;
		public mailController(
			ISendGridClient sendGridClient,
			IConfiguration configuration)
		{
			_sendGridClient = sendGridClient;
			_configuration = configuration;
		}
		[HttpGet]
		[Route("Text-Mail")]
		public async Task<IActionResult> SendPlainTextEmail(string toEmail)
		{
			string fromEmail = _configuration.GetSection("SendGridEmailSettings")
			.GetValue<string>("FromEmail");

			string fromName = _configuration.GetSection("SendGridEmailSettings")
			.GetValue<string>("FromName");

			var msg = new SendGridMessage()
			{
				From = new EmailAddress(fromEmail, fromName),
				Subject = "Send My New Mail",
				PlainTextContent = "cloud4code It solutions Pvt ltd"
			};
			msg.AddTo(toEmail);
			var response = await _sendGridClient.SendEmailAsync(msg);
			string message = response.IsSuccessStatusCode ? "Email Success Send.." :
			"Sending Unsuccessful..";
			return Ok(message);
		}

		[HttpPost]
		[Route("send-html-mail")]
		public async Task<IActionResult> SendHtmlEmail(HtmlDEmail heroEmail)
		{
			string fromEmail = _configuration.GetSection("SendGridEmailSettings")
			.GetValue<string>("FromEmail");

			string fromName = _configuration.GetSection("SendGridEmailSettings")
			.GetValue<string>("FromName");

			var msg = new SendGridMessage()
			{
				From = new EmailAddress(fromEmail, fromName),
				Subject = "HTML TEMPLATE EMAIL(ANCHAL)",
				
			};
			string FilePathname = Directory.GetCurrentDirectory() + "\\Template\\sendTemplate.html";
			string EmailTemplateText = System.IO.File.ReadAllText(FilePathname);
			EmailTemplateText = Convert.ToString((EmailTemplateText, DateTime.Now.Date.ToShortDateString()));
			//msg.HtmlContent = EmailTemplateText;
			msg.HtmlContent = EmailTemplateText.Replace("name", heroEmail.Name);


			msg.AddTo(heroEmail.ToEmail);
			var response = await _sendGridClient.SendEmailAsync(msg);
			string message = response.IsSuccessStatusCode ? "Email Send Successfully" :
			"Email Sending Failed";
			return Ok(message);
		}
	}
}