using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Net.Mime;

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
        PromotorReEnvio,
        bugs,
        ImplementadorRegreso,
        Comunicacion
    }
    public class SmtpService : Page
    {
        public SmtpService() {}

        public Boolean EnviarMensaje(String To, typeBody Body, String Usuario, String Cambio, String Nombre, String BodySecundario = null)
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
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") creado pendiente de autorización",
                            ConfigurationManager.AppSettings["Host"] + "/pages/customs/authorizations.aspx",
                            "Te informamos que el cambio ha sido creado con exito y esta a la espera de tu autorización para proceder con el proceso, " +
                            "para aprobar el cambio entra al aplicativo y ve a la sección de Autorizaciones." 
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.Supervisor:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") creado pendiente de revisión por QA",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx?busqueda=" + Nombre,
                            "Te informamos que el cambio ha sido autorizado y esta a la espera de tu revisión y certificación para proceder con el proceso, " +
                            "para certificar el cambio entra al aplicativo y ve a la sección de cambios."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.QA:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") creado, pendiente de autorización por el CAB Manager",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx?busqueda=" + Nombre,
                            "Te informamos que el cambio ha sido certificado por QA y esta a la espera de tu revisión y autorización para proceder con el proceso, " +
                            "para autorizar el cambio entra al aplicativo y ve a la sección de cambios."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.CAB:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") creado pendiente de implementación",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx?busqueda=" + Nombre,
                            "Te informamos que el cambio ha sido autorizado por el CAB Manager y esta a la espera de tu implementación en las fechas programadas, " +
                            "para implementar el cambio entra al aplicativo y ve a la sección de cambios."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.Implementacion:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") esta implementado y pendiente de revisión",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx?busqueda=" + Nombre,
                            "Te informamos que el cambio ha sido implementado y esta a la espera de la revisión y certificación por parte del implementador " +
                            "para revisar el cambio entra al aplicativo y ve a la sección de cambios."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.QARevision:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") esta revisado por el implementador y pendiente de tu revision para el cierre",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx?busqueda=" + Nombre,
                            "Te informamos que el cambio ha sido revisado y certificado para finalizar el proceso, " +
                            "para revisar el cambio y darlo por finalizado entra al aplicativo y ve a la sección de cambios."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.SupervisorCierre:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") se ha finalizado",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx?busqueda=" + Nombre,
                            "Te informamos que el cambio se ha finalizado, " +
                            "para revisar el cambio entra al aplicativo y ve a la sección de cambios."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.PromotorRegreso:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") se te ha devuelto",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx?busqueda=" + Nombre,
                            "Te informamos que el cambio no se aprobo, QA se comunicara contigo en breve, " +
                            "para revisar el cambio entra al aplicativo y ve a la sección de cambios."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.PromotorReEnvio:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") esta implementado y pendiente de revisión",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx?busqueda=" + Nombre,
                            "Te informamos que el cambio ha sido implementado y esta a la espera de la revisión y certificación del implementador, " +
                            "para revisar el cambio entra al aplicativo y ve a la sección de cambios."
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.bugs:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Tipor de error: " + Cambio ,
                            ConfigurationManager.AppSettings["Host"] ,
                            "Descripción del error: " + Nombre
                            ), Server.MapPath("/images/logo.png")));
                        break;
                    case typeBody.ImplementadorRegreso:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") Se ha regresado para que ingreses de nuevo tus concluciones.",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx?busqueda=" + Nombre,
                            "Te informamos que el cambio ha sido implementado y esta a la espera de la certificación del implementador, " +
                            "para revisar el cambio entra al aplicativo y ve a la sección de cambios."
                            ), Server.MapPath("/images/logo.png")));
                        break;

                    case typeBody.Comunicacion:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            Usuario,
                            "Cambio #" + Cambio + " (" + Nombre + ") Se ha ejecutado una accion.",
                            ConfigurationManager.AppSettings["Host"] + "/pages/services/search.aspx?busqueda=" + Nombre,
                            "Te informamos que el cambio sigue en proceso, actualmentemente se encuentra en estado <b>" + BodySecundario + "</b>"
                            ), Server.MapPath("/images/logo.png")));
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
            catch (Exception Ex)
            {
                throw;
            }
            return vRespuesta;
        }
        private AlternateView CreateHtmlMessage(string message, string logoPath)
        {
            var inline = new LinkedResource(logoPath, "image/png");
            inline.ContentId = "companyLogo";

            var alternateView = AlternateView.CreateAlternateViewFromString(
                                    message,
                                    Encoding.UTF8,
                                    MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(inline);

            return alternateView;
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