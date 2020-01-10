using ControlCambios.classes;
using ControlCambios.messages;
using ControlCambios.pages.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ControlCambios
{
    public partial class main : System.Web.UI.MasterPage
    {
        msgLoginResponse vConfigurations = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Convert.ToBoolean(Session["AUTH"]))
            {
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Session["USERTYPE"] = vConfigurations.resultSet1[0].idCargo;
            }
            else
                Response.Redirect("/login.aspx");

            if (!Page.IsPostBack)
            {
                String vError = "";
                try
                {
                    LitUsuario.Text = vConfigurations.resultSet1[0].nombres;

                    if (vConfigurations.resultSet1[0].idCargo.Equals("1") || vConfigurations.resultSet1[0].idCargo.Equals("2") )
                    {
                        LIAuth.Visible = true;
                        if(vConfigurations.resultSet1[0].idCargo.Equals("1"))
                            LILogs.Visible = true;
                    }


                    CargarNotifiaciones();
                    CargarConfiguraciones();
                }
                catch (Exception Ex)
                {
                    vError = Ex.Message;
                }
            }
            this.TxBuscarCambio.Attributes.Add("onkeypress", "button_click(this,'" + this.TxBuscarCambio.ClientID + "')");
        }

        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }

        void CargarConfiguraciones()
        {
            try
            {
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                if (vConfigurations.resultSet1[0].idCargo.Equals("1") || vConfigurations.resultSet1[0].idCargo.Equals("2"))
                {
                    String vConfiguracionesHtml = "<li class=\"nav-item dropdown mr-1\">" + Environment.NewLine +
                            "<a class=\"nav-link count-indicator dropdown-toggle d-flex justify-content-center align-items-center\" id=\"messageDropdown\" href=\"#\" data-toggle=\"dropdown\">" + Environment.NewLine +
                            "    <i class=\"mdi mdi-database mx-0\"></i>" + Environment.NewLine +
                            "</a>" + Environment.NewLine +
                            "<div class=\"dropdown-menu dropdown-menu-right navbar-dropdown\" aria-labelledby=\"messageDropdown\">" + Environment.NewLine +
                            "    <p class=\"mb-0 font-weight-normal float-left dropdown-header\">Configuraciones</p>" + Environment.NewLine +
                            "    <a class=\"dropdown-item\" href=\"/pages/configurations/generals.aspx\">" + Environment.NewLine +
                            "        <p class=\"font-weight-light small-text text-muted mb-0\">Generales</p>" + Environment.NewLine +
                            "    </a>" + Environment.NewLine +
                            "    <a class=\"dropdown-item\" href=\"/pages/configurations/users.aspx\">" + Environment.NewLine +
                            "        <p class=\"font-weight-light small-text text-muted mb-0\">Usuarios</p>" + Environment.NewLine +
                            "    </a>" + Environment.NewLine +
                            //"    <a class=\"dropdown-item\" href=\"/pages/configurations/systems.aspx\">" + Environment.NewLine +
                            //"        <p class=\"font-weight-light small-text text-muted mb-0\">Equipos / Sistemas</p>" + Environment.NewLine +
                            //"    </a>" + Environment.NewLine +
                            "</div>" + Environment.NewLine +
                            "</li>";
                    LitConfiguraciones.Text = vConfiguracionesHtml;
                }  
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        public void CerrarModal(String vModal)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }
        void CargarNotifiaciones()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

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
                        String vLiteralNotificaciones = "";
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambiosResponse.resultSet1)
                        {
                            int vPaso = 5;
                            switch (item.pasos)
                            {
                                case "1": vPaso = 2; break;
                                case "2": vPaso = 3; break;
                                case "3": vPaso = 4; break;
                                case "4": vPaso = 5; break;
                                case "5": vPaso = 6; break;
                                case "6": vPaso = 6; break;
                            }
                            if (vPaso == 6)
                            {
                                vLiteralNotificaciones += "<a class=\"dropdown-item\" href=\"/pages/services/changes.aspx?id=" + item.idcambio + "#step-" + vPaso + "\">" + Environment.NewLine +
                                    "<div class=\"item-thumbnail\">" + Environment.NewLine +
                                    "    <div class=\"item-icon bg-danger\">" + Environment.NewLine +
                                    "        <i class=\"mdi mdi-lock mx-0\"></i>" + Environment.NewLine +
                                    "    </div>" + Environment.NewLine +
                                    "</div>" + Environment.NewLine +
                                    "<div class=\"item-content\">" + Environment.NewLine +
                                    "    <h6 class=\"font-weight-normal\">Cambio #" + item.idcambio + " Creado</h6>" + Environment.NewLine +
                                    "    <p class=\"font-weight-light small-text mb-0 text-muted\">" + Environment.NewLine +
                                    "      " + item.mantenimientoNombre + Environment.NewLine +
                                    "    </p>" + Environment.NewLine +
                                    "</div>" + Environment.NewLine +
                                    "</a>";
                            }
                            else
                            {
                                vLiteralNotificaciones += "<a class=\"dropdown-item\" href=\"/pages/services/changes.aspx?id=" + item.idcambio + "#step-" + vPaso + "\">" + Environment.NewLine +
                                        "<div class=\"item-thumbnail\">" + Environment.NewLine +
                                        "    <div class=\"item-icon bg-success\">" + Environment.NewLine +
                                        "        <i class=\"mdi mdi-information mx-0\"></i>" + Environment.NewLine +
                                        "    </div>" + Environment.NewLine +
                                        "</div>" + Environment.NewLine +
                                        "<div class=\"item-content\">" + Environment.NewLine +
                                        "    <h6 class=\"font-weight-normal\">Cambio #" + item.idcambio + " Creado</h6>" + Environment.NewLine +
                                        "    <p class=\"font-weight-light small-text mb-0 text-muted\">" + Environment.NewLine +
                                        "      " + item.mantenimientoNombre + Environment.NewLine +
                                        "    </p>" + Environment.NewLine +
                                        "</div>" + Environment.NewLine +
                                        "</a>";
                            }
                        }
                        LitNotificaciones.Text = vLiteralNotificaciones;
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        protected void TxBuscarCambio_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/pages/services/search.aspx?busqueda=" + TxBuscarCambio.Text);
            }
            catch {}
        }

        protected void BtnBugInsert_Click(object sender, EventArgs e)
        {
            try
            {

                if (DDLBugTipos.SelectedValue.Equals("0"))
                {
                    throw new Exception("Por favor seleccione un tipo de error");
                }

                if (TxBugDescripcion.Text.Equals(""))
                {
                    throw new Exception("Por favor escriba una descripción del error");
                }

                String vTipoError = String.Empty;
                switch (DDLBugTipos.SelectedValue)
                {
                    case "1":
                        vTipoError = "Error en información";
                        break;
                    case "2":
                        vTipoError = "Envio de correos";
                        break;
                    case "3":
                        vTipoError = "Procesos";
                        break;
                    case "4":
                        vTipoError = "Otros";
                        break;
                }

                SmtpService vSmtpService = new SmtpService();
                Boolean vMensaje = vSmtpService.EnviarMensaje(
                    "dehenriquez@bancatlan.hn",
                    typeBody.bugs,
                    "Daniel Henriquez",
                    vTipoError,
                    TxBugDescripcion.Text);

                if (vMensaje)
                {
                    DDLBugTipos.SelectedIndex = -1;
                    TxBugDescripcion.Text = String.Empty;

                    LbBugMensaje.Text = "";
                    UpdateBugMensaje.Update();
                    CerrarModal("BugsModal");
                }
                
            }
            catch (Exception Ex)
            {
                LbBugMensaje.Text = Ex.Message;
                UpdateBugMensaje.Update();
            }
        }
    }
}