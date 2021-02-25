using Chama.FernandoGJose.Domain.Share.Interfaces.Emails;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Email
{
    public class CourseSignUpProcessedWithErrorEmail : ISendEmail
    {
        public string TypeEmail => "CourseSignUpProcessedWithError";

        public async Task Send(string subject, string email, string name, Dictionary<string, string> parameter)
        {
            // Send e-mail with SendGrid (refactor)
            var sendGridClient = new SendGridClient("API_KEY");
            var from = new EmailAddress("from email", "from user");
            var to = new EmailAddress(email, name);
            var plainContent = "CourseSignUpProcessedWithSuccessEmail";
            var htmlContent = "<h1>Error</h1>";
            var mailMessage = MailHelper.CreateSingleEmail(from, to, subject, plainContent, htmlContent);
            await sendGridClient.SendEmailAsync(mailMessage);
        }
    }
}
