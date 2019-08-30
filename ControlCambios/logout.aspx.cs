using ControlCambios.classes;
using ControlCambios.messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlCambios
{
    public partial class logout : System.Web.UI.Page
    {
        msgLoginResponse vConfigurations = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
            //Logs vLog = new Logs();
            //vLog.postLog("Logout", "Salida exitosa del sistema", vConfigurations.resultSet1[0].idUsuario);

            Session.RemoveAll();
            Response.Redirect("/login.aspx");
        }
    }
}