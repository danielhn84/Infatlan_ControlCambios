using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;

namespace ControlCambios.classes
{
    public class SmtpService
    {
        public SmtpService() {}

        public Boolean EnviarMensaje(String To, String Subject, String Body)
        {
            Boolean vRespuesta = false;
            try
            {
                MailMessage mail = new MailMessage(ConfigurationManager.AppSettings["SmtpFrom"], To);
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = ConfigurationManager.AppSettings["SmtpServer"];
                mail.Subject = Subject;
                mail.Body = Body;
                client.Send(mail);
                vRespuesta = true;
            }
            catch
            {
                throw;
            }
            return vRespuesta;
        }
    }
}