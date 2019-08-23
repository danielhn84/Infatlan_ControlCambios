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

namespace ControlCambios.pages.customs
{
    public partial class reports : System.Web.UI.Page
    {
        msgLoginResponse vConfigurations = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String vBusqueda = Request.QueryString["busqueda"];

                if (Convert.ToBoolean(Session["AUTH"]))
                {
                    vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                    Session["USERTYPE"] = vConfigurations.resultSet1[0].idCargo;


                }
            }
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        protected void BtnResumenReportes_Click(object sender, EventArgs e)
        {

        }

        protected void BtnBuscarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                String vTipo = String.Empty;
                String vBusqueda = String.Empty;
                if (!TxBuscarNumero.Text.Equals(""))
                {
                    vTipo = "3";
                    vBusqueda = TxBuscarNumero.Text;
                }
                else
                {
                    vTipo = "4";
                    vBusqueda = TxBuscarNombre.Text;
                }

                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                DataTable vDatos = new DataTable();
                vDatos.Columns.Add("idcambio");
                vDatos.Columns.Add("mantenimientoNombre");
                vDatos.Columns.Add("fechaSolicitud");
                vDatos.Columns.Add("idUsuarioResponsable");
                vDatos.Columns.Add("idResolucion");
                vDatos.Columns.Add("estado");

                msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                {
                    tipo = vTipo,
                    idcambio = vBusqueda,
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
                                case "1": vEstado = "Pendiente revisión QA"; break;
                                case "2": vEstado = "Pendiente de Cierre"; break;
                                case "3": vEstado = "Proceso terminado"; break;
                            }

                            vDatos.Rows.Add(
                                item.idcambio,
                                item.mantenimientoNombre,
                                item.fechaSolicitud,
                                item.idUsuarioResponsable,
                                item.idResolucion,
                                vEstado
                                );
                        }
                    }
                    else
                    {
                        DivBusqueda.Visible = false;
                        UpdateDivBusquedas.Update();
                        throw new Exception("No se encontro ningun resultado.");
                    }
                }
                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                foreach (GridViewRow row in GVBusqueda.Rows)
                {
                    msgInfoCambios vInfoCambiosRowRequest = new msgInfoCambios()
                    {
                        tipo = "3",
                        idcambio = row.Cells[2].Text,
                        usuariogrud = vConfigurations.resultSet1[0].idUsuario
                    };
                    String vResponseRowCambios = "";
                    HttpResponseMessage vHttpResponseRowCambios = vConector.PostInfoCambios(vInfoCambiosRowRequest, ref vResponseRowCambios);

                    if (vHttpResponseRowCambios.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgInfoCambiosQueryResponse vInfoCambioRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseRowCambios);
                        if (vInfoCambioRowsResponse.resultSet1.Count() > 0)
                        {

                            if (vInfoCambioRowsResponse.resultSet1[0].idResolucion.Equals("1"))
                            {
                                Button button = row.FindControl("BtnCerrarCambio") as Button;
                                button.Text = "Cerrado";
                                button.CssClass = "btn btn-success mr-2 ";
                                button.Enabled = false;
                                button.CommandName = "Cerrado";
                            }
                        }
                    }
                }
                DivBusqueda.Visible = true;
                UpdateDivBusquedas.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                if (e.CommandName == "Reporte")
                {
                    string vIdCambio = e.CommandArgument.ToString();
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}