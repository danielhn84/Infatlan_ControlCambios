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
    public partial class users : System.Web.UI.Page
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
                        vLog.postLog("Users", "Usuario intento ingresar a las configs de usuario y no tiene permiso", vConfigurations.resultSet1[0].idUsuario);
                        Response.Redirect("/Default.aspx");
                    }

                    CargarUsuarios();
                }
            }
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
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
                    tipo = "4"
                };

                String vResponseResult = "";
                HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                    foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                    {
                        vDatos.Rows.Add(
                            itemUsuarios.idUsuario,
                            itemUsuarios.nombres,
                            itemUsuarios.apellidos,
                            itemUsuarios.correo,
                            itemUsuarios.estado,
                            itemUsuarios.idcargo
                            );
                    }
                }

                GVBusqueda.DataSource = vDatos;
                GVBusqueda.DataBind();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}