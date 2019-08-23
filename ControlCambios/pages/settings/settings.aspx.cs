using ControlCambios.classes;
using ControlCambios.messages;
using ControlCambios.pages.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlCambios.pages.settings
{
    public partial class settings : System.Web.UI.Page
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

                    LbNombreUsuario.Text = vConfigurations.resultSet1[0].nombres + " " + vConfigurations.resultSet1[0].apellidos;
                }
            }
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        protected void BtnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];


                if (!TxPassword.Text.Equals(TxConfirmar.Text))
                    throw new Exception("Las contraseñas ingresadas no coinciden.");

                msgInfoUsuarios vInfoUsuarioRequest = new msgInfoUsuarios()
                {
                    tipo = "3",
                    password = (TxPassword.Text.Equals("") ? vConfigurations.resultSet1[0].password : TxPassword.Text),
                    telefono = (TxTelefono.Text.Equals("") ? vConfigurations.resultSet1[0].telefono : TxTelefono.Text),
                    correo = (TxCorreo.Text.Equals("") ? vConfigurations.resultSet1[0].correo : TxCorreo.Text),
                    estado = vConfigurations.resultSet1[0].estado,
                    idcargo = vConfigurations.resultSet1[0].idCargo,
                    nombres = vConfigurations.resultSet1[0].nombres,
                    apellidos = vConfigurations.resultSet1[0].apellidos,
                    usuario = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseUsuarios = "";
                HttpResponseMessage vHttpResponseUsuarios = vConector.PostInfoUsuarios(vInfoUsuarioRequest, ref vResponseUsuarios);

                if (vHttpResponseUsuarios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vInfoUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseUsuarios);
                    if (vInfoUsuariosResponse.updateCount1.Equals("1"))
                    {
                        Exitoso();
                    }
                    else
                        throw new Exception("Ha ocurrido un problema, contacte a sistemas");
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void Exitoso()
        {
            TxPassword.Text = String.Empty;
            TxConfirmar.Text = String.Empty;
            TxTelefono.Text = String.Empty;
            TxCorreo.Text = String.Empty;

            Mensaje("Actualizado con Exito!", WarningType.Success);
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/default.aspx");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}