using ControlCambios.classes;
using ControlCambios.messages;
using ControlCambios.pages.services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlCambios.pages.configurations
{
    public partial class users : System.Web.UI.Page
    {
        msgLoginResponse vConfigurations = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String vEstado = Request.QueryString["e"];

                if (Convert.ToBoolean(Session["AUTH"]))
                {
                    vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                    Generales vGenerales = new Generales();
                    if (!vGenerales.PermisosEntrada(Permisos.Administrador, vConfigurations.resultSet1[0].idCargo))
                    {
                        Logs vLog = new Logs();
                        vLog.postLog("Users", "Usuario intento ingresar a las configs de usuario y no tiene permiso", vConfigurations.resultSet1[0].idUsuario);
                        Response.Redirect("/Default.aspx");
                    }
                    if(vEstado != null)
                        Response.Redirect("/pages/configurations/users.aspx#nav-buscar");

                    CargarUsuarios();
                    CargarSupervisores();
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
        protected void CargarSupervisores()
        {
            try
            {
                DDLSupervisor.Items.Clear();
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoUsuarios vRequest = new msgInfoUsuarios()
                {
                    tipo = "7",
                };

                DDLSupervisor.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                DDLSupervisorModificar.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                String vResponseResult = "";
                HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                    foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                    {
                        DDLSupervisor.Items.Add(new ListItem { Text = itemUsuarios.nombres + " " + itemUsuarios.apellidos, Value = itemUsuarios.idUsuario });
                        DDLSupervisorModificar.Items.Add(new ListItem { Text = itemUsuarios.nombres + " " + itemUsuarios.apellidos, Value = itemUsuarios.idUsuario });
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void CargarUsuarios()
        {
            try
            {

                DataTable vDatos = new DataTable();
                vDatos.Columns.Add("idUsuario");
                vDatos.Columns.Add("nombres");
                vDatos.Columns.Add("apellidos");
                vDatos.Columns.Add("correo");
                vDatos.Columns.Add("estado");
                vDatos.Columns.Add("perfil");

                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgInfoUsuarios vRequest = new msgInfoUsuarios()
                {
                    tipo = "8"
                };

                String vResponseResult = "";
                DDLUsuariosCambioEstandar.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                DDLUsuariosCambioEstandarModificar.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                    foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                    {
                        DDLUsuariosCambioEstandar.Items.Add(new ListItem { Text = itemUsuarios.nombres + " " + itemUsuarios.apellidos, Value = itemUsuarios.idUsuario });
                        DDLUsuariosCambioEstandarModificar.Items.Add(new ListItem { Text = itemUsuarios.nombres + " " + itemUsuarios.apellidos, Value = itemUsuarios.idUsuario });
                        String vCargo = String.Empty;
                        switch (itemUsuarios.idcargo)
                        {
                            case "1": vCargo = "Admin"; break;
                            case "2": vCargo = "Supervisor"; break;
                            case "3": vCargo = "QA"; break;
                            case "4": vCargo = "Implementador"; break;
                            case "5": vCargo = "Promotor"; break;
                            case "6": vCargo = "CAB Manager"; break;
                            case "7": vCargo = "Supervisor QA"; break;
                        }

                        vDatos.Rows.Add(
                            itemUsuarios.idUsuario,
                            itemUsuarios.nombres,
                            itemUsuarios.apellidos,
                            itemUsuarios.correo,
                            (itemUsuarios.estado.Equals("true") ? "Activo" : "Inactivo"),
                            vCargo
                            );
                    }
                }

                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
                Session["DATOSSEG"] = vDatos;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVBusqueda.PageIndex = e.NewPageIndex;
            GVBusqueda.DataSource = (DataTable)Session["DATOSSEG"];
            GVBusqueda.DataBind();
        }

        protected void BtnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxUsuario.Text.Equals(""))
                    throw new Exception("Por favor ingrese un usuario.");

                if (TxPassword.Text.Equals(""))
                    throw new Exception("Por favor ingrese un password.");

                if (TxPassword.Text != TxPasswordConfirmacion.Text)
                    throw new Exception("Error en la verificación del password.");

                if (TxCorreo.Text.Equals(""))
                    throw new Exception("Por favor ingrese un correo valido.");

                if (TxNombres.Text.Equals(""))
                    throw new Exception("Por favor ingrese un nombre.");

                String Supervisor = "admin";
                if (DDLCargo.SelectedValue.Equals("0"))
                    throw new Exception("Por favor seleccione un cargo valido");
                else if (DDLCargo.SelectedValue.Equals("5") || DDLCargoModificar.SelectedValue.Equals("4"))
                {
                    if (DDLSupervisor.SelectedValue.Equals("0"))
                        throw new Exception("Por favor seleccione un supervisor valido");
                    else
                    {
                        Supervisor = DDLSupervisor.SelectedValue;
                    }
                }

                String SupervisorSTD = "";
                if (DDLCambiosEstandar.SelectedValue.Equals("1"))
                {
                    if (DDLUsuariosCambioEstandar.SelectedValue.Equals("0"))
                        throw new Exception("Por favor seleccione un usuario valido para supervisor Estandar");
                    else
                        SupervisorSTD = DDLUsuariosCambioEstandar.SelectedValue;
                }

                Generales vGenerales = new Generales();
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoUsuarios vRequest = new msgInfoUsuarios()
                {
                    tipo = "1",
                    usuario = TxUsuario.Text,
                    password = vGenerales.MD5Hash(TxPassword.Text),
                    nombres = TxNombres.Text,
                    apellidos = TxApellidos.Text,
                    correo = TxCorreo.Text,
                    estado = "true",
                    idcargo = DDLCargo.SelectedValue,
                    dependencia = Supervisor,
                    dependenciaSTD = SupervisorSTD
                };

                String vResponseResult = "";
                HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseResult);
                    if (vUsuariosResponse.updateCount1.Equals("1"))
                    {
                        LimpiarUsuario();
                        Mensaje("Creado con exito", WarningType.Success);

                        CargarUsuarios();
                        UpdateDivBusquedas.Update();
                       
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        void LimpiarUsuario()
        {
            TxUsuario.Text = String.Empty;
            TxCorreo.Text = String.Empty;
            TxPassword.Text = String.Empty;
            TxPasswordConfirmacion.Text = String.Empty;
            TxNombres.Text = String.Empty;
            TxApellidos.Text = String.Empty;
            DDLCargo.SelectedIndex = 0;
            DDLSupervisor.SelectedIndex = 0;
            DDLCambiosEstandar.SelectedIndex = 0;
            DDLUsuariosCambioEstandar.SelectedIndex = 0;
            DIVUsuarioEstandar.Visible = false;
            //UpdatePanel2.Update();
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/default.aspx");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void DDLCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DDLCargo.SelectedValue.Equals("5") || DDLCargo.SelectedValue.Equals("4"))
                    DIVSupervisores.Visible = true;
                else
                    DIVSupervisores.Visible = false;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void GVBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LbUsuarioMensaje.Text = "";
                string vIdUsuario = e.CommandArgument.ToString();
                if (e.CommandName == "UsuarioEstado")
                {
                    LbUsuario.Text = vIdUsuario;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openEstadoModal();", true);
                }
                if (e.CommandName == "UsuarioModificar") // 
                {

                    HttpService vConector = new HttpService();
                    vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                    msgInfoUsuarios vRequest = new msgInfoUsuarios()
                    {
                        tipo = "2",
                        usuario = vIdUsuario
                    };

                    String vResponseResult = "";
                    HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                    if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                        foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                        {
                            TxModificarUsuario.Text = itemUsuarios.idUsuario;
                            TxModificarCorreo.Text = itemUsuarios.correo;
                            TxModificarNombres.Text = itemUsuarios.nombres;
                            TxModificarApellidos.Text = itemUsuarios.apellidos;
                            TxPassword.Text = itemUsuarios.password;
                            TxPasswordConfirmacion.Text = itemUsuarios.password;

                            DDLCargoModificar.SelectedIndex = CargarInformacionDDL(DDLCargoModificar, itemUsuarios.idcargo);

                            if (itemUsuarios.idcargo.Equals("5") || itemUsuarios.idcargo.Equals("4"))
                            {
                                DIVSupervisorModificar.Visible = true;
                                DDLSupervisorModificar.SelectedIndex = CargarInformacionDDL(DDLSupervisorModificar, itemUsuarios.dependencia);
                            }
                            else
                            {
                                DIVSupervisorModificar.Visible = false;
                            }
                        }
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModificacionesModal();", true);
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                if (!TxModificarPassword.Text.Equals(""))
                {
                    if (TxModificarPassword.Text != TxModificarPasswordConfirmar.Text)
                        throw new Exception("Error en la verificación del password.");
                }

                String Supervisor = "admin";
                if (DDLCargoModificar.SelectedValue.Equals("0"))
                    throw new Exception("Por favor seleccione un cargo valido");
                else if (DDLCargoModificar.SelectedValue.Equals("5") || DDLCargoModificar.SelectedValue.Equals("4"))
                {
                    if (DDLSupervisorModificar.SelectedValue.Equals("0"))
                        throw new Exception("Por favor seleccione un supervisor valido");
                    else
                    {
                        Supervisor = DDLSupervisorModificar.SelectedValue;
                    }
                }

                String SupervisorSTD = "";
                if (DDLCambiosEstandarModificar.SelectedValue.Equals("1"))
                {
                    if (DDLUsuariosCambioEstandarModificar.SelectedValue.Equals("0"))
                        throw new Exception("Por favor seleccione un usuario valido para supervisor Estandar");
                    else
                        SupervisorSTD = DDLUsuariosCambioEstandarModificar.SelectedValue;
                }

                String vPassword = String.Empty;
                msgInfoUsuarios vRequestCambios = new msgInfoUsuarios()
                {
                    tipo = "2",
                    usuario = TxModificarUsuario.Text
                };

                String vResponseCambiosResult = "";
                HttpResponseMessage vHttpResponseCambios = vConector.PostInfoUsuarios(vRequestCambios, ref vResponseCambiosResult);
                if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoUsuariosQueryResponse vUsuariosCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseCambiosResult);
                    foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosCambiosResponse.resultSet1)
                    {
                        vPassword = itemUsuarios.password;
                    }
                }
                Generales vGenerales = new Generales();
                String vPasswordFinal = string.Empty;
                if (TxModificarPassword.Text.Equals(""))
                {
                    vPasswordFinal = vPassword;
                }
                else
                {
                    vPasswordFinal = vGenerales.MD5Hash(TxModificarPassword.Text);
                }
      
                msgInfoUsuarios vRequest = new msgInfoUsuarios()
                {
                    tipo = "9",
                    usuario = TxModificarUsuario.Text,
                    password = vPasswordFinal,
                    nombres = TxModificarNombres.Text,
                    apellidos = TxModificarApellidos.Text,
                    correo = TxModificarCorreo.Text,
                    idcargo = DDLCargoModificar.SelectedValue,
                    dependencia = Supervisor,
                    dependenciaSTD = SupervisorSTD
                };

                String vResponseResult = "";
                HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseResult);
                    if (vUsuariosResponse.updateCount1.Equals("1"))
                    {
                        CargarUsuarios();
                        UpdateGridView.Update();
                        CerrarModal("ModificacionModal");
                        Mensaje("Usuario actualizado con exito", WarningType.Success);
                    }
                }
            }
            catch (Exception Ex) { LbUsuarioMensaje.Text = Ex.Message; UpdateUsuarioMensaje.Update(); }
        }

        protected void BtnEstado_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoUsuarios vRequest = new msgInfoUsuarios()
                {
                    tipo = "10",
                    usuario = LbUsuario.Text,
                    estado = DDLEstado.SelectedValue
                };

                String vResponseResult = "";
                HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseResult);
                    if (vUsuariosResponse.updateCount1.Equals("1"))
                    {
                        CargarUsuarios();
                        UpdateGridView.Update();
                        CerrarModal("EstadoModal");
                        Mensaje("Usuario actualizado con exito",WarningType.Success);
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void DDLCargoModificar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DDLCargoModificar.SelectedValue.Equals("5") || DDLCargoModificar.SelectedValue.Equals("4"))
                    DIVSupervisorModificar.Visible = true;
                else
                    DIVSupervisorModificar.Visible = false;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        int CargarInformacionDDL(DropDownList vList, String vValue)
        {
            int vIndex = 0;
            try
            {
                int vContador = 0;
                foreach (ListItem item in vList.Items)
                {
                    if (item.Value.Equals(vValue))
                    {
                        vIndex = vContador;
                    }
                    vContador++;
                }
            }
            catch { throw; }
            return vIndex;
        }

        protected void DDLCambiosEstandar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DDLCambiosEstandar.SelectedValue.Equals("1"))
                {
                    DIVUsuarioEstandar.Visible = true;
                }
                else
                {
                    DIVUsuarioEstandar.Visible = false;
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void DDLCambioEstandarModificar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLCambiosEstandarModificar.SelectedValue.Equals("1"))
            {
                DIVUsuarioEstandarModificar.Visible = true;
            }
            else
            {
                DIVUsuarioEstandarModificar.Visible = false;
            }
        }
    }
}