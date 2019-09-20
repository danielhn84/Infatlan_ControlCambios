using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using ControlCambios.messages;

namespace ControlCambios.classes
{
    public class HttpService
    {
        public HttpService()
        {
        }
        public HttpResponseMessage postLogin(msgLogin vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "Login", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public msgGeneralesResponse getGenerales(String vTipo, String vParametro)
        {
            var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 };
            var response = client.DownloadString(string.Format("{0}{1}", ConfigurationManager.AppSettings["Server"], "Generales?tipo=" + vTipo + "&parametro=" + vParametro));
            var vCoverage = Newtonsoft.Json.JsonConvert.DeserializeObject<msgGeneralesResponse>(response);

            return (msgGeneralesResponse)vCoverage;
        }

        public msgCatEquiposQueryResponse getCatEquipos(String vTipo)
        {
            var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 };
            var response = client.DownloadString(string.Format("{0}{1}", ConfigurationManager.AppSettings["Server"], "CatalogoEquipos?tipo=" + vTipo ));
            var vCoverage = Newtonsoft.Json.JsonConvert.DeserializeObject<msgCatEquiposQueryResponse>(response);

            return (msgCatEquiposQueryResponse)vCoverage;
        }
        
        public msgCatSistemasQueryResponse getCatSistemas(String vTipo) 
        {
            var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 };
            var response = client.DownloadString(string.Format("{0}{1}", ConfigurationManager.AppSettings["Server"], "CatalogoSistemas?tipo=" + vTipo));
            var vCoverage = Newtonsoft.Json.JsonConvert.DeserializeObject<msgCatSistemasQueryResponse>(response);

            return (msgCatSistemasQueryResponse)vCoverage;
        }
        public msgCatSistemasQueryResponse getCatSistemas(String vTipo, String vParametro)
        {
            var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 };
            var response = client.DownloadString(string.Format("{0}{1}", ConfigurationManager.AppSettings["Server"], "CatalogoSistemas?tipo=" + vTipo + "&parametro=" + vParametro));
            var vCoverage = Newtonsoft.Json.JsonConvert.DeserializeObject<msgCatSistemasQueryResponse>(response);

            return (msgCatSistemasQueryResponse)vCoverage;
        }

        public HttpResponseMessage PostRelacionesCambios(msgRelacionesCambios vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "RelacionesCambios", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostRelacionesMantenimientos(msgRelacionesMantenimientos vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "RelacionesMantenimientos", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoUsuarios(msgInfoUsuarios vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoUsuarios", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;   
        }

        public HttpResponseMessage PostInfoCambios(msgInfoCambios vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoCambios", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoMantenimientos(msgInfoMantenimientos vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoMantenimientos", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoCalendarios(msgInfoCalendarios vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoCalendarios", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoCanales(msgInfoSistemas vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoCanales", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }
    
        public HttpResponseMessage PostInfoEquipos(msgInfoEquipos vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoEquipos", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoPersonal(msgInfoPersonal vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoPersonal", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoComunicaciones(msgInfoComunicaciones vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoComunicaciones", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoProcedimientos(msgInfoProcedimientos vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoProcedimientos", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoRollbacks(msgInfoRollbacks vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoRollbacks", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoPruebas(msgInfoPruebas vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoPruebas", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoAprobaciones(msgInfoAprobaciones vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoAprobaciones", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoCambiosCierre(msgInfoCambiosCierre vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoCambiosCierre", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostInfoArchivos(msgInfoArchivos vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                String vTestJson = Newtonsoft.Json.JsonConvert.SerializeObject(vDatos);
                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoArchivos", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }
        public HttpResponseMessage PostLogs(msgLogs vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                String vTestJson = Newtonsoft.Json.JsonConvert.SerializeObject(vDatos);
                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "Logs", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }
        public HttpResponseMessage PostCalendario(msgConsultasGenerales vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                String vTestJson = Newtonsoft.Json.JsonConvert.SerializeObject(vDatos);
                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "ConsultasGenerales", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostImplementadores(msgInfoImplementadores vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                String vTestJson = Newtonsoft.Json.JsonConvert.SerializeObject(vDatos);
                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "InfoImplementadores", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostEquipos(msgCatEquipos vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                String vTestJson = Newtonsoft.Json.JsonConvert.SerializeObject(vDatos);
                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "GeneralEquipos", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }

        public HttpResponseMessage PostSistemas(msgCatSistemas vDatos, ref String vJsonResult)
        {
            HttpResponseMessage vResponse = null;
            try
            {
                System.Net.Http.HttpClient client = new HttpClient();

                String vTestJson = Newtonsoft.Json.JsonConvert.SerializeObject(vDatos);
                var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vDatos));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                vResponse = client.PostAsync(ConfigurationManager.AppSettings["Server"] + "GeneralSistemas", httpContent).Result;

                var vContents = vResponse.Content.ReadAsStringAsync();
                vJsonResult = vContents.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return vResponse;
        }
    }
}