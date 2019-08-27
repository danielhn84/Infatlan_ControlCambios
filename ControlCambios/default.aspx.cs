using ControlCambios.classes;
using ControlCambios.messages;
using ControlCambios.pages.services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlCambios
{
    public partial class _default : System.Web.UI.Page
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
                    ObtenerUltimos();
                }
            }
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
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
                            vDatos.Rows.Add(
                                item.idcambio,
                                item.mantenimientoNombre,
                                item.fechaSolicitud,
                                item.idUsuarioResponsable);
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
    }
}