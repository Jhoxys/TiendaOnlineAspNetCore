using System.Net;
using System.Net.Mail;

namespace CapaAdmin.Service
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string mails, string pw, string email, string subject, string message)
		{


			// Set up the SMTP client
			SmtpClient smtp = new SmtpClient("smtp.gmail.com");
			smtp.Port = 587;
			smtp.EnableSsl = true;
			smtp.UseDefaultCredentials = false;
			smtp.Credentials = new NetworkCredential(mails, pw);
			// Send the email
		

		
			return smtp.SendMailAsync(
		   new MailMessage(from: mails,
							to: email,
						   subject,
						   message
						   ));



		}



	}
}







/*

namespace CapaAdmin.Service
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string message)
		{

			var mail = "proximab02@gmail.com";
			var pw = "jhoxysanimeid01";

			var client = new SmtpClient("smtp-mail.gmail.com", 465)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(mail, pw)
			};

			return client.SendMailAsync(
				   new MailMessage(from: mail,
				                    to: email,
				                   subject, 
								   message
								   ));



		}
	}

	//	public static void SendEmail(string senderName, string senderEmail, string toName, string toEmail,
	//		string subject, string textContent)
	//	{
	//		var apiInstance = new TransactionalEmailsApi();
	//		SendSmtpEmailSender Email = new SendSmtpEmailSender(senderName, senderEmail);
	//		SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(toEmail, toName);
	//		List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
	//		To.Add(smtpEmailTo);

	//		//string HtmlContent = "<html><body><h1>This is my first transactional email {{params.parameter}}</h1></body></html>";


	//		try
	//		{  
	//			//var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, null, textContent, subject, ReplyTo, Attachment, Headers, TemplateId, Params, messageVersiopns, Tags);
	//			var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, null, textContent, subject);
	//			// aqui html
	//			CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
	//			Console.WriteLine("Email Sender Ok: \n"+ result.ToJson());
	//		}
	//		catch (Exception e)
	//		{
	//			Console.WriteLine("Email Sender Failure  \n" + e.Message);
	//		}
	//	}

	//}
}
*/