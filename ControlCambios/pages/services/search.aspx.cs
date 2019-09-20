using ControlCambios.classes;
using ControlCambios.messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlCambios.pages.services
{
    public partial class search : System.Web.UI.Page
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


                    if (vBusqueda != null)
                    {
                        BuscarCambioGet(vBusqueda);
                    }
                    else
                    {
                        DivBusquedaCampos.Visible = true;
                    }
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

        protected void BuscarCambioGet(String vBusquedaQuery)
        {
            try
            {
                String vTipo = "4";
                String vBusqueda = vBusquedaQuery;


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
                                case "0": vEstado = "Correcciones promotor"; break;
                                case "1": vEstado = "Pendiente revisión QA"; break;
                                case "2": vEstado = "CAB Manager"; break;
                                case "3": vEstado = "Implementación"; break;
                                case "4": vEstado = "Revisión Promotor"; break;
                                case "5": vEstado = "Cambio terminado / No cerrado"; break;
                                case "6": vEstado = "Cambio cerrado"; break;
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
                Session["BUSQUEDACAMBIOS"] = vDatos;
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
                else if (!TxBuscarNombre.Text.Equals(""))
                {
                    vTipo = "4";
                    vBusqueda = TxBuscarNombre.Text;
                }
                else if (!TxFechaCambio.Text.Equals(""))
                {
                    vTipo = "11";
                    vBusqueda = Convert.ToDateTime(TxFechaCambio.Text).ToString("yyyy-MM-dd");
                }
                else if (!DDLTipoCambio.SelectedValue.Equals("0"))
                {
                    vTipo = "10";
                    vBusqueda = DDLTipoCambio.SelectedValue;
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
                                case "0": vEstado = "Correcciones promotor"; break;
                                case "1": vEstado = "Pendiente revisión QA"; break;
                                case "2": vEstado = "CAB Manager"; break;
                                case "3": vEstado = "Implementación"; break;
                                case "4": vEstado = "Revisión Promotor"; break;
                                case "5": vEstado = "Cambio terminado / No cerrado"; break;
                                case "6": vEstado = "Cambio cerrado"; break;
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
                Session["BUSQUEDACAMBIOS"] = vDatos;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "GoBottom();", true);
                LimpiarBusqueda();
                
                
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

        void LimpiarBusqueda()
        {
            TxBuscarNombre.Text = String.Empty;
            TxBuscarNumero.Text = String.Empty;
            TxFechaCambio.Text = String.Empty;
            DDLTipoCambio.SelectedIndex = -1;
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                if (e.CommandName == "EntrarCambio")
                {
                    string vIdCambio = e.CommandArgument.ToString();

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
                                if (item.pasos != null)
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

                if (e.CommandName == "CerrarCambio")
                {
                    string vIdCambio = e.CommandArgument.ToString();
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
                                if (item.pasos != null)
                                {
                                    if (item.pasos.Equals("3"))
                                    {
                                        LbNumeroCambio.Text = vIdCambio;
                                        UpdateLabelCambio.Update();
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                                    }
                                    else
                                        throw new Exception("Necesitas que la resolución del cambio este creada (Paso 6)");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCerrarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();
                if (!vGenerales.PermisosEntrada(Permisos.Supervisor, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion");

                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCierreCambiosRequest = new msgInfoCambios()
                {
                    tipo = "6",
                    idcambio = LbNumeroCambio.Text,
                    idresolucion = "1"
                };
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCierreCambios = vConector.PostInfoCambios(vInfoCierreCambiosRequest, ref vResponseCambios);

                if (vHttpResponseCierreCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vInfoCambiosCierreResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambios);
                    if (vInfoCambiosCierreResponse.updateCount1.Equals("1"))
                    {
                        Mensaje("Cambio cerrado con exito!", WarningType.Success);
                        CerrarModal("CerrarModal");
                    }
                    else
                    {
                        Mensaje("Ha pasado un error al cerrar el cambio, contacte a sistemas", WarningType.Danger); //22692800 Elisa Ramirez #8cbe40
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); CerrarModal("CerrarModal"); }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVBusqueda.PageIndex = e.NewPageIndex;
            GVBusqueda.DataSource = (DataTable)Session["BUSQUEDACAMBIOS"];
            GVBusqueda.DataBind();
        }

        
    }
}