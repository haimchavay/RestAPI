using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BLL.Versions.V1.Helpers
{
    public class SmptClientWrapper
    {
        public void SendMail(string receivermail, string subject, string body)
        {
            SmtpClient client = new SmtpClient()
            {
                //Host = "smtp.gmail.com",
                Host = "hgws23.win.hostgator.com",
                Port = 587,
                //Port = 465,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    //UserName = "getcouponticketnoreply@gmail.com",
                    UserName = "fpnoreply@getcouponticket.com",
                    //Password = "dawxsedvajnwlpbi"
                    Password = "^Ztja718"
                }
            };

            //MailAddress fromEmail = new MailAddress("getcouponticketnoreply@gmail.com", "sender email");
            MailAddress fromEmail = new MailAddress("fpnoreply@getcouponticket.com", "sender email");
            MailAddress toEmail = new MailAddress(receivermail, "reciver email");
            MailMessage message = new MailMessage()
            {
                From = fromEmail,
                Subject = subject,
                Body = body
            };
            message.To.Add(toEmail);

            // TODO : Create Logger object that logs to server
            try
            {
                client.Send(message);
                //Console.WriteLine("Send Successfully");
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Send email error : " + ex.Message);
                throw ex;
            }
        }
    }
}
