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

namespace ControlCambios.pages.configurations
{
    public partial class generals : System.Web.UI.Page
    {
        msgLoginResponse vConfigurations = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Convert.ToBoolean(Session["AUTH"]))
                {
                    vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                    Generales vGenerales = new Generales();
                    if (!vGenerales.PermisosEntrada(Permisos.Administrador, vConfigurations.resultSet1[0].idCargo))
                    {
                        Logs vLog = new Logs();
                        vLog.postLog("Generals", "Usuario intento ingresar a las configs generales y no tiene permiso", vConfigurations.resultSet1[0].idUsuario);
                        Response.Redirect("/Default.aspx");
                    }

                    CargarEquipos();
                    CargarSistemas();
                    CargarEquiposSistemas();
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

        void CargarEquiposSistemas()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgCatEquipos vRequest = new msgCatEquipos()
                {
                    tipo = "1"
                };

                String vResponseResult = "";

                DDLSistemaEquipo.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                HttpResponseMessage vHttpResponse = vConector.PostEquipos(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgCatEquiposQueryResponse vResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgCatEquiposQueryResponse>(vResponseResult);
                    if (vResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgCatEquiposQueryResponseItem item in vResponse.resultSet1)
                        {
                            DDLSistemaEquipo.Items.Add(new ListItem { Text = item.nombre, Value = item.idCatEquipo });
                        }
                    }
                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void CargarEquipos()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                DataTable vDatos = new DataTable();
                vDatos.Columns.Add("idCatEquipo");
                vDatos.Columns.Add("nombre");
                vDatos.Columns.Add("tipoEquipo");
                vDatos.Columns.Add("ip");
                vDatos.Columns.Add("ubicacion");

                msgCatEquipos vRequest = new msgCatEquipos()
                {
                    tipo = "1"
                };

                String vResponseResult = "";
                HttpResponseMessage vHttpResponse = vConector.PostEquipos(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgCatEquiposQueryResponse vResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgCatEquiposQueryResponse>(vResponseResult);
                    if (vResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgCatEquiposQueryResponseItem item in vResponse.resultSet1)
                        {
                            vDatos.Rows.Add(
                                item.idCatEquipo,
                                item.nombre,
                                item.tipoEquipo,
                                item.ip,
                                item.ubicacion
                                );
                        }
                    }
                }

                GVBusquedaEquipos.DataSource = vDatos;
                GVBusquedaEquipos.DataBind();
                Session["DATOSEQUIPOS"] = vDatos;
                UpdateGridView.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void CargarSistemas()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                DataTable vDatos = new DataTable();
                vDatos.Columns.Add("idCatSistemas");
                vDatos.Columns.Add("idCatEquipo");
                vDatos.Columns.Add("sistema");
                vDatos.Columns.Add("descripcion");

                msgCatSistemas vRequest = new msgCatSistemas()
                {
                    tipo = "1"
                };

