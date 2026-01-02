using HMS.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace HMS.Helpers
{
    public class EmailHelper
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailHelper(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public bool SendEmailPasswordReset(User user, string link)
        {
            var message = new MimeMessage();
            //от кого отправляем и заголовок
            message.From.Add(new MailboxAddress("Hotel Management System", _emailConfig.From));
            //кому отправляем
            message.To.Add(new MailboxAddress(user.FirstName + " " + user.LastName, user.Email));
            //тема письма
            message.Subject = "Password reset on the Hotel Management System site";
            //тело письма
            message.Body = new TextPart("html")
            {
                Text = link,
            };
            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                //Указываем smtp сервер почты и порт
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, false);
                //Указываем свой Email адрес и пароль приложения
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                try
                {
                    client.Send(message);
                    return true;
                }
                catch (Exception)
                {

                    //Logging information
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
            return false;
        }

        public bool SendEmailCheck(User user, string link)
        {
            var message = new MimeMessage();
            //от кого отправляем и заголовок
            message.From.Add(new MailboxAddress("Hotel Management System", _emailConfig.From));
            //кому отправляем
            message.To.Add(new MailboxAddress(user.FirstName + " " + user.LastName, user.Email));
            //тема письма
            message.Subject = "Email confirmation on the Hotel Management System site";
            //тело письма
            message.Body = new TextPart("html")
            {
                Text = link,
            };
            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                //Указываем smtp сервер почты и порт
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, false);
                //Указываем свой Email адрес и пароль приложения
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                try
                {
                    client.Send(message);
                    return true;
                }
                catch (Exception)
                {

                    //Logging information
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
            return false;
        }

        public bool SendEmailWithNumbers(User user, string numbers)
        {
            var message = new MimeMessage();
            //от кого отправляем и заголовок
            message.From.Add(new MailboxAddress("Hotel Management System", _emailConfig.From));
            //кому отправляем
            message.To.Add(new MailboxAddress(user.FirstName + " " + user.LastName, user.Email));
            //тема письма
            message.Subject = "Numbers for two-factor authentication on the Hotel Management System site";
            //тело письма
            message.Body = new TextPart("html")
            {
                Text = $"Your one-time login password is: <h3>{numbers}</h3>.",
            };
            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                //Указываем smtp сервер почты и порт
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, false);
                //Указываем свой Email адрес и пароль приложения
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                try
                {
                    client.Send(message);
                    return true;
                }
                catch (Exception)
                {

                    //Logging information
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
            return false;
        }
    }
}
