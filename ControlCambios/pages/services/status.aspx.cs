using ControlCambios.classes;
using ControlCambios.messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace ControlCambios.pages.services
{
    
    public partial class status : System.Web.UI.Page
    {
        msgLoginResponse vConfigurations = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"]))
                {
                    vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                    Session["USERTYPE"] = vConfigurations.resultSet1[0].idCargo;


                    ObtenerAprobaciones();
                }
                LbCambio.Text = "1";//Convert.ToString(Session["CAMBIOCREADO"]);
            }
        }

        void ObtenerAprobaciones()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                DataTable vDatos = new DataTable();
                vDatos.Columns.Add("usuario");
                vDatos.Columns.Add("estado");
                vDatos.Columns.Add("fechaCreacion");

                msgInfoAprobaciones vInfoAprobacionesRequest = new msgInfoAprobaciones()
                {
                    tipo = "3",
                    idaprobacion = Convert.ToString(Session["CAMBIOCREADO"])
                };
                String vResponseAprobaciones = "";
                HttpResponseMessage vHttpResponseAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRequest, ref vResponseAprobaciones);

                if (vHttpResponseAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoAprobacionesQueryResponse vInfoAprobacionesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseAprobaciones);
                    if (vInfoAprobacionesResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoAprobacionesQueryResponseItem item in vInfoAprobacionesResponse.resultSet1)
                        {
                            EnviarMensaje();

                            vDatos.Rows.Add(
                                item.idUsuarioAprobador,
                                (Convert.ToBoolean(item.estado) == false ? "Pendiente" : "Aprobada"),
                                item.fechaCreacion);
                        }
                    }
                }

                GVNotificaciones.DataSource = vDatos;
                GVNotificaciones.DataBind();

            }
                
            catch (Exception)
            {

                throw;
            }
        }

        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        protected void BtnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/pages/services/changes.aspx");
            }
            catch (Exception Ex){ Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVNotificaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EnviarRow")
                {
                    EnviarMensaje();
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }
        }

        protected Boolean EnviarMensaje()
        {
            Boolean vRespuesta = false;
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRowRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["CAMBIOCREADO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseRowCambios = "";
                HttpResponseMessage vHttpResponseRowCambios = vConector.PostInfoCambios(vInfoCambiosRowRequest, ref vResponseRowCambios);

                if (vHttpResponseRowCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambioRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseRowCambios);
                    if (vInfoCambioRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambioRowsResponse.resultSet1)
                        {

                            msgInfoAprobaciones vInfoAprobacionesRequest = new msgInfoAprobaciones()
                            {
                                tipo = "3",
                                idaprobacion = Convert.ToString(Session["CAMBIOCREADO"])
                            };
                            String vResponseAprobaciones = "";
                            HttpResponseMessage vHttpResponseAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRequest, ref vResponseAprobaciones);

                            if (vHttpResponseAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoAprobacionesQueryResponse vInfoAprobacionesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseAprobaciones);
                                if (vInfoAprobacionesResponse.resultSet1.Count() > 0)
                                {
                                    foreach (msgInfoAprobacionesQueryResponseItem itemAprobaciones in vInfoAprobacionesResponse.resultSet1)
                                    {
                                        msgInfoUsuarios vRequest = new msgInfoUsuarios()
                                        {
                                            tipo = "2",
                                            usuario = itemAprobaciones.idUsuarioAprobador
                                        };

                                        String vResponseResult = "";
                                        HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                                        if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                        {
                                            msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                            foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                            {
                                                SmtpService vSmtpService = new SmtpService();
                                                vRespuesta = vSmtpService.EnviarMensaje(
                                                    itemUsuarios.correo,
                                                    typeBody.Promotor,
                                                    itemUsuarios.nombres + "(" + itemAprobaciones.idUsuarioAprobador + ")",
                                                    Convert.ToString(Session["CAMBIOCREADO"]),
                                                    item.mantenimientoNombre);
                                            }
                                        }
                                    }
                                }
                            }
                        } 
                    }
                } 
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }

            return vRespuesta;
        }
    }
}