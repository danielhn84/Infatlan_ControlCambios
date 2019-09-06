using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.IO;
using System.Web.UI;

namespace ControlCambios.classes
{
    public enum typeBody
    {
        Supervisor,
        Implementacion,
        QA,
        CAB,
        Promotor,
        QARevision,
        SupervisorCierre,
        PromotorRegreso,
        PromotorReEnvio
    }
    public class SmtpService : Page
    {
        public SmtpService() {}

        public Boolean EnviarMensaje(String To, typeBody Body, String Usuario, String Cambio, String Nombre)
        {
            Boolean vRespuesta = false;
            try
            {
                MailMessage mail = new MailMessage("Control de Cambios<" + ConfigurationManager.AppSettings["SmtpFrom"] + ">", To);
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = ConfigurationManager.AppSettings["SmtpServer"];
                mail.Subject = "Control de Cambios - Información de Cambio #" + Cambio;
                mail.IsBodyHtml = true;

                switch (Body)
                {
                    case typeBody.Promotor:
                        mail.Body = PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") creado pendiente de autorización",
                            ConfigurationManager.AppSettings["Host"] + "/pages/customs/authorizations.aspx",
                            "Te informamos que el cambio ha sido creado con exito y esta a la espera de tu autorización para proceder con el proceso, " +
                            "para aprobar el cambio entra al aplicativo y ve a la sección de Autorizaciones." 
                            );
                        break;
                    case typeBody.Supervisor:
                        mail.Body = PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") creado pendiente de revisión por QA",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx",
                            "Te informamos que el cambio ha sido autorizado y esta a la espera de tu revisión y certificación para proceder con el proceso, " +
                            "para certificar el cambio entra al aplicativo y ve a la sección de cambios."
                            );
                        break;
                    case typeBody.QA:
                        mail.Body = PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") creado pendiente de autorización por los CAB Manager",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx",
                            "Te informamos que el cambio ha sido certificado por QA y esta a la espera de tu revisión y autorización para proceder con el proceso, " +
                            "para autorizar el cambio entra al aplicativo y ve a la sección de cambios."
                            );
                        break;
                    case typeBody.CAB:
                        mail.Body = PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") creado pendiente de implementación",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx",
                            "Te informamos que el cambio ha sido autorizado por el CAB Manager y esta a la espera de tu implementación en las fechas programadas, " +
                            "para implementar el cambio entra al aplicativo y ve a la sección de cambios."
                            );
                        break;
                    case typeBody.Implementacion:
                        mail.Body = PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") esta implementado y pendiente de revisión por parte de QA",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx",
                            "Te informamos que el cambio ha sido implementado y esta a la espera de tu revisión y certificación, " +
                            "para revisar el cambio entra al aplicativo y ve a la sección de cambios."
                            );
                        break;
                    case typeBody.QARevision:
                        mail.Body = PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") esta revisado por QA para su cierre",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx",
                            "Te informamos que el cambio ha sido revisado y certificado para finalizar el proceso, " +
                            "para revisar el cambio y darlo por finalizado entra al aplicativo y ve a la sección de cambios."
                            );
                        break;
                    case typeBody.SupervisorCierre:
                        mail.Body = PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") se ha finalizado",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx",
                            "Te informamos que el cambio se ha finalizado, " +
                            "para revisar el cambio entra al aplicativo y ve a la sección de cambios."
                            );
                        break;
                    case typeBody.PromotorRegreso:
                        mail.Body = PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") se te ha devuelto",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx",
                            "Te informamos que el cambio no se aprobo, QA se comunicara contigo en breve, " +
                            "para revisar el cambio entra al aplicativo y ve a la sección de cambios."
                            );
                        break;
                }
                client.Send(mail);
                vRespuesta = true;
            }
            catch (System.Net.Mail.SmtpException Ex)
            {
                String vError = Ex.Message;
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return vRespuesta;
        }

        public string PopulateBody(string vNombre, string vTitulo, string vUrl, string vDescripcion)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("/pages/mail/TemplateMail.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Host}", ConfigurationManager.AppSettings["Host"]);
            body = body.Replace("{Nombre}", vNombre);
            body = body.Replace("{Titulo}", vTitulo);
            body = body.Replace("{Url}", vUrl);
            body = body.Replace("{Descripcion}", vDescripcion);
            return body;
        }
    }
}