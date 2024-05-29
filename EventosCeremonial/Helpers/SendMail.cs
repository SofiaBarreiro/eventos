using EventosCeremonial.IRepository;
using MimeKit;
using System.Threading.Tasks;
namespace EventosCeremonial.Helpers
{
    public class SendMail : IEmailSender
    {


        /// <summary>Sends the email asynchronous.
        /// Envío de correos de identity. Llama a la función conectarse de funciones email.</summary>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            var root = FuncionesBasicas.getAppSettings();

            //emailMessage.From.Add(new MailboxAddress(root.GetSection("SmtpClient")["UserName"], root.GetSection("SmtpClient")["Address"]));
            emailMessage.From.Add(new MailboxAddress("Ceremonial y Relaciones Institucionales", root.GetSection("SmtpClient")["Address"]));


            emailMessage.To.Add(new MailboxAddress(email, email));

            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;

            var multipart = new Multipart("mixed");
            multipart.Add(bodyBuilder.ToMessageBody());
            emailMessage.Body = multipart;
            await FuncionesEmail.Conectarse(emailMessage);

          
        }

    }
}
