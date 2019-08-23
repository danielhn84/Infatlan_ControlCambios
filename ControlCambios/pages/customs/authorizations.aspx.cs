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
    public partial class authorizations : System.Web.UI.Page
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
        public void CerrarModal(String vModal)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }
        protected void BtnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/pages/services/changes.aspx");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
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
                            vDatos.Rows.Add(
                                item.idcambio,
                                item.mantenimientoNombre,
                                item.fechaSolicitud,
                                item.idUsuarioResponsable);
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
                    msgInfoAprobaciones vInfoAprobacionesRowRequest = new msgInfoAprobaciones()
                    {
                        tipo = "3",
                        idaprobacion = row.Cells[1].Text
                    };
                    String vResponseRowAprobaciones = "";
                    HttpResponseMessage vHttpResponseRowAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRowRequest, ref vResponseRowAprobaciones);

                    if (vHttpResponseRowAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgInfoAprobacionesQueryResponse vInfoAprobacionesRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseRowAprobaciones);
                        if (vInfoAprobacionesRowsResponse.resultSet1.Count() > 0)
                        {

                            if (vInfoAprobacionesRowsResponse.resultSet1[0].estado.Equals("true"))
                            {
                                Button button = row.FindControl("BtnAutorizar") as Button;
                                button.Text = "Autorizado";
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
                if (e.CommandName == "AutorizarCambio")
                {
                    string vIdCambio = e.CommandArgument.ToString();
                    LbNumeroCambio.Text = vIdCambio;
                    UpdateLabelCambio.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnAutorizarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                Generales vGenerales = new Generales();
                if (!vGenerales.PermisosEntrada(Permisos.Supervisor, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion");

                msgInfoAprobaciones vInfoAprobacionesRowRequest = new msgInfoAprobaciones()
                {
                    tipo = "2",
                    idaprobacion = LbNumeroCambio.Text,
                    aprobador = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseRowAprobaciones = "";
                HttpResponseMessage vHttpResponseRowAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRowRequest, ref vResponseRowAprobaciones);

                if (vHttpResponseRowAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vInfoAprobacionesRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseRowAprobaciones);
                    if (vInfoAprobacionesRowsResponse.updateCount1.Equals("1"))
                    {
                        CerrarModal("AutorizarModal");
                        Mensaje("El cambio ha sido autorizada", WarningType.Success);
                    }
                    else
                    {
                        CerrarModal("AutorizarModal");
                        Mensaje("No tienes permisos para autorizar este cambio",WarningType.Danger);
                    }
                }
            }
            catch (Exception Ex) { LbAutorizarMensaje.Text = Ex.Message; UpdateAutorizarMensaje.Update(); }
        }
    }
}