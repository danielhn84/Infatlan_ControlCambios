using ControlCambios.classes;
using ControlCambios.messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlCambios.pages.configurations
{
    public partial class systems : System.Web.UI.Page
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
                        vLog.postLog("Systems", "Usuario intento ingresar a las configs de sistemas y no tiene permiso", vConfigurations.resultSet1[0].idUsuario);
                        Response.Redirect("/Default.aspx");
                    }
                }
            }
        }
    }
}