                String vResponseResult = "";
                HttpResponseMessage vHttpResponse = vConector.PostSistemas(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgCatSistemasQueryResponse vResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgCatSistemasQueryResponse>(vResponseResult);
                    if (vResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgCatSistemasQueryResponseItem item in vResponse.resultSet1)
                        {
                            vDatos.Rows.Add(
                                item.idCatSistemas,
                                item.idCatEquipo,
                                item.sistema,
                                item.descripcion
                                );
                        }
                    }
                }

                GVBusquedaSistemas.DataSource = vDatos;
                GVBusquedaSistemas.DataBind();
                Session["DATOSSISTEMAS"] = vDatos;
                UpdatePanel4.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnEquipoGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgCatEquipos vRequest = new msgCatEquipos()
                {
                    tipo = "2",
                    nombre = TxEquipoNombre.Text,
                    tipoequipo = TxEquipoTipo.Text,
                    ip = TxEquipoIp.Text,
                    ubicacion = TxEquipoUbicacion.Text
                };

                String vResponseResult = "";
                HttpResponseMessage vHttpResponse = vConector.PostEquipos(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseResult);
                    if (vResponse.updateCount1.Equals("1"))
                    {
                        LimpiarEquipos();
                        Mensaje("Ingresado con Exito", WarningType.Success);

                        CargarEquipos();
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void LimpiarEquipos()
        {
            TxEquipoNombre.Text = "";
            TxEquipoTipo.Text = "";
            TxEquipoIp.Text = "";
            TxEquipoUbicacion.Text = "";
        }

        protected void BtnEquipoCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarEquipos();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusquedaEquipos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LbEquipoEstado.Text = "";
                LbEquipoModificar.Text = "";
                string vIdEquipo = e.CommandArgument.ToString();
                if (e.CommandName == "EquipoEstado")
                {
                    LbEquipoEstado.Text = vIdEquipo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openEquipoEstadoModal();", true);
                }
                if (e.CommandName == "EquipoModificar")  
                {
                    LbEquipoModificar.Text = vIdEquipo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openEquipoModificacionesModal();", true);
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusquedaEquipos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusquedaEquipos.PageIndex = e.NewPageIndex;
                GVBusquedaEquipos.DataSource = (DataTable)Session["DATOSEQUIPOS"];
                GVBusquedaEquipos.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnEquipoEstado_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgCatEquipos vRequest = new msgCatEquipos()
                {
                    tipo = "4",
                    idequipo = LbEquipoEstado.Text,
                    estado = DDLEstado.SelectedValue
                };

                String vResponseResult = "";
                HttpResponseMessage vHttpResponse = vConector.PostEquipos(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseResult);
                    if (vResponse.updateCount1.Equals("1"))
                    {
                        Mensaje("Actualizado con Exito", WarningType.Success);
                        CargarEquipos();
                        CerrarModal("EquipoEstadoModal");
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnEquipoModificar_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgCatEquipos vRequest = new msgCatEquipos()
                {
                    tipo = "3",
                    idequipo = LbEquipoModificar.Text,
                    ip = TxEquipoModificarIp.Text
                };

                String vResponseResult = "";
                HttpResponseMessage vHttpResponse = vConector.PostEquipos(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseResult);
                    if (vResponse.updateCount1.Equals("1"))
                    {
                        Mensaje("Actualizado con Exito", WarningType.Success);
                        CargarEquipos();
                        CerrarModal("EquipoModificacionModal");
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnSistemaGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLSistemaEquipo.SelectedValue.Equals("0"))
                    throw new Exception("Seleccione un equipo valido");

                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgCatSistemas vRequest = new msgCatSistemas()
                {
                    tipo = "3",
                    idequipo = DDLSistemaEquipo.SelectedValue,
                    nombre = TxSistemaNombre.Text,
                    descripcion = TxSistemaDescripcion.Text
                };

                String vResponseResult = "";
                HttpResponseMessage vHttpResponse = vConector.PostSistemas(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseResult);
                    if (vResponse.updateCount1.Equals("1"))
                    {
                        LimpiarSistemas();
                        Mensaje("Ingresado con Exito", WarningType.Success);

                        CargarSistemas();
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void LimpiarSistemas()
        {
            DDLSistemaEquipo.SelectedIndex = -1;
            TxSistemaNombre.Text = "";
            TxSistemaDescripcion.Text = "";
        }

        protected void BtnSistemaCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarSistemas();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusquedaSistemas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusquedaSistemas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVBusquedaSistemas.PageIndex = e.NewPageIndex;
                GVBusquedaSistemas.DataSource = (DataTable)Session["DATOSSISTEMAS"];
                GVBusquedaSistemas.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void TxBuscarEquipo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String vBusqueda = TxBuscarEquipo.Text;
                DataTable vDatos =  (DataTable)Session["DATOSEQUIPOS"];

                if (vBusqueda.Equals(""))
                {
                    GVBusquedaEquipos.DataSource = vDatos;
                    GVBusquedaEquipos.DataBind();
                    UpdateGridView.Update();
                }
                else
                {
                    EnumerableRowCollection<DataRow> filtered = vDatos.AsEnumerable()
                        .Where(r => r.Field<String>("nombre").Contains(vBusqueda)
                        || r.Field<String>("ip").Contains(vBusqueda)
                        || r.Field<String>("ubicacion").Contains(vBusqueda));


                    DataTable vDatosFiltrados = new DataTable();
                    vDatosFiltrados.Columns.Add("idCatEquipo");
                    vDatosFiltrados.Columns.Add("nombre");
                    vDatosFiltrados.Columns.Add("tipoEquipo");
                    vDatosFiltrados.Columns.Add("ip");
                    vDatosFiltrados.Columns.Add("ubicacion");
                    foreach (DataRow item in filtered)
                    {
                        vDatosFiltrados.Rows.Add(
                            item["idCatEquipo"].ToString(),
                            item["nombre"].ToString(),
                            item["tipoEquipo"].ToString(),
                            item["ip"].ToString(),
                            item["ubicacion"].ToString()
                            );
                    }

                    GVBusquedaEquipos.DataSource = vDatosFiltrados;
                    GVBusquedaEquipos.DataBind();
                    UpdateGridView.Update();
                }

                
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}