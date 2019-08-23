using ControlCambios.classes;
using ControlCambios.messages;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlCambios
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            LdapService vLdap = new LdapService();

            //String[] vTest = vLdap.Groups("dehenriquez", "2560sdm300..2019H");
            //Boolean vLogin = vLdap.ValidateCredentials("ADBancat.hn","dehenriquez", "2560sdm300..2019H");
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxUsername.Text.Equals("") || TxPassword.Text.Equals(""))
                    throw new Exception("Por favor ingrese un usuario o password");

                msgLogin vRequest = new msgLogin()
                {
                    username = TxUsername.Text,
                    password = TxPassword.Text
                };

                String vResponseResult = "";
                HttpService vConector = new HttpService();

                //test vTest = vConector.getTest(vRequest);
                HttpResponseMessage vHttpResponse = vConector.postLogin(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgLoginResponse vLoginResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgLoginResponse>(vResponseResult);

                    if (vLoginResponse.resultSet1[0].error.Equals("Success"))
                    {
                        Session["AUTHCLASS"] = vLoginResponse;
                        Session["AUTH"] = true;
                        Logs vLog = new Logs();
                        vLog.postLog("Login", "Usuario ingresado con exito", TxUsername.Text);

                        Response.Redirect("/default.aspx");
                    }
                    else if (vLoginResponse.resultSet1[0].error.Equals("Error"))
                    {
                        Session["AUTH"] = false;
                        Logs vLog = new Logs();
                        vLog.postLog("Login", "Intento fallido de ingreso", TxUsername.Text);

                        throw new Exception(vLoginResponse.resultSet1[0].mensaje);
                    }
                }
            }
            catch (Exception Ex)
            {
                LbMensaje.Text = Ex.Message;
            }
        }
    }
}