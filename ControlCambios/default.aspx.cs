using ControlCambios.classes;
using ControlCambios.messages;
using ControlCambios.pages.services;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Timers;

namespace ControlCambios
{
    public partial class _default : System.Web.UI.Page
    {
        msgLoginResponse vConfigurations = null;
        System.Timers.Timer myTimer = new System.Timers.Timer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"]))
                {
                    vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                    Session["USERTYPE"] = vConfigurations.resultSet1[0].idCargo;
                    ObtenerUltimos();
                    CargarCambiosSummary();

                    if (vConfigurations.resultSet1[0].telefono != null)
                    {
                        if (vConfigurations.resultSet1[0].telefono.Equals(""))
                        {
                            Thread vThread = new Thread(new ThreadStart(alert));
                            vThread.IsBackground = true;
                            vThread.Start();
                        }
                    }
                    else
                    {
                        Thread vThread = new Thread(new ThreadStart(alert));
                        vThread.IsBackground = true;
                        vThread.Start();
                    }
                }
            }
        }

        void alert()
        {
            String vAlerta = "Por favor actualiza tu numero de telefono en Settings." + @"\n\r" +
                            "Dirección: Tu nombre > Settings";

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "window.alert('" + vAlerta + "')", true);
        }


        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void CargarCambiosSummary()
        {
            try
            {

                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgConsultasGenerales vConsultasGeneralesRequest = new msgConsultasGenerales()
                {
                    tipo = "2"
                };

                String vResponseConsultasGenerales = "";
                HttpResponseMessage vHttpResponseConsultasGenerales = vConector.PostCalendario(vConsultasGeneralesRequest, ref vResponseConsultasGenerales);

                if (vHttpResponseConsultasGenerales.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgConsultasGeneralesSummary vInfoConsultasGeneralesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgConsultasGeneralesSummary>(vResponseConsultasGenerales);
                    if (vInfoConsultasGeneralesResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgConsultasGeneralesSummaryItem item in vInfoConsultasGeneralesResponse.resultSet1)
                        {
                            LitFechaCambios.Text = item.fechaSolicitud;
                            LitCambiosCreados.Text = item.creados;
                            LitCambiosFinalizados.Text = item.finalizados;
                        }
                    }
                }


            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        
        protected void ObtenerUltimos()
        {
            try
            {

                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                DataTable vDatos = new DataTable();
                vDatos.Columns.Add("idcambio");
                vDatos.Columns.Add("mantenimientoNombre");
                vDatos.Columns.Add("fechaSolicitud");
                vDatos.Columns.Add("idUsuarioResponsable");
                vDatos.Columns.Add("estado");

                msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                {
                    tipo = "5",
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                    if (vInfoCambiosResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambiosResponse.resultSet1)
                        {
                            String vEstado = String.Empty;
                            switch (item.pasos)
                            {
                                case "0": vEstado = "Correcciones promotor"; break;
                                case "1":
                                    msgInfoAprobaciones vInfoAprobacionesRowRequest = new msgInfoAprobaciones()
                                    {
                                        tipo = "3",
                                        idaprobacion = item.idcambio
                                    };
                                    String vResponseRowAprobaciones = "";
                                    HttpResponseMessage vHttpResponseRowAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRowRequest, ref vResponseRowAprobaciones);

                                    if (vHttpResponseRowAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        msgInfoAprobacionesQueryResponse vInfoAprobacionesRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseRowAprobaciones);
                                        if (vInfoAprobacionesRowsResponse.resultSet1.Count() > 0)
                                        {
                                            foreach (msgInfoAprobacionesQueryResponseItem itemAprobacion in vInfoAprobacionesRowsResponse.resultSet1)
                                            {
                                                if (itemAprobacion.estado.Equals("true"))
                                                    vEstado = "Pendiente revisión QA";
                                                else
                                                    vEstado = "Autorización";
                                            }
                                        }
                                    }

                                    break;
                                case "2": vEstado = "CAB Manager"; break;
                                case "3": vEstado = "Implementación"; break;
                                case "4": vEstado = "Revisión Implementador"; break;
                                case "5": vEstado = "Cambio terminado / No cerrado"; break;
                                case "6": vEstado = "Cambio cerrado"; break;
                            }

                            vDatos.Rows.Add(
                                item.idcambio,
                                item.mantenimientoNombre,
                                item.fechaSolicitud,
                                item.idUsuarioResponsable,
                                vEstado);
                        }
                    }

                }
                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EntrarCambio")
                {
                    string vIdCambio = e.CommandArgument.ToString();

                    HttpService vConector = new HttpService();
                    vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                    msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                    {
                        tipo = "3",
                        idcambio = vIdCambio,
                        usuariogrud = vConfigurations.resultSet1[0].idUsuario
                    };
                    String vResponseCambios = "";
                    HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                    if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                        if (vInfoCambiosResponse.resultSet1.Count() > 0)
                        {
                            foreach (msgInfoCambiosQueryResponseItem item in vInfoCambiosResponse.resultSet1)
                            {
                                int vPaso = 6;
                                switch (item.pasos)
                                {
                                    case "1": vPaso = 2; break;
                                    case "2": vPaso = 3; break;
                                    case "3": vPaso = 4; break;
                                    case "4": vPaso = 5; break;
                                    case "5": vPaso = 6; break;
                                    case "6": vPaso = 6; break;
                                }

                                Response.Redirect("/pages/services/changes.aspx?id=" + vIdCambio + "#step-" + vPaso);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnReporteSummary_Click(object sender, EventArgs e)
        {
            try
            {
                
           
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
            
        }
    }
}