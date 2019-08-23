using ControlCambios.messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ControlCambios.classes
{
    public class Logs
    {
        msgLoginResponse vConfigurations = null;
        public Logs() { }

        public Boolean postLog(String vLugar, String vMensaje, String vUsuario)
        {
            Boolean vResultado = false;
            try
            {
                HttpService vConector = new HttpService();
                msgLogs vLogRequest = new msgLogs()
                {
                    tipo = "1",
                    lugar = vLugar,
                    mensaje = vMensaje,
                    usuario = vUsuario
                };

                String vResponseLogs = "";
                HttpResponseMessage vHttpResponseLogs = vConector.PostLogs(vLogRequest, ref vResponseLogs);
                if (vHttpResponseLogs.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vLogsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseLogs);
                    if (vLogsResponse.updateCount1.Equals("1"))
                    {
                        vResultado = true;
                    }
                }
            }
            catch { }
            return vResultado;
        }

        public DataTable ObtenerLogs()
        {
            DataTable vDatos = new DataTable();
            try
            {

                vDatos.Columns.Add("idLog");
                vDatos.Columns.Add("lugar");
                vDatos.Columns.Add("mensaje");
                vDatos.Columns.Add("fechaCreacion");
                vDatos.Columns.Add("usuario");

                HttpService vConector = new HttpService();
                msgLogs vLogRequest = new msgLogs()
                {
                    tipo = "2"
                };

                String vResponseLogs = "";
                HttpResponseMessage vHttpResponseLogs = vConector.PostLogs(vLogRequest, ref vResponseLogs);
                if (vHttpResponseLogs.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgLogsQueryResponse vLogsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgLogsQueryResponse>(vResponseLogs);
                    if (vLogsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgLogsQueryResponseItem item in vLogsResponse.resultSet1)
                        {
                            vDatos.Rows.Add(
                                item.idLog,
                                item.lugar,
                                item.mensaje,
                                item.fechaCreacion,
                                item.usuario
                                );
                        }
                    }
                }
            }
            catch { }
            return vDatos;
        }

        public Boolean BorrarLog()
        {
            Boolean vResultado = false;
            try
            {
                HttpService vConector = new HttpService();
                msgLogs vLogRequest = new msgLogs()
                {
                    tipo = "3"
                };

                String vResponseLogs = "";
                HttpResponseMessage vHttpResponseLogs = vConector.PostLogs(vLogRequest, ref vResponseLogs);
                if (vHttpResponseLogs.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vLogsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseLogs);
                    if (Convert.ToInt32(vLogsResponse.updateCount1) > 0)
                    {
                        vResultado = true;
                    }
                }
            }
            catch { }
            return vResultado;
        }
    }
}