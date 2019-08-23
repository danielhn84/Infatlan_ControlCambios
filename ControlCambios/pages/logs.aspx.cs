using ControlCambios.classes;
using ControlCambios.messages;
using ControlCambios.pages.services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlCambios.pages
{
    public partial class logs : System.Web.UI.Page
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
                    CargarLogs();
                }
            }
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        void CargarLogs()
        {
            try
            {
                Logs vLog = new Logs();
                DataTable vDatos = vLog.ObtenerLogs();
                
                GVLog.DataSource = vDatos;
                GVLog.DataBind();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnLimpiarLogs_Click(object sender, EventArgs e)
        {
            try
            {
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();
                if (!vGenerales.PermisosEntrada(Permisos.Administrador, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion");

                Logs vLog = new Logs();
                Boolean vDatos = vLog.BorrarLog();

                if (vDatos)
                {
                    Response.Redirect("/pages/logs.aspx");
                }
                else
                {
                    Mensaje("No se pudo limpiar el log, contacte a sistemas",WarningType.Danger);
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/Default.aspx");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
    }
}
