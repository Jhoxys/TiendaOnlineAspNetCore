namespace CapaAdmin.Service
{
	public interface IEmailSender
	{

		Task SendEmailAsync(string mails, string pw, string email, string subject, string message );
	}

}
