using BookStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;

namespace BookStore.Domain.Concrete
{

    public class EmailSettings
    {
        public string MailToAddress = "order@xbookstore.com";// Book Store email
        public string MailFrom = "any@gmail.com";// customer email;
        public bool UseSsl = true; //SSL= secure socket layer
        public string Username = "rder@xbookstore.com";// Book Store email
        public string Password = "Password";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"C:\Order_BookStore_Emails"; // root of attachment
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSetting;
        public EmailOrderProcessor(EmailSettings setting)
        {
            emailSetting = setting;
        }
        public void ProcessOrder(Cart cart, ShippingDetail shippingDetail)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSetting.UseSsl;
                smtpClient.Host = emailSetting.ServerName;
                smtpClient.Port = emailSetting.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSetting.Username, emailSetting.Password);
                if (emailSetting.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSetting.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("----------")
                    .AppendLine("Books:");
                foreach (var line in cart.lines)
                {
                    var subTotal = line.Book.Price * line.Quantity;
                    body.AppendFormat("{0} X {1} (subtotal: {2:c})", line.Quantity, line.Book.Title, subTotal);
                }
                body.AppendFormat("Total order value:{0:c} \n", cart.ComupteTotalValue())
                    .AppendLine("----------")
                    .AppendLine("Shipp to")
                    .AppendLine(shippingDetail.Name)
                    .AppendLine(shippingDetail.Line1)
                    .AppendLine(shippingDetail.Line2)
                    .AppendLine(shippingDetail.City)
                    .AppendLine(shippingDetail.State)
                    .AppendLine(shippingDetail.Country)
                    .AppendLine("----------")
                    .AppendFormat("Gift wrap: {0}", shippingDetail.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(
                                          emailSetting.MailFrom,
                                          emailSetting.MailToAddress,
                                          "New order submitted",
                                          body.ToString());
                if (emailSetting.WriteAsFile)
                    mailMessage.BodyEncoding = Encoding.ASCII;
                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch(Exception ex)
                {
                    Debug.Print(ex.Message);
                }
            }
        }
    }

}
