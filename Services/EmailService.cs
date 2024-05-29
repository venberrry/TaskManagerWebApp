using System.Net.Mail;
using System.Threading.Tasks;
// Вообще не уверена что работает, но я не нашла бесплатных SMTP-серверов
public class EmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var mail = new MailMessage("kakaya-to-tam-pochta@goomail.com", toEmail, subject, body);
        using (var smtpClient = new SmtpClient("kakoi-to-tam-smtp-server"))
        {
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential("kakaya-to-tam-pochta@goomail.com", "kakio-to-tam-password");
            smtpClient.EnableSsl = true;
            await smtpClient.SendMailAsync(mail);
        }
    }
}
